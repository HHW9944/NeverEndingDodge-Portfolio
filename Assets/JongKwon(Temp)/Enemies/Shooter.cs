using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject player;
    public GameObject missilePrefab;
    public float initialDelay = 2.0f; // ù �߻������ ��� �ð�
    public float fireRate = 1.5f;     // �̻��� �߻� �ֱ�
    public float rotationSpeed = 5.0f;

    void Start()
    {
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

            // targetRotation *= Quaternion.Euler(0, 180, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void FireMissile()
    {
        Instantiate(missilePrefab, transform.position, transform.rotation);
    }
}
