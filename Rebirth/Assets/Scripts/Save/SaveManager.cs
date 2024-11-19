using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : SingletonManager<SaveManager>
{
    public float autoSaveInterval = 300f; 
    private bool isAutoSaveEnabled = true;

    private void Start()
    {
        StartCoroutine(AutoSaveCoroutine()); 
    }

    private IEnumerator AutoSaveCoroutine()
    {
        while (isAutoSaveEnabled)
        {
            yield return new WaitForSeconds(autoSaveInterval); 
            SaveGame(); 
            Debug.Log("자동저장 되었습니다.");
        }
    }


    public void SaveGame()
    { 
        InventoryManager.Instance.SaveInventory();
    }

    public void LoadGame()
    { 
        InventoryManager.Instance.LoadInventory();
    }

}
