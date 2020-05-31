using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;

using MoonBehaviorEditor.Core;
using MoonBehaviorEditor.Core.Graphs;
using MoonBehaviorEditor.Modals;

using MoonBehavior.BehaviorTrees;

namespace MoonBehaviorEditor.BehaviorTrees
{
    /// <summary>
    /// Behavior tree Graph Module
    /// </summary>
    [System.Serializable,GraphModule("BehaviorTree" , typeof(MoonBT) , ExportFolderName = "Behavior Trees")]
    public class BTGraph : NodeGraph
    {
        /// <summary>
        /// Root node id
        /// </summary>
        [SerializeField]
        private string rootId;

        /// <summary>
        /// Root node instance
        /// </summary>
        [System.NonSerialized]
        private BTNode _rootNode = null;

        /// <summary>
        /// Selected MoonAI instance
        /// </summary>
        [System.NonSerialized]
        private MoonAI _selected = null;

        /// <summary>
        /// User defined scriptName
        /// </summary>
        [System.NonSerialized]
        private string NewScriptName;

        [System.NonSerialized]
        private int NewScriptMode = 0; // 0 : Action , 1 : Decision

        /// <summary>
        /// Stored Behavior tree
        /// </summary>
        [SerializeField]
        private MoonBT _bt;

        /// <summary>
        /// Working BehaviorTree
        /// </summary>
        public MoonBT Bt
        {
            get
            {
                if (!_bt)
                {
                    _bt = AssetDatabase.LoadAssetAtPath<MoonBT>(AssetDatabase.GetAssetPath(GetInstanceID()));
                }
                return _bt;
            }
        }

        /// <summary>
        /// Selected AI 
        /// </summary>
        public MoonAI SelectedAI
        {
            get
            {
                if (EditorApplication.isPlaying)
                {
                    GameObject obj = Selection.activeGameObject;

                    if (obj != null)
                    {
                        MoonAI ai = obj.GetComponent<MoonAI>();
                        if (ai != null && ai != _selected)
                        {
                            _selected = ai;
                        }
                    }

                    return _selected;
                }
                return null; 
            }
        }
        
        /// <summary>
        /// Are the node ID the root node ?
        /// </summary>
        /// <param name="nodeid">ID</param>
        /// <returns>True if is root</returns>
        public bool CheckRoot(string nodeid)
        {
            return string.Equals(nodeid, rootId);
        }

        /// <summary>
        /// Root node
        /// </summary>
        public BTNode RootNode
        {
            get
            {
                if (!_rootNode || _rootNode.ID != rootId)
                {
                    _rootNode = FindNode(rootId) as BTNode;  
                }

                return _rootNode;
            }
        }

        /// <summary>
        /// Sets a node as the root node ( Must be a Decision node )
        /// </summary>
        /// <param name="node">Node</param>
        public void SetRoot(BTNode node)
        {
            if (node != null && node.ID != rootId && node.IsDecision)
            {
                rootId = node.ID;
                Bt.Root = (Decision)node.Task;
            }
        }
        
        /// <summary>
        /// Node task type
        /// </summary>
        public override System.Type TaskType
        {
            get
            {
                return typeof(Task);
            }
        }

        /// <summary>
        /// Node type
        /// </summary>
        protected override System.Type NodeType
        {
            get
            {
                return typeof(BTNode);
            }
        }
        
        /// <summary>
        /// Toolbar extra elements
        /// </summary>
        public override void OnToolBarGUI()
        {
            GUILayout.Label(name + " (BehaviorTree)", GUI.skin.FindStyle("PreToolbar"));
        }

        /// <summary>
        /// Canvas extra elements
        /// </summary>
        public override void OnCanvasGUI()
        {
            if (!RootNode)
            {
                GUI.DrawTexture(new Rect(10, Screen.height - 80, 30, 30), EditorGUIUtility.FindTexture("console.erroricon"));
                MoonGUI.DrawCanvasLabel(new Rect(40, Screen.height - 80, Screen.width - 40, 30),
                    new GUIContent("No root node asigned !"), Color.white, 20, false);
            }
        }
        
        /// <summary>
        /// BT canvas extra context menu
        /// </summary>
        /// <returns>Extra context meny elements</returns>
        public override Dictionary<string, System.Action> CanvasContextMenu(MoonEditorController controller)
        {
            Dictionary<string, System.Action> dic = new Dictionary<string, System.Action>();
            dic.Add("$", null);
            dic.Add("Create/Action", () => 
            {
                NewScriptMode = 0;
                controller.CurrentGUIState = 4;
            });

            dic.Add("Create/Decision", () => 
            {
                NewScriptMode = 1;
                controller.CurrentGUIState = 4;
            });

            return dic;
        }

