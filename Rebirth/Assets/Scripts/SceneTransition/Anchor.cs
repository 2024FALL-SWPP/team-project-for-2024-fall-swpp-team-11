using UnityEngine;

public class Anchor : MonoBehaviour
{
    public string anchorID;

    void Awake()
    {
        if (string.IsNullOrEmpty(anchorID))
        {
            Debug.LogError("AnchorID가 설정되지 않았습니다: " + transform.position);
        }
    }
}
