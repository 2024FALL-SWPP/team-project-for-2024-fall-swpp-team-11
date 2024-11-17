using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Plain Next Dialogue Node", menuName = "Dialogue/Quest Dialogue Node")]
public class QuestOfferDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    public DialogueNode questAcceptedNode;
    public DialogueNode questRejectedNode;


    private void OnEnable()
    {
        QuestOfferAcceptOption yesOption = new QuestOfferAcceptOption(associatedQuest)
        {
            optionText = "Yes",
            nextNode = questAcceptedNode
        };

        DialogueOption noOption = new QuestOfferRejectOption(associatedQuest)
        {
            optionText = "No",
            nextNode = questRejectedNode
        };

        options.Add(yesOption);
        options.Add(noOption);
    }
}