        /// <summary>
        /// BT Canvas extra GUI States
        /// </summary>
        /// <param name="controller">MoonEditor Controller</param>
        /// <returns>Defined gui states</returns>
        public override Dictionary<int, System.Action> CanvasGUIStates( MoonEditorController controller )
        {
            Dictionary<int, System.Action> dic = new Dictionary<int, System.Action>();

            dic.Add(4, () =>
           {
               MoonGUI.DrawGrid(new Rect(0, 17, Screen.width, Screen.height), Vector2.zero);
               NewScriptName = MoonModal.InputPanel(new Vector2(200, 100), "Create " + ((NewScriptMode == 0) ? " Action" : "Decision"), "Ok", "Cancel", "Class Name", NewScriptName,
                   () =>
                   {
                       if (MoonIO.ValidFileName(NewScriptName))
                       {
                           if (MoonIO.ValidClassName(NewScriptName))
                           {
                               string Savefolder = MoonSettings.ActionsSaveDirectory + "/BehaviorTrees";

                               if (NewScriptMode >= 1)
                               {
                                   // create Decition
                                   Savefolder += "/_Decisions";
                               }
                               CreateBTScript(Savefolder, NewScriptMode == 0 , NewScriptName);
                               NewScriptName = string.Empty;
                               controller.CurrentGUIState = 0;
                           }
                       }
                   }, () => 
                   {
                       NewScriptName = string.Empty;
                       controller.CurrentGUIState = 0;
                   });
           });

            return dic; 
        }
        
        /// <summary>
        /// Create a new C# BehaviorTree Action Script Based on template
        /// </summary>
        /// <param name="savefolder">Script save folder</param>
        /// <param name="isaction">Should make an Action or a Decision (false) ?</param>
        private static void CreateBTScript(string savefolder , bool isaction , string scriptName )
        {
            MoonIO.ValidateFolder(savefolder);

            string savep_rel = savefolder + "/" + scriptName + ".cs";

            string savep = savep_rel.Replace("Assets", Application.dataPath).Replace(@"\" , @"/");

            string templatePath = "ScriptTemplates" + ( isaction ? "/BTActionTemplate" : "/BTDecisionTemplate");

            TextAsset ScriptTemplateAsset = MoonResources.LoadCustomResource<TextAsset>(templatePath); 

            if (ScriptTemplateAsset != null )
            {
                string scriptTemplate = ScriptTemplateAsset.text.Replace("#NAME#", scriptName);

                System.IO.File.WriteAllText(savep, scriptTemplate);

                AssetDatabase.Refresh();

                Object assetsaved = AssetDatabase.LoadAssetAtPath(savep_rel, typeof(TextAsset));

                AssetDatabase.OpenAsset(assetsaved);

            }

        }

        /// <summary>
        /// Called when an node is added to the Grapg
        /// </summary>
        /// <param name="node">Node instance</param>
        public override void OnNodeAdded(Node node)
        {
            Task t = node.Task as Task;
            if (t != null && !Bt.Tasks.Contains(t))
            {
                Bt.Tasks.Add(t);
            }
        }

        /// <summary>
        /// On Node deteled override to check if the deleted node where the root node
        /// </summary>
        public override void OnNodeDeleted(Node node)
        {
            if (string.Equals(node.ID , rootId))
            {
                rootId = string.Empty;
                _rootNode = null; 
            }

            Task t = node.Task as Task;
            if (t != null && Bt.Tasks.Contains(t))
                Bt.Tasks.Remove(t);
        }

        /// <summary>
        /// Node Pasted override to Generate new ID to node task
        /// </summary>
        public override void OnNodePasted(Node node)
        {
            Task tt = (Task)node.Task;
            tt.GenerateID();

            if (!Bt.Tasks.Contains(tt))
            {
                Bt.Tasks.Add(tt);
            }

        }
        
        /// <summary>
        /// Icon name override for node list icons
        /// </summary>
        public override string GetIconName(System.Type BaseTaskType)
        {
            if (BaseTaskType == typeof(Task))
            {
                return "BTAction";
            }else if (BaseTaskType == typeof(Decision))
            {
                return "BTDecision";
            }
            return string.Empty;
        }
    }
}