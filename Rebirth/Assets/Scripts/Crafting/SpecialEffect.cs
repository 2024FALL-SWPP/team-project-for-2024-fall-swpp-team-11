using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    [Header("Success Effect Settings")]
    [SerializeField] private GameObject successEffectPrefab; // 성공 이펙트
    [SerializeField] private float successEffectScale = 1f; // 스케일
    [SerializeField] private float successEffectDuration = 1f; // 지속 시간
    [SerializeField] private AudioClip successSound; // 성공 시 재생할 사운드

    [Header("Failure Effect Settings")]
    [SerializeField] private GameObject failureEffectPrefab; // 실패 이펙트
    [SerializeField] private float failureEffectScale = 1f; // 스케일
    [SerializeField] private float failureEffectDuration = 1f; // 지속 시간
    [SerializeField] private AudioClip failureSound; // 실패 시 재생할 사운드

    private AudioSource audioSource;

    private void Awake()
    {
        // AudioSource 컴포넌트 가져오기 또는 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // AudioSource 설정 (옵션: 재생 시 다른 이펙트를 방해하지 않도록 설정)
        audioSource.playOnAwake = false;
    }

    public void TriggerSuccessEffect(Vector3 position)
    {
        TriggerEffect(successEffectPrefab, position, successEffectScale, successEffectDuration);
        PlaySound(successSound);
    }

    public void TriggerFailureEffect(Vector3 position)
    {
        TriggerEffect(failureEffectPrefab, position, failureEffectScale, failureEffectDuration);
        PlaySound(failureSound);
    }

    private void TriggerEffect(GameObject effectPrefab, Vector3 position, float scale, float duration)
    {
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, position, Quaternion.identity);
            effect.transform.localScale = Vector3.one * scale;
            Destroy(effect, duration);
        }
        else
        {
            Debug.LogWarning("Effect prefab is not assigned.");
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip or AudioSource is not assigned.");
        }
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
