
#if UNITY_EDITOR
using UnityEditor;
namespace NekoLegends
{
    public class GrassStylizedShaderInspector : ShaderGUIBase
    {

        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            ShowMainSection(materialEditor, properties);
        }


        protected void ShowMainSection(MaterialEditor materialEditor, MaterialProperty[] properties)
        {

            ShowLogo();
            MaterialProperty _BlendTexture = FindProperty("_BlendTexture", properties);
            MaterialProperty _Blend_Strength = FindProperty("_Blend_Strength", properties); 
            MaterialProperty _Blend_Offset = FindProperty("_Blend_Offset", properties); 


            materialEditor.ShaderProperty(_BlendTexture, "Blend Texture");
            materialEditor.ShaderProperty(_Blend_Strength, _Blend_Strength.displayName);
            materialEditor.ShaderProperty(_Blend_Offset, _Blend_Offset.displayName);


        }

    }

}

#endif