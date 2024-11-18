using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float vDist { get; private set; } = 2f;
    public float hDist { get; private set; } = 4f;
    public Transform target;
    private IInputHandler inputHandler;

    void Start()
    {
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
            inputHandler = player.GetComponent<IInputHandler>();
        }
    }


    void Update()
    {
        FollowCharacterRotation();
        FollowCharacterPosition();
    }

    private void FollowCharacterPosition()
    {
        transform.position = target.position - transform.forward * hDist + transform.up * vDist;
    }

    private void FollowCharacterRotation()
    {
        if (GameStateManager.Instance.IsViewLocked)
        {
            transform.rotation = inputHandler.GetViewRot();
        }
    }
}
