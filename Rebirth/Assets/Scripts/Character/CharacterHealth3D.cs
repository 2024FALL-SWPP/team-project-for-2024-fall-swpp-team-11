using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth3D : MonoBehaviour
{
    public Slider healthSlider; // Reference to the 3D health slider UI

    private void OnEnable()
    {
  
        CharacterStatus.Instance.OnHealthChanged += UpdateHealthUI;
   
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
 
        CharacterStatus.Instance.OnHealthChanged -= UpdateHealthUI;
        
    }

    private void Start()
    {

        // Initialize the slider with current health
        healthSlider.maxValue = CharacterStatus.Instance.GetMaxHealth();
        healthSlider.value = CharacterStatus.Instance.Health;
    
    }

    private void UpdateHealthUI(int currentHealth)
    {
   
        healthSlider.value = currentHealth;
    }
}
