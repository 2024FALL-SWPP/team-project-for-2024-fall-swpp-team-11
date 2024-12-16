using UnityEngine;

public class Anchor : MonoBehaviour
{
    public string anchorID;
    public bool isMazeAnchor = false; // Set this to true for maze anchors in the Inspector.

    void Awake()
    {
        if (string.IsNullOrEmpty(anchorID))
        {
            Debug.LogError("AnchorID is not set on: " + transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isMazeAnchor && other.CompareTag("Player"))
        {
            DimensionManager.Instance.SetCurrentMazeAnchor(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isMazeAnchor && other.CompareTag("Player"))
        {
            DimensionManager.Instance.SetCurrentMazeAnchor(this);
        }
    }
}
