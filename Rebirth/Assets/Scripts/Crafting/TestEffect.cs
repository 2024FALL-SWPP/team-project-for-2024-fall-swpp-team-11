using UnityEngine;

public class TestEffect : MonoBehaviour
{
    [SerializeField] private Vector3 effectScale = Vector3.one; // Scale for the effect
    [SerializeField] private float effectDuration = 1f; // Duration of the effect
    [SerializeField] private float effectSpeed = 2f; // Speed of the effect
    [SerializeField] private string sortingLayerName = "Default"; // Sorting layer for the effect
    [SerializeField] private int orderInLayer = 10; // Order in layer for the effect

    private void Start()
    {
        // Apply scale to the effect
        transform.localScale = effectScale;

        // Set sorting layer and order in layer for Particle System if applicable
        ParticleSystemRenderer particleRenderer = GetComponent<ParticleSystemRenderer>();
        if (particleRenderer != null)
        {
            particleRenderer.sortingLayerName = sortingLayerName;
            particleRenderer.sortingOrder = orderInLayer;
        }

        // Destroy the effect after the specified duration
        Destroy(gameObject, effectDuration);
    }

    private void Update()
    {
        // Move the effect upwards (or any direction as needed)
        transform.Translate(Vector3.up * effectSpeed * Time.deltaTime);
    }
}
