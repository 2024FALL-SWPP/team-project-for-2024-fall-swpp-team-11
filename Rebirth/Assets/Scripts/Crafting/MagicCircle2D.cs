using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftRecipe2D
{
    public List<ItemData> ingredientsItems; // Required items for crafting
    public WorldItem result;                // Result of crafting
}

public class MagicCircle2D : MonoBehaviour
{
    public List<CraftRecipe2D> craftRecipes; // List of recipes available for crafting

    [SerializeField] private KeyCode triggerKey = KeyCode.C; // Key to trigger crafting
    [SerializeField] private float craftCooldown = 2f;       // Crafting cooldown time
    [SerializeField] private float activationRange = 5f;     // Player must be within this range to craft
    [SerializeField] private Transform player;               // Player transform for distance checking
    [SerializeField] private SpecialEffect2D specialEffect;  // Special effect handler for crafting
    private float lastCraftTime = -Mathf.Infinity;           // Tracks last craft time
    
    [SerializeField] private Collider2D craftingTableCollider; // Specific collider for the crafting area

    private void Start()
    {
        if (craftingTableCollider == null)
        {
            Debug.LogError("Crafting table collider not found.");
        }

        if (player == null)
        {
            Debug.LogError("Player transform not assigned.");
        }

        if (specialEffect == null)
        {
            Debug.LogError("Special effect handler not assigned.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey) && Vector2.Distance(player.position, transform.position) <= activationRange)
        {
            if (Time.time >= lastCraftTime + craftCooldown)
            {
                Craft();
                lastCraftTime = Time.time; // Update last craft time
            }
        }
    }

    private List<WorldItem> GetItemsOnTable()
    {
        // Detect all items within the specific crafting area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            craftingTableCollider.bounds.center,
            craftingTableCollider.bounds.size,
            0f // No rotation for 2D
        );

        List<WorldItem> itemsOnTable = new List<WorldItem>();
        foreach (Collider2D collider in colliders)
        {
            WorldItem item = collider.GetComponent<WorldItem>();
            if (item != null)
            {
                Debug.Log($"Item found: {item.itemData.itemName}");
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
            Debug.Log("Not enough items to craft.");
            specialEffect.TriggerFailureEffect(craftingTableCollider.bounds.center);
            return;
        }

        WorldItem combinedWorldItem = CombineItems(itemsOnTable);
        if (combinedWorldItem != null)
        {
            Vector3 spawnPosition = craftingTableCollider.bounds.center; // Crafting item at the center
            Instantiate(combinedWorldItem, spawnPosition, Quaternion.identity);
            Debug.Log("Crafted item successfully!");
            Debug.Log($"Crafted item: {combinedWorldItem.itemData.itemName}");

            // Trigger success effect
            specialEffect.TriggerSuccessEffect(spawnPosition);
            
            ClearTable(itemsOnTable); // Destroy the used items
        }
        else
        {
            Debug.Log("Crafting failed. Recipe not found.");
            specialEffect.TriggerFailureEffect(craftingTableCollider.bounds.center);
        }
    }

    private WorldItem CombineItems(List<WorldItem> itemsOnTable)
    {
        HashSet<ItemData> ingredientsSet = new HashSet<ItemData>();
        foreach (WorldItem itemHolder in itemsOnTable)
        {
            ingredientsSet.Add(itemHolder.itemData);
        }

        foreach (CraftRecipe2D recipe in craftRecipes)
        {
            if (ingredientsSet.SetEquals(recipe.ingredientsItems))
            {
                return recipe.result;
            }
        }

        return null; // No matching recipe found
    }

    private void ClearTable(List<WorldItem> itemsOnTable)
    {
        foreach (WorldItem itemHolder in itemsOnTable)
        {
            Debug.Log($"Destroying item: {itemHolder.itemData.itemName}");
            Destroy(itemHolder.gameObject); // Destroy the GameObject of the used items
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (craftingTableCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(craftingTableCollider.bounds.center, craftingTableCollider.bounds.size);
        }

        // Draw activation range for crafting
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}
