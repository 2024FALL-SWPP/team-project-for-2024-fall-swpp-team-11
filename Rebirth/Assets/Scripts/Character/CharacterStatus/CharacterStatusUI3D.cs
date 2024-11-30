using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterStatusUI3D : MonoBehaviour, ICharacterStatusUI
{
    public TextMeshProUGUI moneyText;
    public Slider healthSlider;

    private void Start()
    {
        SetMaxHealth(CharacterStatusManager.Instance.GetMaxHealth());
        UpdateMoneyUI(CharacterStatusManager.Instance.Money);
        UpdateHealthUI(CharacterStatusManager.Instance.Health);
    }

    public void UpdateMoneyUI(int currentMoney)
    {
        moneyText.text = currentMoney.ToString();
    }

    public void UpdateHealthUI(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
    }
}
