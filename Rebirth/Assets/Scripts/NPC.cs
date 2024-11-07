using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public Quest quest;
    public float interactRange;

    public void Interact()
    {
        QuestManager.Instance.AddQuest(quest);
        // TODO: 퀘스트 수락 후 UI 업데이트 또는 대화창 닫기 등 추가 로직 가능
        Debug.Log($"퀘스트 수락: {quest.title}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
}
