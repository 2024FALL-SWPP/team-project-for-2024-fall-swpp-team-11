using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public List<NPCSceneData> npcSceneDatas = new List<NPCSceneData>();
    public List<ItemSceneData> itemSceneDatas = new List<ItemSceneData>();
}

[System.Serializable]
public class NPCSceneData
{
    // example
    public string npcID;
    public Vector3 position;
    public bool hasTalked;
}

[System.Serializable]
public class ItemSceneData
{
    public string itemName;
    public Vector3 position;
}
