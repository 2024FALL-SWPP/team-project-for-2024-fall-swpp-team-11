using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class DemoClothingSets : DemoScenes
    {
        [Space]
        [SerializeField] public GameObject[] sets;
        [SerializeField] private Button ModelBtn;
        private int currentModelIndex;

        protected override void Start()
        {
            base.Start();

            HideModels();
            NextModel(0);

        }

        protected override void OnEnable()
        {
            if(ModelBtn)
                RegisterButtonAction(ModelBtn, () => BtnPressedHandler("ModelBtn"));

            base.OnEnable();
        }

        private void BtnPressedHandler(string in_btn)
        {
            switch (in_btn)
            {
                case "ModelBtn":
                    NextModel();
                    break;
            }
        }

        protected void HideModels()
        {
            if (sets != null && sets.Length > 0)
            {
                for (int i = 0; i < sets.Length; i++)
                {
                    sets[i].gameObject.SetActive(false);
                }
            }
        }

        protected void NextModel(int forceShowModelIndex = -1)
        {
            if (sets != null && sets.Length > 0)
            {
                if (forceShowModelIndex >= 0 && forceShowModelIndex < sets.Length)
                {
                    currentModelIndex = forceShowModelIndex;
                }
                else
                {
                    currentModelIndex = (currentModelIndex + 1) % sets.Length;
                }
            
                for (int i = 0; i < sets.Length; i++)
                {
                    sets[i].gameObject.SetActive(i == currentModelIndex);
                }
            }
        }


    }
}
