using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy 하나마다 가지는 컴포넌트
public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public bool isAlwaysRecognizePlayer;
    public float recognitionDistance;
    public float rotationSpeed = 2.0f;

    void Start()
    {
        if (player == null)
            Debug.LogError("no player");
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // 플레이어를 바라봄 (Slerp 사용해서 부드럽게 회전)
            if (isAlwaysRecognizePlayer || distanceToPlayer <= recognitionDistance)
            {
                Vector3 direction = player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // !!! Player 오브젝트의 반대 방향으로 바라본다면 여기를 삭제해주세요 !!!
                targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}