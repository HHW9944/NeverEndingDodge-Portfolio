using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject player;
    public GameObject missilePrefab;
    public float initialDelay = 2.0f; // 첫 발사까지의 대기 시간
    public float fireRate = 1.5f;     // 미사일 발사 주기
    public float rotationSpeed = 5.0f;

    void Start()
    {
        // 지정된 시간 이후부터 일정한 간격으로 미사일 발사
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
