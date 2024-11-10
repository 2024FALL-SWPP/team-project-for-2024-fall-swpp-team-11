using UnityEngine;

public class CharacterInteract : MonoBehaviour
{
    private IInteractable currentInteractable;

    private void Update()
    {
        FindClosestInteractable();
    }

    private void FindClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        // 이전 상호작용 대상과 다르다면 하이라이트를 업데이트합니다
        if (currentInteractable != closestInteractable)
        {
            if (currentInteractable != null)
                currentInteractable.OnDefocus();
            
            currentInteractable = closestInteractable;
            
            if (currentInteractable != null)
                currentInteractable.OnFocus();
        }
    }

    public void TryInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }
}
