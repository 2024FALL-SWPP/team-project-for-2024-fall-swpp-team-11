using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NekoLegends
{
    public class FoodDemo : DemoScenes
    {
        [Space]
        [SerializeField] private Button NextBtn, PrevBtn, SushiBoardBtn;

        [SerializeField] private List<GameObject> Foods, SushiBoards;
       
        private int currentFoodIndex, currentSushiBoardIndex;


        protected override void Start()
        {
            base.Start();

            HideFoods();

            if(Foods != null)
                Foods[currentFoodIndex].SetActive(true);
            if (SushiBoards != null)
                SushiBoards[currentSushiBoardIndex].SetActive(true);


            SetDescriptionText(Foods[currentFoodIndex].name);
        }

        protected override void OnEnable()
        {
            RegisterButtonAction(NextBtn, () => BtnPressedHandler("NextBtn"));
            RegisterButtonAction(PrevBtn, () => BtnPressedHandler("PrevBtn"));
            RegisterButtonAction(SushiBoardBtn, () => BtnPressedHandler("SushiBoardBtn"));

            base.OnEnable();
        }

        private void BtnPressedHandler(string in_fromBtn)
        {
            switch (in_fromBtn)
            {
                case "NextBtn":
                    Foods[currentFoodIndex].SetActive(false);
                    currentFoodIndex = (currentFoodIndex + 1) % Foods.Count;
                    Foods[currentFoodIndex].SetActive(true);
                    SetDescriptionText(Foods[currentFoodIndex].name);
                    break;
                case "PrevBtn":
                    Foods[currentFoodIndex].SetActive(false);
                    currentFoodIndex--;
                    if (currentFoodIndex < 0) currentFoodIndex = Foods.Count - 1;
                    Foods[currentFoodIndex].SetActive(true);
                    SetDescriptionText(Foods[currentFoodIndex].name);
                    break;
                case "SushiBoardBtn":
                    SushiBoards[currentSushiBoardIndex].SetActive(false);
                    currentSushiBoardIndex = (currentSushiBoardIndex + 1) % SushiBoards.Count;
                    SushiBoards[currentSushiBoardIndex].SetActive(true);
                    SetDescriptionText(SushiBoards[currentSushiBoardIndex].name);
                    break;


                default:
                    break;
            }
        }

        private void HideFoods()
        {
            foreach (var food in Foods)
            {
                food.SetActive(false);
            }
        }

    }
}
