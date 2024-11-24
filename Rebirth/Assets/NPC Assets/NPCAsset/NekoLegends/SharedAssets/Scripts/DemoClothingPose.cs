using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoClothingPose : DemoScenes
    {
        [Space]
        [SerializeField] public DemoNekoCharacter[] _nekoCharacters;
        [SerializeField] private Button ModelBtn,PoseBtn, EmotionBtn, ChangeBGBtn, ChangeColorBtn, ChangeTextureBtn;
        [SerializeField] private Sprite[] backgrounds;
        [SerializeField] private ClothingAddOn clothingAddOn;
        [SerializeField] private Color[] lightColors; 
        [SerializeField] private float[] intensities; 
        private int currentBGIndex, currentModelIndex;

        protected override void Start()
        {
            base.Start();
            // Initialize with the first sprite and light properties
            if (backgrounds.Length > 0)
            {
                BGTransform.GetComponent<Image>().sprite = backgrounds[0];
                if (directionalLight != null)
                {
                    ApplyLightSettings(0);
                }
            }


            HideModels();
            NextModel(0);

        }

        protected override void OnEnable()
        {
            if(ModelBtn)
                RegisterButtonAction(ModelBtn, () => BtnPressedHandler("ModelBtn"));

            RegisterButtonAction(PoseBtn, () => BtnPressedHandler("PoseBtn"));
            RegisterButtonAction(EmotionBtn, () => BtnPressedHandler("EmotionBtn"));
            RegisterButtonAction(ChangeBGBtn, () => BtnPressedHandler("ChangeBGBtn"));
            RegisterButtonAction(ChangeColorBtn, () => BtnPressedHandler("ChangeColorBtn"));
            RegisterButtonAction(ChangeTextureBtn, () => BtnPressedHandler("ChangeTextureBtn"));

            base.OnEnable();
        }

        private void BtnPressedHandler(string in_btn)
        {
            switch (in_btn)
            {
                case "ModelBtn":
                    NextModel();
                    break;
                case "PoseBtn":
                    foreach (var item in _nekoCharacters)
                    {
                        if(item.gameObject.activeSelf)
                            item.NextPose();
                    }
                    break;
                case "EmotionBtn":
                    foreach (var item in _nekoCharacters)
                    {
                        if (item.gameObject.activeSelf)
                            item.NextEmotion();
                    }
                    break;
                case "ChangeBGBtn":
                    ChangeBackgroundAndLighting();
                    break;
                case "ChangeColorBtn":
                    clothingAddOn.NextColor();
                    break;
                case "ChangeTextureBtn":
                    clothingAddOn.NextTexture();
                    break;
            }
        }

        private void HideModels()
        {
            if (_nekoCharacters != null && _nekoCharacters.Length > 0)
            {
                for (int i = 0; i < _nekoCharacters.Length; i++)
                {
                    _nekoCharacters[i].gameObject.SetActive(false);
                }
            }
        }

        private void NextModel(int forceShowModelIndex = -1)
        {
            if (_nekoCharacters != null && _nekoCharacters.Length > 0)
            {
                if (forceShowModelIndex >= 0 && forceShowModelIndex < _nekoCharacters.Length)
                {
                    currentModelIndex = forceShowModelIndex;
                }
                else
                {
                    currentModelIndex = (currentModelIndex + 1) % _nekoCharacters.Length;
                }
            
                for (int i = 0; i < _nekoCharacters.Length; i++)
                {
                    _nekoCharacters[i].gameObject.SetActive(i == currentModelIndex);
                }
            }
        }



        private void ChangeBackgroundAndLighting()
        {
            currentBGIndex = (currentBGIndex + 1) % backgrounds.Length;
            BGTransform.GetComponent<Image>().sprite = backgrounds[currentBGIndex];
            this.SetDescriptionText(backgrounds[currentBGIndex].name);
            // Update the directional light's properties
            if (directionalLight != null)
            {
                ApplyLightSettings(currentBGIndex);
            }
        }

        private void ApplyLightSettings(int index)
        {
            if (lightColors.Length > index) directionalLight.color = lightColors[index];
            if (intensities.Length > index) directionalLight.intensity = intensities[index];
        }


    }
}
