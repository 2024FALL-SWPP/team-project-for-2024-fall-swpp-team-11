#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
namespace NekoLegends
{
    public class UnityVersionCompatibility : MonoBehaviour
    {
        private const string MessageViewedFlag = "VersionSpecificShaderLoaded";

        // This method gets called after a package import is completed
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            // Check if we've already loaded the shaders for this project
            if (EditorPrefs.GetBool(MessageViewedFlag, false))
            {
                return; // Exit if shaders were already loaded
            }
            bool showDialog = false;
            string dialogTitle = "";
            string dialogMessage = "";
            // Detect Unity version
            string unityVersion = Application.unityVersion;
            
            // Example: Check if the major version is 2021
            if (unityVersion.StartsWith("2021"))
            {
                dialogTitle = "Unity 2021 Detected";
                dialogMessage = "Make sure to install the correct shader packages.  By default, Unity2022(URP 14) is used.";
                showDialog = true;
            }
            else if (unityVersion.StartsWith("2022"))
            {
                Debug.Log("Unity 2022 detected. Shaders are good to go.");
            }
            else
            {
                dialogTitle = "Unsupported Unity Version Detected";
                dialogMessage = "Unity Version: " + unityVersion;
                showDialog = true;
            }

            if (showDialog)
            {
                // Display a popup dialog to the user
                EditorUtility.DisplayDialog(dialogTitle, dialogMessage, "OK");
            }

            EditorPrefs.SetBool(MessageViewedFlag, true);
        }
    }
}
#endif
