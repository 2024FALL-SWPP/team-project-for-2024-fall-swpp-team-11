using UnityEngine;
using System.Collections;

public class MoneyTree2D : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject coinPrefab; // 생성할 코인 프리팹
    [SerializeField] private int coinAmount = 1; // 생성할 코인의 수
    [SerializeField] private float interactCooldown = 0.05f;

    private bool canInteract = true;

    public void Interact()
    {
        if (canInteract)
        {
            SpawnCoins();
            StartCoroutine(InteractCooldown());
        }
    }

    public void OnFocus()
    {
        // 나무에 포커스되었을 때의 효과를 추가할 수 있습니다.
    }

    public void OnDefocus()
    {
        // 나무에서 포커스가 해제되었을 때의 효과를 추가할 수 있습니다.
    }

    private void SpawnCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            // 무작위 위치 계산
            Vector2 randomPosition = new Vector2(
                Random.Range(transform.position.x - 1f, transform.position.x + 1f),
                Random.Range(transform.position.y - 2f, transform.position.y - 1f)
            );

            // 코인 생성
            Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        }
    }

    private IEnumerator InteractCooldown()
    {
        canInteract = false;
        yield return new WaitForSeconds(interactCooldown);
        canInteract = true;
    }
}
