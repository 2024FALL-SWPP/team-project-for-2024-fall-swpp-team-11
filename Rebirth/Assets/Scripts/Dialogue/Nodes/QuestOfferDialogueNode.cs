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
        isSelfSpeak = true;
        options.Clear();

        QuestOfferAcceptOption yesOption = new QuestOfferAcceptOption(associatedQuest)
        {
            optionText = "그래 뭐.. 알겠어.",
            nextNode = questAcceptedNode
        };

        DialogueOption noOption = new QuestOfferRejectOption(associatedQuest)
        {
            optionText = "내가 왜 해야 하는데?",
            nextNode = questRejectedNode
        };

        options.Add(yesOption);
        options.Add(noOption);
    }
}
