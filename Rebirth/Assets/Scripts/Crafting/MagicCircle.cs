using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private GameObject stoneOfTruthPrefab;
    [SerializeField] private GameObject magicBoxPrefab;
    [SerializeField] private GameObject potionOfCurePrefab;
    [SerializeField] private SpecialEffect specialEffect;
    [SerializeField] private Collider detectionCollider;
    [SerializeField] private KeyCode triggerKey = KeyCode.C;
    [SerializeField] private float craftCooldown = 2f; // Cooldown
    [SerializeField] private float activationRange = 5f; // craft dist
    [SerializeField] private Transform player; // player pos
    private float lastCraftTime = -Mathf.Infinity; // track time

    private void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (Vector3.Distance(player.position, transform.position) <= activationRange)
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

    private void Craft()
    {
        Collider[] colliders = Physics.OverlapBox(
            detectionCollider.bounds.center,
            detectionCollider.bounds.extents);

        List<ItemHolder> itemsOnTable = new List<ItemHolder>();
        List<WorldItem> itemlist = new List<WorldItem>();
        foreach (Collider collider in colliders)
        {
            WorldItem item = collider.GetComponent<WorldItem>();
            itemlist.Add(item);
            //Debug.Log($"Item: {item.name}");

            Debug.Log(itemlist.Count);
            ItemHolder itemHolder = collider.GetComponent<ItemHolder>();
            if (itemHolder != null)
            {
                itemsOnTable.Add(itemHolder);
            }
        }

        Debug.Log(itemsOnTable.Count);
        GameObject newItemPrefab = null;

        if (itemsOnTable.Count == 2)
        {
            newItemPrefab = CombineItems(itemsOnTable);
        }

        if (newItemPrefab != null)
        {
            Vector3 spawnPosition = detectionCollider.bounds.center + Vector3.up * 1;

            Instantiate(newItemPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Crafted item!");

            specialEffect.TriggerSuccessEffect(detectionCollider.bounds.center);

            ClearTable(itemsOnTable); // Destroy combination items
        }
        else if (itemlist.Count >= 1)
        {
            specialEffect.TriggerFailureEffect(detectionCollider.bounds.center);
        }
    }

    private GameObject CombineItems(List<ItemHolder> itemHolders)
    {
        Debug.Log($"Item 0: {itemHolders[0].itemData.itemName}");
        Debug.Log($"Item 1: {itemHolders[1].itemData.itemName}");

        HashSet<string> itemNames = new HashSet<string>
        {
            itemHolders[0].itemData.itemName,
            itemHolders[1].itemData.itemName
        };

        if (itemNames.SetEquals(new HashSet<string> { "Pikachu", "Veilasis Flower" }))
        {
            return stoneOfTruthPrefab;
        }
        else if (itemNames.SetEquals(new HashSet<string> { "Elixir Veil", "Dragon Feather" }))
        {
            return magicBoxPrefab;
        }
        else if (itemNames.SetEquals(new HashSet<string> { "Serentis Flower", "Pheonix Rose" }))
        {
            return potionOfCurePrefab;
        }

        return null;
    }

    private void ClearTable(List<ItemHolder> itemHolders)
    {
        foreach (ItemHolder itemHolder in itemHolders)
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
