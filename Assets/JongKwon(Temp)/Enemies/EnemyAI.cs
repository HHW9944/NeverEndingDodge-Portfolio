using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private GameObject player;
    public bool isAlwaysRecognizePlayer;
    public float recognitionDistance;
    public float rotationSpeed = 5.0f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 플레이어 방향을 계산하여 적이 생성될 때 플레이어를 바로 바라보도록 설정
            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
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

                //targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}