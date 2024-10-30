using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy �ϳ����� ������ ������Ʈ
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
            // �÷��̾���� �Ÿ� ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // �÷��̾ �ٶ� (Slerp ����ؼ� �ε巴�� ȸ��)
            if (isAlwaysRecognizePlayer || distanceToPlayer <= recognitionDistance)
            {
                Vector3 direction = player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                // !!! Player ������Ʈ�� �ݴ� �������� �ٶ󺻴ٸ� ���⸦ �������ּ��� !!!
                targetRotation *= Quaternion.Euler(0, 180, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}