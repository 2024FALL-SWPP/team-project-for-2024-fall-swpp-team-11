using System;
using UnityEngine;

[System.Serializable]
public class CharacterStatusData
{
    public int Money;
    public int Health;

    public PlayerState PlayerState; // 1: Default, 2:Can use weird Potion, 3:isToxified, 4:Can use Cure Potion , 5:isDetoxified
    public bool CanAccessLibrary;
    public bool IsPaperSfixed;
    public bool IsPaperEfixed;
    public bool IsPaperBfixed;
    public int EndingID;

    public string LastScene;

    public CharacterStatusData()
    {
        Money = CharacterStatusManager.Instance ? CharacterStatusManager.Instance.maxMoney : 100;
        Health = CharacterStatusManager.Instance ? CharacterStatusManager.Instance.maxHealth : 100;

        PlayerState = PlayerState.Default;
        CanAccessLibrary = false;
        IsPaperSfixed = false;
        IsPaperEfixed = false;
        IsPaperBfixed = false;
        EndingID = 1;

        LastScene = "HeroHouse2D";
    }
}

public class CharacterStatusManager : SingletonManager<CharacterStatusManager>
{
    // Health-related variables
    public int maxHealth { get; private set; } = 100;
    public int maxMoney { get; private set; } = 1000;
    public int Health { get; private set; }
    public event Action<int> OnHealthChanged;

    // Money-related variables
    public int Money { get; private set; }
    public event Action<int> OnMoneyChanged;
    // public bool IsDimensionSwitchable { get; private set; }

    public PlayerState PlayerState { get; set; } // 1: Default, 2:isToxified, 3:isDetoxified
    public bool CanAccessLibrary  { get; set; }
    public bool IsPaperBfixed  { get; set; }
    public bool IsPaperSfixed  { get; set; }
    public bool IsPaperEfixed  { get;  set; }
    public int EndingID { get; set; }


    protected override void Awake()
    {
        base.Awake();

        SaveManager.save += SaveCharacterStatusToDisk;
        SaveManager.load += LoadCharacterStatusFromDisk;
    }

    private void SaveCharacterStatusToDisk()
    {
        DiskSaveSystem.SaveCharacterStatusToDisk();
    }

    private void LoadCharacterStatusFromDisk()
    {
        CharacterStatusData data = DiskSaveSystem.LoadCharacterStatusFromDisk();
        Health = data.Health;
        Money = data.Money;

        PlayerState = data.PlayerState;
        CanAccessLibrary = data.CanAccessLibrary;
        IsPaperBfixed = data.IsPaperBfixed;
        IsPaperSfixed = data.IsPaperSfixed;
        IsPaperEfixed = data.IsPaperEfixed;
        EndingID = data.EndingID;
        // IsDimensionSwitchable = data.IsDimensionSwitchable;

        RefreshStatusUI();
    }

    private void Update()
    {
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

    public void RefreshStatusUI()
    {
        NotifyHealthChanged();
        NotifyMoneyChanged();
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

    // public void SetIsDimensionSwitchable(bool val)
    // {
    //     IsDimensionSwitchable = val;
    // }

    private void OnDestroy()
    {
        SaveManager.save -= SaveCharacterStatusToDisk;
        SaveManager.load -= LoadCharacterStatusFromDisk;
    }
    
    public void SetPlayerState(PlayerState state)
    {
        PlayerState = state;
    }

    #region canAccessLibrary
    public bool GetCanAccessLibrary()
    {
        return CanAccessLibrary;
    }

    public void SetCanAccessLibrary(bool canAccess)
    {
        CanAccessLibrary=canAccess;
    }
    #endregion

    #region paper
    public bool GetPaper(string sort)
    {
        if (sort == "s")
        {  
            return IsPaperSfixed;
        }
        else if (sort == "b")
        {
            return IsPaperBfixed;
        }
        else if (sort == "e")
        {
            return IsPaperEfixed;
        }
        else
        {
            return false;
        }
        
    }

    public void SetPaper(bool isFixed, string sort)
    {
        if (sort == "s")
        {  
            IsPaperSfixed = isFixed;
        }
        else if (sort == "b")
        {
            IsPaperBfixed = isFixed;
        }
        else if (sort == "e")
        {
            IsPaperEfixed = isFixed;
        }
    }
    #endregion

}