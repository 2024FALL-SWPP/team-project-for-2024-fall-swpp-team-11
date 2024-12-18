using UnityEngine;
using System.Collections.Generic;

public class ItemVisibilityController : MonoBehaviour
{
    public List<PlayerState> playerStatesWantToShow;
    void Update()
    {
        if (playerStatesWantToShow.Contains(CharacterStatusManager.Instance.PlayerState))
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void SetVisibility(bool visible)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = visible;
        }
    }
}