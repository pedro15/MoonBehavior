using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MoonBehaviorEditor
{ 
    /// <summary>
    /// GUI extra methods for manual positioning
    /// </summary>
    public static class MoonGUI
    {
        private static Texture2D NodeLineTex;

        /// <summary>
        /// Draws canvas grid
        /// </summary>
        /// <param name="canvas">Canvas rect</param>
        /// <param name="panOffset">Pan offset</param>
        /// <param name="zoom">Zoom divisor</param>
        public static void DrawGrid(Rect canvas, Vector2 panOffset, float zoom = 1)
        {
            if (Event.current.type == EventType.Repaint)
            {


                Rect rect = new Rect(canvas);

                rect.position = new Vector2(0, 18);

                Vector2 center = rect.size * 0.5f;

                Texture2D gridTex = MoonResources.GridTexture;

                float z = zoom;

                // Offset from origin in tile units
                float xOffset = -(center.x * z + panOffset.x) / gridTex.width;
                float yOffset = ((center.y - rect.size.y) * z + panOffset.y) / gridTex.height;

                Vector2 tileOffset = new Vector2(xOffset, yOffset);

                // Amount of tiles
                float tileAmountX = Mathf.Round(rect.size.x * z) / gridTex.width;
                float tileAmountY = Mathf.Round(rect.size.y * z) / gridTex.height;

                Vector2 tileAmount = new Vector2(tileAmountX, tileAmountY);

                // Draw tiled background

                GUI.DrawTextureWithTexCoords(rect, gridTex, new Rect(tileOffset, tileAmount));
            }

        }

        /// <summary>
        /// Draws a Bezier line
        /// </summary>
        /// <param name="pointA">Start point</param>
        /// <param name="pointB">End Point</param>
        /// <param name="color">Color</param>
        /// <param name="DrawArrow">Should draw white arrow on end point ?</param>
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color , bool DrawArrow = false)
        {
            if (pointA == Vector2.zero) return;

            if (!NodeLineTex) NodeLineTex = MoonResources.GenerateLineText();

            float tanimpulse = 30;
            
            Vector2 center = (pointA + pointB) / 2;

            Rect crect = new Rect(center.x - 2.5f , center.y - 2.5f , 5, 5);
            
            Vector3 startTan = pointA + Vector2.right * tanimpulse;
            Vector3 endTan = pointB - Vector2.right * tanimpulse;

            if (crect.Contains(new Vector2(center.x, pointA.y)) || crect.Contains(new Vector2(center.x, pointB.y)))
            {
                Handles.DrawAAPolyLine(NodeLineTex, 2.5f, new Vector3[] { pointA, pointB });
            }else
            {
                Handles.DrawBezier(pointA, pointB, startTan, endTan, color, NodeLineTex, 2.5f);
            }
            
            if (DrawArrow)
            {
               GUI.Box(new Rect(new Vector2(pointB.x  , pointB.y - 4), new Vector2(8,8)), "", MoonResources.ArrowStyle);
            }

        }

        /// <summary>
        /// Draws a Bezier line with label in middle point
        /// </summary>
        /// <param name="pointA">Start point</param>
        /// <param name="pointB">End point</param>
        /// <param name="color">Line color</param>
        /// <param name="label">Label content</param>
        /// <param name="DrawArrow">Should draw white arrow on end point ?</param>
        public static void DrawLine(Vector2 pointA , Vector2 pointB , Color color , GUIContent label ,  bool DrawArrow = false )
        {
            DrawLine(pointA, pointB, color, DrawArrow);

            Vector2 center = (pointA + pointB) / 2;
            float w = MoonResources.LabelOrderStyle.CalcSize(label).x + 0.5f;

            float h = 15;

            GUI.Label(new Rect(center.x - (w / 2 ) , center.y - 11 , w  , h), label , 
                MoonResources.LabelOrderStyle );
        }
        
        /// <summary>
        /// Draws a label to be used in canvas space
        /// </summary>
        /// <param name="rect">Label rect</param>
        /// <param name="content">Label content</param>
        /// <param name="col">Label color</param>
        /// <param name="FontSize">Label font size</param>
        /// <param name="bold">Bold label ?</param>
        public static void DrawCanvasLabel(Rect rect , GUIContent content , Color col , int FontSize = 14 , bool bold = false)
        {
            GUIStyle labelstyle = new GUIStyle(GUI.skin.label)
            {
                normal = new GUIStyleState()
                {
                    textColor = col
                },
                fontSize = FontSize,
                fontStyle = bold ? FontStyle.Bold : FontStyle.Normal
            };
            GUI.Label(rect, content , labelstyle);
        }

        /// <summary>
        /// Draws a label to be used in canvas space
        /// </summary>
        /// <param name="rect">Label rect</param>
        /// <param name="content">Label content</param>
        /// <param name="FontSize">Label font size</param>
        /// <param name="bold">Bold label ?</param>
        public static void DrawCanvasLabel(Rect rect, GUIContent content, int FontSize = 14 , bool bold = false)
        {
            DrawCanvasLabel(rect, content, new Color(1, 1, 1, 0.5f), FontSize , bold);
        }
        
        /// <summary>
        /// Search text field using unity styles
        /// </summary>
        /// <param name="rect">Display rect</param>
        /// <param name="text">Text string reference</param>
        /// <returns>search text</returns>
        public static string SearchTextField( Rect rect , string text )
        {
            Rect searchRech_l = new Rect(rect.x , rect.y , rect.width - 15 , rect.height);

            text = EditorGUI.TextField(searchRech_l, text, GUI.skin.FindStyle("SearchTextField"));

            Rect searchRect_r = new Rect(searchRech_l.xMax, searchRech_l.y, 15 , searchRech_l.height);

            GUI.Box(searchRect_r, "", GUI.skin.FindStyle("SearchCancelButtonEmpty"));
            
            return text;
        }
    }
}