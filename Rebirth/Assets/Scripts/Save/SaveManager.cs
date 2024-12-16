using System.Collections;
using UnityEngine;

public class SaveManager : SingletonManager<SaveManager>
{
    public static event System.Action save;
    public static event System.Action load;
    public float autoSaveInterval = 300f;
    public bool isSaveMangerEnabled = false;
    private bool isAutoSaveEnabled = true;

    private void Start()
    {
        if (!isSaveMangerEnabled) return;

        // LoadGame();
        // StartCoroutine(AutoSaveCoroutine());
    }

    public void StartSaving()
    {
        LoadGame();
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
        save?.Invoke();
    }

    public void LoadGame()
    {
        load?.Invoke();
    }
}