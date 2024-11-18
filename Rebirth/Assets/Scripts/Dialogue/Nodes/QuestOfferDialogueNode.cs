using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quest Offer Dialogue Node", menuName = "Dialogue/Quest Offer Dialogue Node")]
public class QuestOfferDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    public DialogueNode questAcceptedNode;
    public DialogueNode questRejectedNode;


    private void OnEnable()
    {
        options.Clear();

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
