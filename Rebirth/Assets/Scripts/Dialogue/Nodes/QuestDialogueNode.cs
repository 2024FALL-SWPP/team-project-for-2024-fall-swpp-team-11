using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Dialogue Node")]
public class QuestDialogueNode : DialogueNode
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

            DialogueOption yesOption = new DialogueOption
            {
                optionText = "Yes",
                nextNode = null,
                fallbackNode = null,
                conditions = new List<DialogueCondition>(),
                onSelectActions = new UnityEvent()
            };
            yesOption.conditions.Add(alwaysTrueCondition);
            yesOption.onSelectActions.AddListener(() => { 
                Debug.Log("Adding quest: " + associatedQuest.questTitle);
                QuestManager.Instance.AddQuest(associatedQuest); 
            });

            DialogueOption noOption = new DialogueOption
            {
                optionText = "No",
                nextNode = null,
                fallbackNode = null,
                conditions = new List<DialogueCondition>(),
                onSelectActions = new UnityEvent()
            };
            noOption.conditions.Add(alwaysTrueCondition);
            noOption.onSelectActions.AddListener(() => { 
                Debug.Log("Quest not added: " + associatedQuest.questTitle);
            });

            options.Add(yesOption);
            options.Add(noOption);
        }
    }
}
