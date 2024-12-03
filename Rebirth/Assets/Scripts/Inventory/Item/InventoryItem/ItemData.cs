using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Dimension dimension;
    public Sprite icon;
    
    public int price;
    public GameObject prefab;
}
