#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace NekoLegends
{
    public class ShaderGUIBase : ShaderGUI
    {
        protected void ShowLogo()
        {
            
            GUIStyle debugButtonStyle = new GUIStyle();
            
            debugButtonStyle.normal.background = null; //invisible

            if (GUI.Button(new Rect(0, 0, 1000, 50), "", debugButtonStyle))
            {
                Application.OpenURL("http://nekolegends.com");
            }

            GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            Color pinkColor = new Color(255f / 255f, 81f / 255f, 115f / 255f);  // FF5173
            centeredStyle.normal.textColor = pinkColor;

            GUILayout.Label("/\\_/\\", centeredStyle);
            GUILayout.Space(-5);
            GUILayout.Label("Neko", centeredStyle);
            GUILayout.Space(-5);

            centeredStyle.normal.textColor = Color.white;
            GUILayout.Label("L e g e n d s", centeredStyle);

            centeredStyle.normal.textColor = GUI.skin.label.normal.textColor;
        }
    }
}

#endif
