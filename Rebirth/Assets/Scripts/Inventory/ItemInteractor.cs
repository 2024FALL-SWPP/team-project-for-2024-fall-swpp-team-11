using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemInteractor : MonoBehaviour, IInteractable
{
    public Item Item;

    void Pickup()
    {
        Inventory.Instance.Add(Item);
        Destroy(gameObject);
    }

    public void Interact()
    {
        Pickup();
    }

    private void OnMouseDown()
    {
        Pickup();
    }
}
