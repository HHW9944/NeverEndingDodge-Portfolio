using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy 하나마다 가지는 컴포넌트
public class EnemyAI : MonoBehaviour
{
    public GameObject player;
    public bool isAlwaysRecognizePlayer;
    public float recognitionDistance;
    public float rotationSpeed = 3.0f;

    void Start()
    {
        if (player == null)
            Debug.LogError("no player");
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (isAlwaysRecognizePlayer || distanceToPlayer <= recognitionDistance)
            {
                Vector3 direction = player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // Y축에 대해 180도 회전 추가 <- 만약에 적이 플레이어의 반대쪽을 본다면 이 부분을 삭제해주세요
                targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
