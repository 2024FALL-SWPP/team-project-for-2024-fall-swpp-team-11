using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class QuestItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image requiredItemIcon; 
    
    private QuestData questData;

    public void Initialize(QuestData data)
    {
        questData = data;
        titleText.text = data.questTitle;
        descriptionText.text = data.questDescription;

        if (questData.requiredItem != null && requiredItemIcon != null)
        {
            requiredItemIcon.sprite = questData.requiredItem.icon;
            requiredItemIcon.gameObject.SetActive(true);
        }
        else if (requiredItemIcon != null)
        {
            requiredItemIcon.gameObject.SetActive(false);
        }
    }
}
