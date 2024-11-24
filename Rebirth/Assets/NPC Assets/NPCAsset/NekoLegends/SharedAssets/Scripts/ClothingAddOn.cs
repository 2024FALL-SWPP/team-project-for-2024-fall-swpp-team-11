using UnityEngine;
using System.Collections;

namespace NekoLegends
{
    public class ClothingAddOn : MonoBehaviour
    {
        [Space]
        [SerializeField] protected SkinnedMeshRenderer[] ClothesMeshRenderers;

        [SerializeField] protected float HideDelay, ShowDelay;
        [SerializeField] protected GameObject[] HideTheseOnStart;
        [SerializeField] protected GameObject[] ShowTheseOnStart;


        [Space]
        [SerializeField] protected Material[] ClothesCelColorsMaterials;
        [SerializeField] protected Texture2D[] ClothesTextures;



        protected int currentColorIndex = 0;
        protected int currentTextureIndex = 0;

        protected virtual void Start()
        {
            StartCoroutine(DelayedHide(HideDelay));
            StartCoroutine(DelayedShow(ShowDelay));

        }
        protected IEnumerator DelayedHide(float in_seconds)
        {
            yield return new WaitForSeconds(in_seconds);

            foreach (var item in HideTheseOnStart)
            {
                item.SetActive(false);
            }
        }
        protected IEnumerator DelayedShow(float in_seconds)
        {
            yield return new WaitForSeconds(in_seconds);

            foreach (var item in ShowTheseOnStart)
            {
                item.SetActive(true);
            }
        }

        public virtual void NextColor() {
            if (ClothesCelColorsMaterials.Length == 0) return;

            currentColorIndex = (currentColorIndex + 1) % ClothesCelColorsMaterials.Length;
            Material newMaterial = new Material(ClothesCelColorsMaterials[currentColorIndex]);
            foreach (var currentNekoClothMesh in ClothesMeshRenderers)
            {
                currentNekoClothMesh.material = newMaterial;
                currentNekoClothMesh.material.SetTexture("_Main_Texture", ClothesTextures[currentTextureIndex]);
            }
        }
        
        public virtual void NextTexture() {
            if (ClothesTextures.Length == 0) return;

            currentTextureIndex = (currentTextureIndex + 1) % ClothesTextures.Length;

            foreach (var currentNekoClothMesh in ClothesMeshRenderers)
            {
                currentNekoClothMesh.material.SetTexture("_Main_Texture", ClothesTextures[currentTextureIndex]);
                DemoScenes.Instance.SetDescriptionText("Clothes Texture: " + ClothesTextures[currentTextureIndex].name);
            }
        }
    }

}