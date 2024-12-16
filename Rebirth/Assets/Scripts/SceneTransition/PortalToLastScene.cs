// Portal.cs
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;

public class PortalToLastScene : Portal
{
    public string gameSceneName;
    public void LoadStartSceneData()
    {
        CharacterStatusData data = DiskSaveSystem.LoadCharacterStatusFromDisk();
        gameSceneName = data.LastScene;
        if (string.IsNullOrEmpty(gameSceneName))
        {
            gameSceneName = "HeroHouse2D";
        }
    }
    public void TransitToLastScene()
    {
        LoadStartSceneData();
        SaveManager.Instance.StartSaving();
        DimensionManager.Instance.RefreshDimension(gameSceneName);

        targetScene = gameSceneName;

        
        Interact();
    }

    protected override void AfterSceneLoad()
    {
        base.AfterSceneLoad();
        targetPosition = GameStateManager.Instance.GetAnchorPosition();
    }
}