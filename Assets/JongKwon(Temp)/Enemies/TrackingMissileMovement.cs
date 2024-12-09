using UnityEngine;

public class TrackingMissileMovement : MonoBehaviour
{
    private GameObject player;
    public float speed = 50.0f;         // 미사일 이동 속도
    public float rotationSpeed = 200.0f; // 회전 속도
    public float straightDistance = 10.0f; // 추적을 중지하는 거리 (플레이어와의)

    private bool shouldRotate = true; // 회전을 계속할지 여부
    private bool stopTracking = false; // 추적 중지 여부

    void Start()
    {
        // "Player" 태그를 가진 오브젝트를 찾아서 할당합니다.
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (!stopTracking && player != null)
        {
            // 플레이어 방향 계산
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize(); // 방향 벡터 정규화

            // 미사일의 머리 방향이 플레이어를 향하도록 회전
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * Quaternion.Euler(0, 0, 0), rotationSpeed * Time.deltaTime);

            // 미사일을 플레이어 방향으로 이동
            transform.position += transform.forward * speed * Time.deltaTime;

            // 플레이어와의 거리가 일정 거리 이하가 되면 추적 중지
            if (Vector3.Distance(transform.position, player.transform.position) <= straightDistance)
            {
                stopTracking = true;
            }
        }
        else
        {
            // 추적 중지 후 현재 방향으로 직진
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
