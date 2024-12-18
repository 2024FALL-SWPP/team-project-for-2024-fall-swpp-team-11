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
        var loadedData = DiskSaveSystem.LoadNPCMetStatusFromDisk();
        ApplyLoadedNPCMetStatus(loadedData);
    }

    private void ApplyLoadedNPCMetStatus(Dictionary<string, bool> loadedData)
    {
        if (loadedData == null) return;

        foreach (var npc in loadedData)
        {
            if (npcMetStatus.ContainsKey(npc.Key))
            {
                npcMetStatus[npc.Key] = npc.Value;
            }
            else
            {
                npcMetStatus.Add(npc.Key, npc.Value);
            }
        }
    }

    public void RegisterNPC(string npcName)
    {
        if (!npcMetStatus.ContainsKey(npcName))
        {
            npcMetStatus.Add(npcName, false);
        }
    }

    public void MarkNPCAsMet(string npcName)
    {
        Debug.Log($"@@@@@@@ MarkNPCAsMet {npcName}");
        if (npcMetStatus.ContainsKey(npcName))
        {
            npcMetStatus[npcName] = true;
            Debug.Log($"#######{npcName}");
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

    private void OnDestroy()
    {
        SaveManager.save -= SaveNPCMetStatusToDisk;
        SaveManager.load -= LoadNPCMetStatusFromDisk;
    }
}
