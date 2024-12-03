using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftRecipe
{
    public List<ItemData> ingredientsItems;
    public WorldItem result;
}

public class MagicCircle3D : MonoBehaviour
{
    public List<CraftRecipe> craftRecipes;

    [SerializeField] private SpecialEffect specialEffect;
    [SerializeField] private KeyCode triggerKey = KeyCode.C;
    [SerializeField] private float craftCooldown = 2f; // Cooldown
    [SerializeField] private float activationRange = 5f; // craft dist
    private float lastCraftTime = -Mathf.Infinity; // track time
    private Collider detectionCollider;

    private void Start()
    {
        detectionCollider = GetComponent<Collider>();
        if (detectionCollider == null)
        {
            Debug.LogError("Detection collider not found.");
        }

        if (specialEffect == null)
        {
            Debug.LogError("Special effect not found.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (IsPlayerInRange())
            {
                // Check cooldown
                if (Time.time >= lastCraftTime + craftCooldown)
                {
                    Craft();
                    lastCraftTime = Time.time; // Update last craft time
                }
            }
        }
    }

    private bool IsPlayerInRange()
    {
        Vector3 colliderCenter = detectionCollider.bounds.center;
        Collider[] hitColliders = Physics.OverlapSphere(colliderCenter, activationRange);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private List<WorldItem> GetItemsOnTable()
    {
        Collider[] colliders = Physics.OverlapBox(
            detectionCollider.bounds.center,
            detectionCollider.bounds.extents
        );

        List<WorldItem> itemsOnTable = new List<WorldItem>();
        foreach (Collider collider in colliders)
        {
            WorldItem item = collider.GetComponent<WorldItem>();
            if (item != null)
            {
                Debug.Log(item.itemData.itemName);
                itemsOnTable.Add(item);
            }
        }

        return itemsOnTable;
    }

    private void Craft()
    {
        List<WorldItem> itemsOnTable = GetItemsOnTable();
        Debug.Log($"Items on table: {itemsOnTable.Count}");

        if (itemsOnTable.Count < 2)
        {
            specialEffect.TriggerFailureEffect(detectionCollider.bounds.center);
            return;
        }

        WorldItem combinedWorldItem = CombineItems(itemsOnTable);
        if (combinedWorldItem != null)
        {
            Vector3 spawnPosition = detectionCollider.bounds.center + Vector3.up * 1;
            Instantiate(combinedWorldItem, spawnPosition, Quaternion.identity);
            Debug.Log("Crafted item!");

            specialEffect.TriggerSuccessEffect(detectionCollider.bounds.center);
            ClearTable(itemsOnTable); // Destroy combination items
        }
        else
        {
            specialEffect.TriggerFailureEffect(detectionCollider.bounds.center);
        }
    }

    private WorldItem CombineItems(List<WorldItem> itemsOnTable)
    {
        HashSet<ItemData> ingredientsSet = new HashSet<ItemData>();
        foreach (WorldItem itemHolder in itemsOnTable)
        {
            ingredientsSet.Add(itemHolder.itemData);
        }

        foreach (CraftRecipe recipe in craftRecipes)
        {
            if (ingredientsSet.SetEquals(recipe.ingredientsItems))
            {
                return recipe.result;
            }
        }

        return null;
    }

    private void ClearTable(List<WorldItem> itemsOnTable)
    {
        foreach (WorldItem itemHolder in itemsOnTable)
        {
            Debug.Log($"Destroying item: {itemHolder.itemData.itemName}");
            Destroy(itemHolder.gameObject); // Destroy the GameObject associated with the ItemHolder
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(detectionCollider.bounds.center, detectionCollider.bounds.size);
        }

        // Draw activation range in the scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}
