using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Vector3 spawnPosition;
    public List<PlayerState> requiredStates;

    private void Start()
    {
        if (itemPrefab == null)
        {
            Debug.LogError("Item prefab not found.");
        }

        if (requiredStates.Count == 0)
        {
            Debug.LogWarning("No required states set.");
        }

        if (spawnPosition == null)
        {
            Debug.LogWarning("No spawn position set.");
        }

        if (requiredStates.Contains(CharacterStatusManager.Instance.PlayerState))
        {
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }
}