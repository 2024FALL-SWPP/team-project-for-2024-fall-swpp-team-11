using UnityEngine;

public class CameraController3D : MonoBehaviour
{
    public float vDist { get; private set; } = 2f;
    public float hDist { get; private set; } = 4f;
    public Transform target;
    private IInputHandler inputHandler;

    [SerializeField] private LayerMask collisionLayers; // 충돌 체크할 레이어 마스크
    [SerializeField] private float minDistanceFromObstacle = 0.1f; // 장애물과의 최소 거리

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
        Vector3 desiredPosition = target.position - transform.forward * hDist + transform.up * vDist;
        Vector3 adjustedPosition = CheckCameraCollision(desiredPosition);
        transform.position = adjustedPosition;
    }

    private Vector3 CheckCameraCollision(Vector3 desiredPosition)
    {
        Vector3 directionToCamera = (desiredPosition - target.position).normalized;
        float distanceToCamera = Vector3.Distance(target.position, desiredPosition);

        RaycastHit hit;
        if (Physics.Raycast(target.position, directionToCamera, out hit, distanceToCamera, collisionLayers, QueryTriggerInteraction.Ignore))
        {
            float safeDistance = hit.distance - minDistanceFromObstacle;
            Vector3 adjustedPosition = target.position + directionToCamera * safeDistance;

            return adjustedPosition;
        }

        return desiredPosition;
    }

    private void FollowCharacterRotation()
    {
        if (GameStateManager.Instance.IsViewLocked) return;

        transform.rotation = inputHandler.GetViewRot();
    }
}