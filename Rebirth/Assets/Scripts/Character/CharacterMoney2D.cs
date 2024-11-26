using TMPro;
using UnityEngine;

public class CharacterMoney2D : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // UI text element to display money

    private void OnEnable()
    {
 
        CharacterStatus.Instance.OnMoneyChanged += UpdateMoneyUI;
      
    }

    private void OnDisable()
    {
       
        CharacterStatus.Instance.OnMoneyChanged -= UpdateMoneyUI;
        
    }


    private void Start()
    {
    
        UpdateMoneyUI(CharacterStatus.Instance.Money);
        
    }


    private void UpdateMoneyUI(int currentMoney)
    {
      
        moneyText.text = currentMoney.ToString();
    }


}
