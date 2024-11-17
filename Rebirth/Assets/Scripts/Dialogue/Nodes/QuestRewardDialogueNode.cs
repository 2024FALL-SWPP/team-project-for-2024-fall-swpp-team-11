using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Reward Dialogue Node")]
public class QuestRewardDialogueNode : DialogueNode
{
    [Header("Quest Data")]
    public QuestData associatedQuest;
    private AlwaysTrueCondition alwaysTrueCondition;

    private void OnEnable()
    {
        // if (options == null || options.Count == 0)
        {
            // TODO: better memeory management
            options = new List<DialogueOption>();

            alwaysTrueCondition = CreateInstance<AlwaysTrueCondition>();            

            DialogueOption option = new DialogueOption
            {
                optionText = "Continue",
                nextNode = null,
                fallbackNode = null,
                conditions = new List<DialogueCondition>(),
                onSelectActions = new UnityEvent()
            };
            option.conditions.Add(alwaysTrueCondition);
            option.onSelectActions.AddListener(() => { 
                Debug.Log("Rewarding quest: " + associatedQuest.questTitle);
                Debug.Log("Reward Item: " + associatedQuest.rewardItem.itemName);
                InventoryManager.Instance.AddItem(associatedQuest.rewardItem);
            });

            options.Add(option);
        }
    }
}
