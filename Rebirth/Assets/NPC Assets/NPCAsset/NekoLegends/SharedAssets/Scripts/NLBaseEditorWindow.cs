
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEditor;

namespace NekoLegends
{
    public class NLBaseEditorWindow : EditorWindow
    {

        protected void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        protected void ShowLogo()
        {
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            Color pinkColor = new Color(255f / 255f, 81f / 255f, 115f / 255f); // FF5173
            centeredStyle.normal.textColor = pinkColor;

            // Draw the first two labels in pink
            GUILayout.Label("/\\_/\\", centeredStyle);
            GUILayout.Space(-5);
            GUILayout.Label("Neko", centeredStyle);
            GUILayout.Space(-5);

            // Change the color for the "Legends" label
            centeredStyle.normal.textColor = Color.white;
            GUILayout.Label("L e g e n d s", centeredStyle);

            // Check if the "Legends" label was clicked
            Rect labelRect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.MouseUp && labelRect.Contains(Event.current.mousePosition))
            {
                Application.OpenURL("http://nekolegends.com");
            }

            // Restore the original color for subsequent labels
            centeredStyle.normal.textColor = pinkColor;
        }



    }
}
#endif