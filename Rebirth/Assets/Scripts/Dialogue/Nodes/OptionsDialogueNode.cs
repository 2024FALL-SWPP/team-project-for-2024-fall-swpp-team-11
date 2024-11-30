using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TextNodePair
{
    public string optionText;
    public DialogueNode nextNode;
}

[CreateAssetMenu(fileName = "New Options Dialogue Node", menuName = "Dialogue/Options Dialogue Node")]
public class OptionsDialogueNode : QuestDialogueNode
{
    [Header("Transition")]
    [SerializeField] private List<TextNodePair> textNodePairs = new List<TextNodePair>();
    

    private void OnEnable()
    {
        options.Clear();
        isSelfSpeak = true;

        foreach (TextNodePair pair in textNodePairs)
        {
            DialogueOption option = new AlwaysTrueOption()
            {
                optionText = pair.optionText,
                nextNode = pair.nextNode
            };

            options.Add(option);
        }
    }
}