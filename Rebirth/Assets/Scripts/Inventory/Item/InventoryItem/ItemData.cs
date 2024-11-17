using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public int value;
    public Sprite icon;
    public GameObject prefab;
}
