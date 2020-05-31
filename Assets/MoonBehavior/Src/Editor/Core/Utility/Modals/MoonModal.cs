using UnityEngine;
using UnityEditor;
using MoonBehaviorEditor.Core.Graphs;
using MoonBehaviorEditor.Modals.Nodes;
using System.Linq;

namespace MoonBehaviorEditor.Modals
{
    /// <summary>
    /// Panel drawing Utility
    /// </summary>
    public static class MoonModal
    {
        /// <summary>
        /// Displays open graph panel
        /// </summary>
        /// <param name="scrollpoint">Open graph scroll point</param>
        /// <param name="searchfilter">Open graph search filter</param>
        /// <param name="Graphs">Graphs list</param>
        /// <param name="OnOpen">On open action</param>
        /// <param name="OnCancel">On cancel action</param>
        /// <param name="OnDelete">On delete action</param>
        public static void OpenGraphPanel(ref Vector2 scrollpoint , ref string searchfilter , NodeGraph[] Graphs  , 
            System.Action<NodeGraph> OnOpen , System.Action OnCancel , System.Action OnDelete )
        {
            Vector2 size = new Vector2(350, 320);
            
            Rect r = new Rect((Screen.width / 2) - (size.x / 2), (Screen.height / 2) - (size.y / 2), size.x, size.y);

            GUI.Box(r, "", GUI.skin.FindStyle("WindowBackground"));


            GUI.BeginGroup(r);

            GUI.Box(new Rect(0, 0 , r.width - 50, 15), "", EditorStyles.toolbar);

            GUI.Label(new Rect(5, 0, 100, 20), "Open Graph" );

            if (GUI.Button(new Rect(r.width - 50, 0 , 50 , 15), "Close", EditorStyles.toolbarButton))
            {
                OnCancel.Invoke();
            }

            Rect searchRect = new Rect(10, 28 , r.width - 20, 15);

            searchfilter = MoonGUI.SearchTextField(searchRect, searchfilter);
            
            //EditorGUI.DrawRect(searchRect, Color.blue);

            Rect ScrollRect = new Rect(5, 50 , r.width - 15, r.height  - 60);

            GUI.Box(ScrollRect, "", GUI.skin.FindStyle("AnimationKeyframeBackground"));
            
            scrollpoint = GUI.BeginScrollView(ScrollRect, scrollpoint, new Rect(0, 0, ScrollRect.width - 20 ,  Graphs.Length * 25 ) , false , true);

            if (Graphs != null && Graphs.Length > 0)
            {
                for (int i = 0; i < Graphs.Length; i++)
                {
                    Rect baserect = new Rect(ScrollRect.x, i * 25, ScrollRect.width - 25, 20);

                    NodeGraph current = Graphs[i];

                    if (current != null)
                    {
                        GraphModuleAttribute module = MoonReflection.GetAttribute(current.GetType(), typeof(GraphModuleAttribute), true)
                            as GraphModuleAttribute;

                        if (module != null)
                        {
                            GUI.Box(baserect, "");

                            GUI.BeginGroup(baserect);

                            Rect labelrect = new Rect(5, 0, baserect.width * 0.8f, baserect.height);


                            Rect namerect = new Rect(labelrect.x, labelrect.y, labelrect.width * 0.55f, labelrect.height);

                            //EditorGUI.DrawRect(namerect, Color.blue);


                            GUI.Label(namerect, current.name);

                            Rect TypeRect = new Rect(namerect.xMax + 5, labelrect.y, labelrect.width * 0.45f, labelrect.height);

                            GUI.Label(TypeRect, module.Name, GUI.skin.FindStyle("PreToolbar"));

                            //EditorGUI.DrawRect(TypeRect, Color.red);

                            Rect loadrect = new Rect(TypeRect.xMax + 5, TypeRect.y + 2, 15, 15);

                            if (GUI.Button(loadrect, new GUIContent("", "Load"), GUI.skin.FindStyle("OL Plus")))
                            {
                                OnOpen.Invoke(current);
                            }

                            Rect removerect = new Rect(loadrect.xMax + 5, loadrect.y, 15, 15);

                            if (GUI.Button(removerect, new GUIContent("", "Remove"), GUI.skin.FindStyle("OL Minus")))
                            {
                                bool ok = EditorUtility.DisplayDialog("Delete", "Are you sure to DELETE the graph '" + current.name + "' ?",
                                    "Yes", "No");

                                if (ok)
                                {
                                    AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(current));
                                    OnDelete.Invoke();
                                }
                            }

                            GUI.EndGroup();
                        }
                    }else
                    {
                        OnDelete.Invoke();
                    }
                }
            }
            GUI.EndScrollView();

            GUI.EndGroup();
        }

