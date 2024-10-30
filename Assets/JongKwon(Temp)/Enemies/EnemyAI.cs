using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy �ϳ����� ������ ������Ʈ
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

                // Y�࿡ ���� 180�� ȸ�� �߰� <- ���࿡ ���� �÷��̾��� �ݴ����� ���ٸ� �� �κ��� �������ּ���
                targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
