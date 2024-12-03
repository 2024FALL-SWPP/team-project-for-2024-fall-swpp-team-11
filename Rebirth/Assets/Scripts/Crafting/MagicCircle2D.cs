using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftRecipe2D
{
    public List<ItemData> ingredientsItems;
    public WorldItem result;                
}

public class MagicCircle2D : MonoBehaviour
{
    public List<CraftRecipe2D> craftRecipes; 

    [SerializeField] private KeyCode triggerKey = KeyCode.C; 
    [SerializeField] private float craftCooldown = 2f;       // Crafting cooldown time
    [SerializeField] private float activationRange = 5f;     
    [SerializeField] private SpecialEffect specialEffect;  
    private float lastCraftTime = -Mathf.Infinity;           // Tracks last craft time
    
    [SerializeField] private Collider2D craftingTableCollider; // Specific collider for the crafting area

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (IsPlayerInRange())
            {
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
        Vector2 colliderCenter = craftingTableCollider.bounds.center;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(colliderCenter, activationRange);
        foreach (var hitCollider in hitColliders)
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
        // Detect all items within the specific crafting area
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            craftingTableCollider.bounds.center,
            craftingTableCollider.bounds.size,
            0f 
        );

        List<WorldItem> itemsOnTable = new List<WorldItem>();
        foreach (Collider2D collider in colliders)
        {
            WorldItem item = collider.GetComponent<WorldItem>();
            if (item != null)
            {
                itemsOnTable.Add(item);
            }
        }

        return itemsOnTable;
    }

    private void Craft()
    {
        List<WorldItem> itemsOnTable = GetItemsOnTable();

        // Check if there are no items on the crafting table
        if (itemsOnTable.Count == 0)
        {
            return; // Exit without triggering any effect
        }

        // Check if there are not enough items to craft
        if (itemsOnTable.Count < 2)
        {
            specialEffect.TriggerFailureEffect(craftingTableCollider.bounds.center);
            return;
        }

        WorldItem combinedWorldItem = CombineItems(itemsOnTable);
        if (combinedWorldItem != null)
        {
            Vector3 spawnPosition = craftingTableCollider.bounds.center; // Craft item at the center
            Instantiate(combinedWorldItem, spawnPosition, Quaternion.identity);

            // Trigger success effect
            specialEffect.TriggerSuccessEffect(spawnPosition);
            
            ClearTable(itemsOnTable); // Destroy the used items
        }
        else
        {
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
           // Debug.Log($"Destroying item: {itemHolder.itemData.itemName}");
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
