using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteract : MonoBehaviour
{
    public void TryInteract(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, 3f);
        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity; 

        // 가장 가까운 interactable
        foreach (var hitCollider in hitColliders)
        {
            IInteractable interactable = hitCollider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        // interact
        if (closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }
}
