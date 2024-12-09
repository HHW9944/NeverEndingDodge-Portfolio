using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public bool isAlwaysRecognizePlayer;
    public float recognitionDistance;
    public float rotationSpeed = 5.0f;
    public float moveSpeed = 50.0f; // 이동 속도
    public float targetX = 100.0f; // 목표 깊이
    public GameObject leftShootPoint;
    public GameObject rightShootPoint;
    public GameObject ironMissile;
    public GameObject bomb;

    private Vector3 targetPosition; // 목표 위치
    private bool hasReachedTarget = false; // 목표 위치에 도달했는지 여부
    private bool lookAtPlayer = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && lookAtPlayer)
        {
            // 플레이어 방향을 계산하여 적이 생성될 때 플레이어를 바로 바라보도록 설정
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }

        float spawnedX = transform.position.x;
        float spawnedY = transform.position.y;
        float spawnedZ = transform.position.z;
        // 시작 시 -x 방향으로 이동
        targetPosition = new Vector3(targetX, spawnedY, spawnedZ);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (isAlwaysRecognizePlayer || distanceToPlayer <= recognitionDistance)
            {
                if (lookAtPlayer)
                {
                    // 플레이어를 바라보도록 회전
                    Vector3 direction = player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
                
                // y, z값을 플레이어와 일치시키기
                // Vector3 newPosition = transform.position;
                // newPosition.y = player.transform.position.y; // y 좌표 일치
                // newPosition.z = player.transform.position.z; // z 좌표 일치
                // transform.position = newPosition; // 새로운 위치로 이동
            }
        }

        if (!hasReachedTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // 목표 위치에 도달하면 이동 멈추기
            if (Mathf.Approximately(transform.position.x, targetPosition.x))
            {
                hasReachedTarget = true;
                // 목표 위치에 도달한 후 추가적인 처리가 필요하면 여기서 할 수 있음
                StartCoroutine(ShootMissilesWithDelay());
            }
        }
    }

    private IEnumerator ShootMissilesWithDelay()
    {
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(2.0f);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(2.0f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(2.0f);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(1.0f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(1.0f);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(1.0f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);

        yield return new WaitForSeconds(2.5f);

        Instantiate(bomb, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        yield return new WaitForSeconds(2.5f);
        Instantiate(bomb, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(1.5f);
        Instantiate(bomb, leftShootPoint.transform.position, leftShootPoint.transform.rotation);

        yield return new WaitForSeconds(1.0f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        Instantiate(ironMissile, leftShootPoint.transform.position, leftShootPoint.transform.rotation);
        Instantiate(ironMissile, rightShootPoint.transform.position, rightShootPoint.transform.rotation);
        

        yield return new WaitForSeconds(3.0f);

        // 돌진
        lookAtPlayer = false;
        Vector3 dashTargetPosition = player.transform.position; // 돌진 시작 시의 플레이어 위치 저장
        Quaternion dashTargetRotation = Quaternion.LookRotation(dashTargetPosition - transform.position); // 목표 위치를 향한 방향 계산
        transform.rotation = dashTargetRotation; // 목표 방향으로 회전

        float dashSpeed = 60.0f; // 돌진 속도

        // 플레이어의 초기 위치를 저장하여 해당 위치로 계속 돌진
        Vector3 dashDirection = (dashTargetPosition - transform.position).normalized; // 돌진 시작 시의 방향

        while (true) // 지속적으로 돌진
        {
            // 저장된 방향으로 계속 이동
            transform.position += dashDirection * dashSpeed * Time.deltaTime;
            yield return null;
        }

    }
}