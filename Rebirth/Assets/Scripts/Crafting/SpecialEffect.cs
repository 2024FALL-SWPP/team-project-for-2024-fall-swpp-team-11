using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    [Header("Success Effect Settings")]
    [SerializeField] private GameObject successEffectPrefab; // success effect
    [SerializeField] private float successEffectScale = 1f; // scale
    [SerializeField] private float successEffectDuration = 1f; // duration

    [Header("Failure Effect Settings")]
    [SerializeField] private GameObject failureEffectPrefab; // fail effect
    [SerializeField] private float failureEffectScale = 1f; // Scale 
    [SerializeField] private float failureEffectDuration = 1f; // Duration 

    public void TriggerSuccessEffect(Vector3 position)
    {
        TriggerEffect(successEffectPrefab, position, successEffectScale, successEffectDuration);
    }

    public void TriggerFailureEffect(Vector3 position)
    {
        TriggerEffect(failureEffectPrefab, position, failureEffectScale, failureEffectDuration);
    }

    private void TriggerEffect(GameObject effectPrefab, Vector3 position, float scale, float duration)
    {
   
        GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);
        
        effect.transform.localScale = Vector3.one * scale;

        Destroy(effect, duration);
    }

    public void SetSuccessEffectScale(float newScale)
    {
        successEffectScale = newScale;
    }

    public void SetSuccessEffectDuration(float newDuration)
    {
        successEffectDuration = newDuration;
    }

    public void SetFailureEffectScale(float newScale)
    {
        failureEffectScale = newScale;
    }

    public void SetFailureEffectDuration(float newDuration)
    {
        failureEffectDuration = newDuration;
    }
}
