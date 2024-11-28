using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStatusManager : SingletonManager<CharacterStatusManager>
{
    // Health-related variables
    private int maxHealth = 100;
    private int maxMoney = 99999;
    public int Health { get; private set; }
    public event Action<int> OnHealthChanged;

    // Money-related variables
    public int Money { get; private set; }
    public event Action<int> OnMoneyChanged;

    private bool isInitialized = false;

    protected override void Awake()
    {
        base.Awake();

        Health = maxHealth;
        Money = maxMoney;
    }

    private void Initialize()
    {
        isInitialized = true;
        // Initialize health and money
        Health = maxHealth;
        Money = maxMoney;
        Debug.Log($"CharacterStatus initialized: Money = {Money}, Health = {Health}");

        NotifyHealthChanged();
        NotifyMoneyChanged();
    }

    private void Update()
    {
        if (!isInitialized && DimensionManager.Instance != null)
        {
            Initialize();
        }
        
        // For testing purposes
        // Check if the space bar is pressed for health
        if (Input.GetKeyDown(KeyCode.N))
        {
            UpdateHealth(-10);
            Debug.Log("Health decreased by 10. Current Health: " + Health);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UpdateHealth(10);
            Debug.Log("Health increased by 10. Current Health: " + Health);
        }

        // Check if the 'M' key is pressed for money 
        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateMoney(-10);
            Debug.Log("Money decreased by 10. Current Money: " + Money);
        }

        // Check if the 'J' key is pressed for money 
        if (Input.GetKeyDown(KeyCode.J))
        {
            UpdateMoney(10);
            Debug.Log("Money increased by 10. Current Money: " + Money);
        }
    }


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