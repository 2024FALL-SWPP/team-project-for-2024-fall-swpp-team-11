#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace NekoLegends
{
    
    public class NLEditorInspector : Editor
    {
        protected void ShowLogo()
        {
            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            Color pinkColor = new Color(255f / 255f, 81f / 255f, 115f / 255f);  // FF5173
            centeredStyle.normal.textColor = pinkColor;

            Rect logoRect = EditorGUILayout.GetControlRect(GUILayout.Height(50));

            // Draw labels
            GUI.Label(new Rect(logoRect.x, logoRect.y, logoRect.width, logoRect.height / 3), "/\\_/\\", centeredStyle);            
            GUI.Label(new Rect(logoRect.x, logoRect.y + logoRect.height / 3, logoRect.width, logoRect.height / 3), "Neko", centeredStyle);
            centeredStyle.normal.textColor = Color.white; 
            GUI.Label(new Rect(logoRect.x, logoRect.y + 2 * logoRect.height / 3, logoRect.width, logoRect.height / 3), "L e g e n d s", centeredStyle);

            // Make the entire area clickable
            if (GUI.Button(logoRect, "", GUIStyle.none))
            {
                Application.OpenURL("http://nekolegends.com");
            }
        }

    }
}

#endif
