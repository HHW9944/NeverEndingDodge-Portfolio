using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float minSpeed = 15.0f;         // 기본 이동 속도
    public float maxSpeed = 60.0f;         // 가속 시 최대 속도
    private float currentSpeed;            // 현재 이동 속도
    private Vector3 direction;             // 이동 방향
    private float accelerationTimer;       // 가속 타이머
    private float trackingTimer;           // 플레이어 추적 타이머
    private float trackingDuration = 1.0f; // 플레이어를 향해 이동하는 시간
    private float trackingDurationTimer;    // 플레이어 향해 이동 타이머
    private bool isTrackingPlayer = false; // 플레이어 향해 이동 중인지 여부
    private Transform playerTransform;     // Player 위치
    private float rotationSpeed;           // 개별 회전 속도

    void Start()
    {
        // 방향 설정 및 개별 회전 속도 부여
        direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        currentSpeed = minSpeed;
        rotationSpeed = Random.Range(15f, 40f);  // 각 운석에 다른 회전 속도 부여
        accelerationTimer = Random.Range(1f, 3f); // 가속 간격을 짧게 조정
        trackingTimer = Random.Range(3f, 7f);     // 3~7초마다 플레이어 방향 조정

        // Player 오브젝트 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // 회전하면서 이동
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Translate(direction * currentSpeed * Time.deltaTime);

        // 가속 타이머
        accelerationTimer -= Time.deltaTime;
        if (accelerationTimer <= 0)
        {
            currentSpeed = (currentSpeed == minSpeed) ? maxSpeed : minSpeed;
            accelerationTimer = Random.Range(1f, 3f); // 타이머 초기화
        }

        // 플레이어를 잠깐 향해 이동하는 동작
        if (isTrackingPlayer)
        {
            trackingDurationTimer -= Time.deltaTime;
            if (trackingDurationTimer <= 0)
            {
                isTrackingPlayer = false; // 원래의 방향으로 돌아가기
            }
        }
        else
        {
            // 플레이어를 향해 방향 전환할 타이머 감소
            trackingTimer -= Time.deltaTime;
            if (trackingTimer <= 0 && playerTransform != null)
            {
                Vector3 toPlayer = (playerTransform.position - transform.position).normalized;
                direction = Vector3.Lerp(direction, toPlayer, 0.7f).normalized; // 플레이어 방향으로 조정
                trackingDurationTimer = trackingDuration; // 플레이어 향해 이동하는 시간 설정
                isTrackingPlayer = true;
                trackingTimer = Random.Range(3f, 7f); // 타이머 초기화
            }
        }

        // 맵 경계를 벗어나면 방향 전환
        if (Mathf.Abs(transform.position.x) > 250 || Mathf.Abs(transform.position.y) > 250 || Mathf.Abs(transform.position.z) > 250)
        {
            direction = -direction; // 반대 방향으로 전환
        }
    }
}
