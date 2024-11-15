using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData = new GameData(); 

    void Start()
    {

    }

    public void SaveGame()
    { 
        SaveSystem.SaveGame(gameData);
        InventoryManager.Instance.SaveInventory();
    }

    public void LoadGame()
    { 
        SaveSystem.LoadGame();
        InventoryManager.Instance.LoadInventory();
    }
}
