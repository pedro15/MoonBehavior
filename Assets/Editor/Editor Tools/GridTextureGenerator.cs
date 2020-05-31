using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Order all outputs
/// </summary>
//private void OrderOutputs()
//{
//    if (outputs.Count > 0)
//    {
//        float baseoffset = rect.height / 2;

//        float offset = baseoffset - ((outputs.Count * 15) / 2);

//        for (int i = 0; i < outputs.Count; i++)
//        {
//            BTNodeOutput currOut = outputs[i];
//            currOut.SetIndex(this, i);
//            currOut.Offset.y = offset + (i * 15f);
//        }
//    }
//}

public class GridTextureGenerator : EditorWindow
{
    [MenuItem("Tools/GridTextureGenerator")]
    public static void ShowWindow()
    {
        EditorWindow wind = GetWindow<GridTextureGenerator>();
        wind.titleContent.text = "GridTextureGen";
        wind.Show();
    }
    
    private Texture2D GridTexture;
    private Vector2 Size = new Vector2(120, 120);

    private Vector2 Step = new Vector2(8,8);

    private Vector2 GetStep { get { return new Vector2(Size.x / Step.x , Size.y / Step.y); } }

    Color bg = new Color(0.2f, 0.2f, 0.2f);
    Color DarkerColor = Color.black;

    float darkFactor = 0.25f;
    float darkIntersectionFactor = 0.3f;
    float lightFactor = 0.13f;
    float lightIntersectionFactor = 0.1f;

    private void SaveTexure()
    {
        string p = EditorUtility.SaveFilePanel("Save Texture", "Assets", "GridTexture", "png");

        if (!string.IsNullOrEmpty(p) && GridTexture != null)
        {
            byte[] dd = GridTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(p, dd);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private void GenerateGridTexture()
    {
        int width = (int)Size.x;
        int height = (int)Size.y;

        GridTexture = new Texture2D(width, height , TextureFormat.ARGB32 , false);
        GridTexture.hideFlags = HideFlags.DontSave;

        Color dark = Color.Lerp(bg, DarkerColor, darkFactor);
        Color darkIntersection = Color.Lerp(bg, DarkerColor, darkIntersectionFactor);

        Color light = Color.Lerp(bg, DarkerColor, lightFactor);
        Color lightIntersection = Color.Lerp(bg, DarkerColor, lightIntersectionFactor);
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                // Debug.Log("x: " + x + " y:" + y);

                // Left Top edge, dark intersection color
                if (x == 0 && y == 0)
                {
                    GridTexture.SetPixel(x, y, darkIntersection);
                    //Debug.Log("darkIntersection");
                }
                // Left and Top edges, dark color
                else if (x == 0 || y == 0)
                {
                    GridTexture.SetPixel(x, y, dark);
                    //Debug.Log("dark");

                }
                // Finer grid intersection color
                else if (x % GetStep.x == 0 && y % GetStep.y == 0)
                {
                    GridTexture.SetPixel(x, y, lightIntersection);
                    //Debug.Log("lightIntersection");
                }
                // Finer grid color
                else if (x % GetStep.x == 0 || y % GetStep.y == 0)
                {
                    GridTexture.SetPixel(x, y, light);
                    //Debug.Log("light");
                }
                // Background
                else
                {
                    GridTexture.SetPixel(x, y, bg);
                    //Debug.Log("Background");
                }
            }

        }

        GridTexture.Apply();
        Repaint();
    }

    private void OnEnable()
    {
        GenerateGridTexture();
    }

    private void OnGUI()
    {
        Rect vrect = EditorGUILayout.BeginVertical();
        
        Step = EditorGUILayout.Vector2Field("LineStep", Step);

        bg = EditorGUILayout.ColorField("Background", bg);
        DarkerColor = EditorGUILayout.ColorField("Grid Color", DarkerColor);
        darkFactor = EditorGUILayout.Slider("Front factor", darkFactor, 0, 1);
        darkIntersectionFactor = EditorGUILayout.Slider("FrontIntersection factor", darkIntersectionFactor, 0, 1);
        lightFactor = EditorGUILayout.Slider("Back factor", lightFactor, 0, 1);
        lightIntersectionFactor = EditorGUILayout.Slider("BackIntersection factor", lightIntersectionFactor, 0, 1);

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Update Texture"))
        {
            GenerateGridTexture();
        }

        if (GUILayout.Button("Save"))
        {
            SaveTexure();
        }

        GUILayout.Space(10);

        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();

        //GUI.DrawTexture(new Rect(10,vrect.yMax + 5, GridTexture.width, GridTexture.height), GridTexture);

        EditorGUI.DrawPreviewTexture(new Rect(10, vrect.yMax + 5, GridTexture.width, GridTexture.height), GridTexture);

    }

}
