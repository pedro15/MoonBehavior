using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using MoonBehaviorEditor.Core.Graphs;

namespace MoonBehaviorEditor
{
    /// <summary>
    /// IO Utiliy
    /// </summary>
    public static class MoonIO
    {
        /// <summary>
        /// Creates graph proyect asset
        /// </summary>
        /// <param name="name">Asset name</param>
        /// <param name="savepath">Save path relative to GraphPathBase</param>
        /// <param name="GraphType">Graph type</param>
        /// <returns>Graph proyect instance</returns>
        public static NodeGraph CreateGraphAsset(string name, string savepath, System.Type GraphType, System.Type ModuleType)
        {
            var ModuleAsset = ScriptableObject.CreateInstance(ModuleType);

            string saveDir = MoonSettings.GraphsSaveDirectory + "/" + savepath;

            ValidateFolder(saveDir);

            string savep = saveDir + "/" + name + ".asset";

            if (AssetExists(savep))
            {
                bool continue_delete = EditorUtility.DisplayDialog("Already exists", "The Graph" + name + " Already exists, Overwrite it ?", "Yes", "No");
                if (continue_delete)
                    AssetDatabase.DeleteAsset(savep);
                else return null;
            }

            AssetDatabase.CreateAsset(ModuleAsset, savep);

            EditorUtility.SetDirty(ModuleAsset);

            NodeGraph GraphAsset = ScriptableObject.CreateInstance(GraphType) as NodeGraph;
            GraphAsset.name = name;
            GraphAsset.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector ;
            EditorUtility.SetDirty(GraphAsset);

            AssetDatabase.AddObjectToAsset(GraphAsset, ModuleAsset);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();

            return GraphAsset;
        }

        public static NodeGraph CopyGraph(NodeGraph original , string name)
        {
            EditorUtility.SetDirty(original);
            AssetDatabase.SaveAssets();

            string Path = AssetDatabase.GetAssetPath(original);
            string savep = Path.Replace(original.name, name);

            if (AssetExists(savep))
            {
                bool continue_delete = EditorUtility.DisplayDialog("Already exists", "A Graph with the same name Already exists, Overwrite it ?", "Yes", "No");
                if (continue_delete)
                    AssetDatabase.DeleteAsset(savep);
                else return null;
            }

            if (AssetDatabase.CopyAsset(Path, savep ))
            {
                AssetDatabase.SaveAssets();
                NodeGraph copy = GetGraphAtPath(savep);
                copy.name = name;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return copy;
            }
            return null; 
        }


        public static NodeGraph GetGraphAtPath(string AssetPath)
        {
            Object[] Assets = AssetDatabase.LoadAllAssetsAtPath(AssetPath);
            for (int i = 0; i < Assets.Length; i++)
            {
                Object currentAsset = Assets[i];
                if (currentAsset is NodeGraph)
                {
                    return (NodeGraph)currentAsset;
                }
            }
            return null;
        }

        /// <summary>
        /// Returns all aviable Graph proyects
        /// </summary>
        /// <param name="searchfilter">User search filter</param>
        /// <returns>GraphFounds</returns>
        public static NodeGraph[] GetAllGraphs(string searchfilter = "")
        {
            string[] assetguids = AssetDatabase.FindAssets(searchfilter + " t:" + typeof(ScriptableObject).Name);

            List<NodeGraph> LoadedGraphs = new List<NodeGraph>();

            if (LoadedGraphs.Count < assetguids.Length)
            {
                for (int i = 0; i < assetguids.Length; i++)
                {
                    string assetpath = AssetDatabase.GUIDToAssetPath(assetguids[i]);
                    NodeGraph foundg = GetGraphAtPath(assetpath);
                    if (foundg != null)
                        LoadedGraphs.Add(foundg);
                }
            }

            return LoadedGraphs.ToArray();
        }

        /// <summary>
        /// Check if an asset exists
        /// </summary>
        /// <param name="path">Asset path relative to Assets folder</param>
        /// <returns>Asset exists boolean</returns>
        public static bool AssetExists(string path)
        {
            string checkp = path.Replace("Assets", Application.dataPath).Replace(@"\" , @"/");

            return File.Exists(checkp);
        }

        /// <summary>
        /// Validate and create Folders from the given path if the directory not exists
        /// </summary>
        /// <param name="path">Folder path relative to Assets folder</param>
        public static void ValidateFolder(string path)
        {
            string checkp = path.Replace("Assets", Application.dataPath);

            if (!Directory.Exists(checkp))
            {
                Directory.CreateDirectory(checkp);
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// Are the filename valid ?
        /// </summary>
        /// <param name="name">Filename</param>
        /// <returns>Returns true if the filename is valid</returns>
        public static bool ValidFileName(string name)
        {

            if (string.IsNullOrEmpty(name)) return false; 

            char[] invalidChars = Path.GetInvalidFileNameChars();
            
            foreach (char c in name)
            {
                if (invalidChars.Contains(c))
                {
                    Debug.LogWarning("[MoonBehavior] Invalid name");
                    return false;
                }
            }
            return true; 
        }

        public static bool ValidClassName(string name)
        {
            string invalidChars = "%^&*()+-*/*#@!$~,.;'";
            if (!invalidChars.Any((char c) => name.Contains(c)))
                return true;
            else
            {
                Debug.LogWarning("[MoonBehavior] Invalid class name.");
                return false; 
            }
        }

    }
}
