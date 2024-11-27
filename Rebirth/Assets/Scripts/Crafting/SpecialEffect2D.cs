using UnityEngine;

public class SpecialEffect2D : MonoBehaviour
{
    [Header("Success Effect Settings")]
    [SerializeField] private GameObject successEffectPrefab; // Prefab for success effect
    [SerializeField] private Vector3 successEffectScale = Vector3.one; // Scale for success effect
    [SerializeField] private float successEffectDuration = 1f; // Duration of the success effect
    [SerializeField] private float successEffectSpeed = 2f; // Speed of the success effect

    [Header("Failure Effect Settings")]
    [SerializeField] private GameObject failureEffectPrefab; // Prefab for failure effect
    [SerializeField] private Vector3 failureEffectScale = Vector3.one; // Scale for failure effect
    [SerializeField] private float failureEffectDuration = 1f; // Duration of the failure effect
    [SerializeField] private float failureEffectSpeed = 2f; // Speed of the failure effect

    public void TriggerSuccessEffect(Vector3 position)
    {
        TriggerEffect(successEffectPrefab, position, successEffectScale, successEffectDuration, successEffectSpeed);
    }

    public void TriggerFailureEffect(Vector3 position)
    {
        TriggerEffect(failureEffectPrefab, position, failureEffectScale, failureEffectDuration, failureEffectSpeed);
    }

    private void TriggerEffect(GameObject effectPrefab, Vector3 position, Vector3 scale, float duration, float speed)
    {
        if (effectPrefab == null)
        {
            Debug.LogWarning("Effect prefab not assigned.");
            return;
        }

        // Instantiate the effect prefab at the given position
        GameObject effectInstance = Instantiate(effectPrefab, position, Quaternion.identity);
        effectInstance.transform.localScale = scale; // Apply the scale to the effect

        // Add the EffectMover2D component to control the movement of the effect
        EffectMover2D effectMover = effectInstance.AddComponent<EffectMover2D>();
        effectMover.Initialize(speed);

        // Destroy the effect after the given duration
        Destroy(effectInstance, duration);
    }

    // Optional: Setter methods for runtime adjustments
    public void SetSuccessEffectScale(Vector3 newScale)
    {
        successEffectScale = newScale;
    }

    public void SetSuccessEffectDuration(float newDuration)
    {
        successEffectDuration = newDuration;
    }

    public void SetFailureEffectScale(Vector3 newScale)
    {
        failureEffectScale = newScale;
    }

    public void SetFailureEffectDuration(float newDuration)
    {
        failureEffectDuration = newDuration;
    }
}

public class EffectMover2D : MonoBehaviour
{
    private float speed;

    public void Initialize(float effectSpeed)
    {
        speed = effectSpeed;
    }

    private void Update()
    {
        // Move the effect upwards (modify direction as needed)
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
