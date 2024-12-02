using System.Collections.Generic;
using UnityEngine;

public class NPCManager : SingletonManager<NPCManager>
{

    private Dictionary<string, bool> npcMetStatus = new Dictionary<string, bool>();

    private static string logPrefix = "[NPCManager] ";

    protected override void Awake()
    {
        base.Awake();

        SaveManager.save += SaveNPCMetStatusToDisk;
        SaveManager.load += LoadNPCMetStatusFromDisk;
    }

    private void SaveNPCMetStatusToDisk()
    {
        DiskSaveSystem.SaveNPCMetStatusToDisk(ref npcMetStatus);
    }

    private void LoadNPCMetStatusFromDisk()
    {
        DiskSaveSystem.LoadNPCMetStatusFromDisk(ref npcMetStatus);
    }

    public void RegisterNPC(string npcName)
    {
        if (!npcMetStatus.ContainsKey(npcName))
        {
            npcMetStatus.Add(npcName, false);
            // Debug.Log(npcMetStatus["우물"]);
        }
    }

    public void MarkNPCAsMet(string npcName)
    {
        if (npcMetStatus.ContainsKey(npcName))
        {
            npcMetStatus[npcName] = true;
        }
        else
        {
            Debug.LogWarning(logPrefix + $"NPC '{npcName}' not registered.");
        }
    }

    public bool HasMetNPC(string npcName)
    {
        if (npcMetStatus.TryGetValue(npcName, out bool hasMet))
        {
            return hasMet;
        }
        Debug.LogWarning(logPrefix + $"NPC '{npcName}' not registered.");
        return false;
    }

    public NPC GetNPC(string npcName)
    {
        GameObject npcObj = GameObject.Find(npcName); // TODO better way to find NPC object. For now, npcName should be matched with GameObject name.
        if (npcObj != null)
        {
            return npcObj.GetComponent<NPC>();
        }
        Debug.LogWarning(logPrefix + $"NPC '{npcName}' not found.");
        return null;
    }
}
