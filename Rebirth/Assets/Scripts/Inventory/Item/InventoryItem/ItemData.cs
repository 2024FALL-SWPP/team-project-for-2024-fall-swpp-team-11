using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName ="Item/Create New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public int value;
    public Dimension dimension;
    public Sprite icon;
    public GameObject prefab;
}
