using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private GameObject player;
    public GameObject missilePrefab;
    public float initialDelay = 2.0f; // ù �߻������ ��� �ð�
    public float fireRate = 1.5f;     // �̻��� �߻� �ֱ�
    public float rotationSpeed = 5.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        // ������ �ð� ���ĺ��� ������ �������� �̻��� �߻�
        InvokeRepeating(nameof(FireMissile), initialDelay, fireRate);
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            Vector3 direction = player.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            //targetRotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireMissile()
    {
        GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);

        if (player != null)
        {
            Vector3 direction = player.transform.position - missile.transform.position;
            Quaternion missileRotation = Quaternion.LookRotation(direction);
            missileRotation *= Quaternion.Euler(90, 0, 0); // 90�� ȸ��

            // �̻����� ȸ���� ����
            missile.transform.rotation = missileRotation;
        }
    }
}