        /// <summary>
        /// Displays an input panel with text field and ok, cancel buttuns
        /// </summary>
        /// <param name="size">Panel size</param>
        /// <param name="title">Panel title</param>
        /// <param name="ok">Ok text</param>
        /// <param name="cancel">cancel text</param>
        /// <param name="inputTitle">Input label title</param>
        /// <param name="inputvalue">Input label value</param>
        /// <param name="OnOk">On ok click action</param>
        /// <param name="OkCancel">On cancel click action</param>
        /// <returns>input string</returns>
        public static string InputPanel(Vector2 size, string title, string ok, string cancel, string inputTitle, string inputvalue,
            System.Action OnOk, System.Action OkCancel)
        {
            Rect r = new Rect((Screen.width / 2) - size.x * 0.5f, (Screen.height / 2) - size.y * 0.5f, size.x, size.y);

            GUI.Box(r, title, GUI.skin.window);

            float lw = GUI.skin.label.CalcSize(new GUIContent(inputTitle)).x;

            GUI.Label(new Rect(r.x + ((r.width / 2) - lw * 0.5f), r.y + 18, lw, 18), inputTitle);

            Rect InputRect = new Rect(r.x + 10, r.y + 41, r.width - 20, 18);

            inputvalue = EditorGUI.TextField(InputRect, inputvalue);
            
            
            float btn_w = ((r.width) - 25) * 0.5f;

            Rect btn_l = new Rect(InputRect.x, InputRect.yMax + 5, btn_w, 25);

            if (GUI.Button(btn_l, ok))
            {
                OnOk.Invoke();
            }

            Rect btn_r = new Rect(btn_l.xMax + 5, btn_l.y, btn_w, 25);

            if (GUI.Button(btn_r, cancel))
            {
                OkCancel.Invoke();
            }

            return inputvalue;
        }

        /// <summary>
        /// Displays the add node window panel
        /// </summary>
        /// <param name="Position">Panel position</param>
        /// <param name="searchfilter">Panel search filter string</param>
        /// <param name="NodeScroll">Panel scroll position</param>
        /// <param name="Collections">Node collections to fill panel</param>
        /// <param name="PanelRect">Out panel area rect</param>
        /// <param name="OnAddNode">On add node event</param>
        public static void AddNodePanel( NodeGraph WorkingGraph, Vector2 Position , ref string searchfilter , ref Vector2 NodeScroll , Nodes.NodeCollection[] Collections , out Rect PanelRect , 
            System.Action<System.Type> OnAddNode )
        {
            Rect r = new Rect(new Vector2( Mathf.Clamp(Position.x , 0 , Screen.width - 290) , 
                Mathf.Clamp(Position.y , 0 , Screen.height - 350 )), new Vector2(280,300));

            GUI.Box(r, "", GUI.skin.FindStyle("WindowBackground"));
            
            GUI.BeginGroup(r);
            int totalNodes = Collections.Select((NodeCollection n) => n.Nodes.Length).Sum();
            GUI.Label(new Rect(5, 3 , r.width - 10, 15), "Add node (" + totalNodes + ")" );

            searchfilter = MoonGUI.SearchTextField(new Rect(5, 20, r.width - 10, 15),  searchfilter);

            Rect scroll_rect = new Rect(10, 45, r.width - 20 , r.height - 55);

            GUI.Box(scroll_rect, "", GUI.skin.FindStyle("AnimationKeyframeBackground"));
            

            NodeScroll = GUI.BeginScrollView(scroll_rect, NodeScroll, new Rect(0, 0, scroll_rect.width - 15, 
               NodeCollection.GetCollectionsHeight(Collections)), false , true);

            for ( int i = 0; i < Collections.Length; i++ )
            {
                NodeCollection curr = Collections[i];
                
                float Y_Pos = ((i - 1) >= 0) ? Collections[i - 1].MaxY : i * NodeCollection.NodeHeight; 

                Rect coll_rect = new Rect( 8 , Y_Pos, scroll_rect.width - 30, curr.GetHeight());
                curr.MaxY = coll_rect.y + coll_rect.height; 
                
                GUI.Box(coll_rect, "");

                GUI.BeginGroup(coll_rect);

                GUIStyle collLabelSkin = GUI.skin.FindStyle("AssetLabel");
                GUIContent label_collection = new GUIContent(curr.CategoryName + " (" + curr.Nodes.Length +")" );
                float label_collectionWidth = coll_rect.width * 0.8f;

                GUI.Label(new Rect(25 , 5, label_collectionWidth, 15), label_collection , collLabelSkin);

                if (GUI.Button(new Rect( 5 , 5 , 15 , 15) , "" , GUI.skin.FindStyle
                    ( (curr.FoldOut ? "OL Minus" : "OL Plus") ) ) )
                {
                    curr.FoldOut = !curr.FoldOut;
                }
                
                if (curr.FoldOut)
                {
                    for (int j = 0; j < curr.Nodes.Length; j++)
                    {
                        NodeItem item = curr.Nodes[j];

                        Rect itemrect = new Rect(0, 25 + (j * NodeCollection.NodeHeight), coll_rect.width , NodeCollection.NodeHeight);

                        
                        bool inItem = itemrect.Contains(Event.current.mousePosition);

                        if (inItem)
                        {
                            EditorGUI.DrawRect(itemrect, new Color32(0, 112, 197, 255));

                            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                            {
                                OnAddNode(item.NodeType);
                            }
                        }

                        GUIStyleState _labelcolored = inItem ? new GUIStyleState() { textColor = Color.white } 
                            : EditorStyles.label.normal;
                        
                        GUI.BeginGroup(itemrect);

                        Rect ItemIconRect = new Rect(5, 2.5f , 16, 16);

                        if (item.Icon != null )
                        {
                            GUI.DrawTexture(ItemIconRect, item.Icon);
                        }else
                        {

                        }
                        
                        Rect ItemLabelRect = new Rect(25, 4, itemrect.width - 25, itemrect.height);

                        GUI.Label(ItemLabelRect, item.Name, new GUIStyle(EditorStyles.label)
                        {
                            normal = _labelcolored,
                            alignment = TextAnchor.UpperLeft
                        });

                        GUI.EndGroup();
                        
                    }
                }

                GUI.EndGroup();

            }

            GUI.EndScrollView();

            GUI.EndGroup();

            PanelRect = r;
        }
    }
}
