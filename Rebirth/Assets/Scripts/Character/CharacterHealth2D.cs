using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth2D : MonoBehaviour
{
    public Slider slider;

    private void OnEnable()
    {
        // Subscribe to health changes
        if (CharacterStatus.Instance != null)
        {
            CharacterStatus.Instance.OnHealthChanged += UpdateHealthUI;
        }
        Debug.Log("ww");
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (CharacterStatus.Instance != null)
        {
            CharacterStatus.Instance.OnHealthChanged -= UpdateHealthUI;
        }
    }

    private void Start()
    {
        if (CharacterStatus.Instance != null)
        {
            // Initialize the slider with current health
            slider.maxValue = CharacterStatus.Instance.GetMaxHealth();
            slider.value = CharacterStatus.Instance.Health;
        }
        else
        {
            Debug.LogError("CharacterStatus.Instance is null in HealthUI2D");
        }
    }

    private void UpdateHealthUI(int currentHealth)
    {
        Debug.Log(currentHealth);
        slider.value = currentHealth;
    }
}
