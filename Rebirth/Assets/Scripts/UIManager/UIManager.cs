using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonManager<UIManager>
{
    List<GameObject> OpenedUIs = new List<GameObject>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && OpenedUIs.Count > 0)
        {
            CloseLatestUI();

            if (OpenedUIs.Count == 0)
            {                
                GameStateManager.Instance.UnlockView();
            }
        }
    }

    private void CloseLatestUI()
    {
        GameObject latestUI = OpenedUIs[OpenedUIs.Count - 1];
        OpenedUIs.RemoveAt(OpenedUIs.Count - 1);
        latestUI.gameObject.SetActive(false);
    }

    public void AddToUIStack(GameObject UI)
    {
        OpenedUIs.Add(UI);
    }

    public void RemoveFromUIStack(GameObject UI)
    {
        if (OpenedUIs.Contains(UI))
        {
            OpenedUIs.Remove(UI);
            UI.gameObject.SetActive(false);

            
        }
    }
}

