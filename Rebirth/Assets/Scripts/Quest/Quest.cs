using System;
using UnityEngine;

[Serializable]
public class Quest
{
    public int id;
    public string title;
    public string description;
    public bool isCompleted;
    public ItemData rewardItem;
    public string ability;
    public ItemData requireItem;

}
