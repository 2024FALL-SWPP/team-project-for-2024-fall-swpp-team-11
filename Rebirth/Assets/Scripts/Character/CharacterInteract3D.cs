using UnityEngine;

public class CharacterInteract3D : MonoBehaviour
{
    private IInteractable currentInteractable;
    [SerializeField] private float interactionRange = 2f;

    private void Update()
    {
        FindClosestInteractable();
    }

    private void FindClosestInteractable()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactionRange);
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
