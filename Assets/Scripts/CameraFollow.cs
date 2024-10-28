using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // 따라다닐 플레이어의 Transform
    public Vector3 offset;          // 플레이어와 카메라 사이의 거리 (오프셋)
    public float smoothSpeed = 0.125f; // 카메라 이동의 부드러움 정도

    private void LateUpdate()
    {
        // 목표 위치 설정 (플레이어 위치 + 오프셋)
        Vector3 desiredPosition = player.position + offset;

        // 부드러운 이동을 위해 Lerp 함수 사용
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치를 부드러운 위치로 업데이트
        transform.position = smoothedPosition;

        // 카메라가 항상 플레이어를 바라보도록 설정 (옵션)
        transform.LookAt(player);
    }
}

