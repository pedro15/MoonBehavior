using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using MoonBehaviorEditor.Core;
using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// Asset handler for Open Graphs with unity native Open Asset action
    /// </summary>
    class MoonAssetHandler
    {
        [OnOpenAsset()]
        static bool OpenAsset(int instanceid , int line)
        {
            Object assetObject = EditorUtility.InstanceIDToObject(instanceid);

            if (assetObject is ScriptableObject)
            {
                string AssetPath = AssetDatabase.GetAssetPath(instanceid);
                NodeGraph ng = MoonIO.GetGraphAtPath(AssetPath);
                
                if (ng != null)
                {
                    Object main = AssetDatabase.LoadMainAssetAtPath(AssetPath);
                    if (main.name != ng.name)
                        ng.name = main.name;

                    MoonEditorWindow wind = MoonEditorWindow.LoadWindow();
                    wind.CurrentGraph = ng;
                    return true;
                }

            }
            return false; 
        }
    }
}