using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingMissileMovement : MonoBehaviour
{
    private GameObject player;
    public float speed = 10.0f;          // 미사일 이동 속도
    public float increasedSpeed = 15.0f;
    public float rotationSpeed = 200.0f; // 회전 속도
    public float straightDistance = 15.0f;   // 추적을 중지하는 거리 (플레이어와의)

    void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾아서 할당합니다.
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어 방향 계산
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize(); // 방향 벡터 정규화

            // 미사일의 머리 방향이 플레이어를 향하도록 회전
            // Quaternion.LookRotation은 Z축이 플레이어를 향하도록 만들므로,
            // 미사일의 'up' 방향을 플레이어 방향으로 맞추기 위해 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // 부드럽게 회전시키기 위해 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * Quaternion.Euler(90, 0, 0), rotationSpeed * Time.deltaTime);

            // 미사일을 플레이어 방향으로 이동
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, player.transform.position) <= straightDistance)
            {
                player = null;
            }
        }
        else
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }
}