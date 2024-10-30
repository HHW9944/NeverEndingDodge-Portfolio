using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    GameObject missilePrefab;

    [SerializeField]
    float initialDelay = 2.0f; // ù �߻������ ��� �ð�
    [SerializeField]
    float fireRate = 1.5f;     // �̻��� �߻� �ֱ�

    void Start()
    {
        // ������ �ð� ���ĺ��� ������ �������� �̻��� �߻�
        InvokeRepeating(nameof(FireMissile), initialDelay, fireRate);
    }

    void FireMissile()
    {
        Instantiate(missilePrefab, transform.position, transform.rotation);
    }
}
