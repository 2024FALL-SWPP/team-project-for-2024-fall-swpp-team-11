using System;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public static CharacterStatus Instance { get; private set; }

    // Health-related variables
    private int maxHealth = 100;
    public int Health { get; private set; }
    public event Action<int> OnHealthChanged;

    // Money-related variables
    public int Money { get; private set; }
    public event Action<int> OnMoneyChanged;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Money = 99999; 
        Health = maxHealth; 
    }

    private void Start()
    {
        // Initialize health and money
        Debug.Log($"CharacterStatus initialized: Money = {Money}, Health = {Health}");

        Health = maxHealth;
        Money = 99999; // Starting money
        NotifyHealthChanged();
        NotifyMoneyChanged();
    }

    private void Update()
    {
        // Check if the space bar is pressed for health
        if (Input.GetKeyDown(KeyCode.N)) //테스트용 
        {
            UpdateHealth(-10);
            Debug.Log("Health decreased by 10. Current Health: " + Health);
        }

        // // Check if the 'M' key is pressed for money 테스트용
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateMoney(-10);
            Debug.Log("Money decreased by 10. Current Money: " + Money);
        }

        // Check if the 'X' key is pressed for a big money deduction

    }


    // Health-related methods
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void UpdateHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, maxHealth); // Ensure health stays within range
        NotifyHealthChanged();
    }

    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = Mathf.Max(newMaxHealth, 1); // Ensure max health is always at least 1
        if (Health > maxHealth)
        {
            Health = maxHealth; // Clamp current health if it exceeds new max
        }
        NotifyHealthChanged();
    }

    public void ResetHealth()
    {
        Health = maxHealth;
        NotifyHealthChanged();
    }


    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(Health); // Notify listeners
    }


    //money related methods
    public void UpdateMoney(int amount)
    {
        Money = Mathf.Max(0, Money + amount); // Ensure money does not drop below 0
        NotifyMoneyChanged();
    }

    private void NotifyMoneyChanged()
    {
        OnMoneyChanged?.Invoke(Money); 
    }

    public void ResetMoney(int startingAmount)
    {
        Money = Mathf.Max(0, startingAmount); // Reset money to a specific amount
        NotifyMoneyChanged();
    }
}