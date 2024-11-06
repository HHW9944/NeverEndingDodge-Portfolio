using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private GameObject player;
    public GameObject missilePrefab;
    public float initialDelay = 2.0f; // 첫 발사까지의 대기 시간
    public float fireRate = 1.5f;     // 미사일 발사 주기
    public float rotationSpeed = 5.0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

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
            missileRotation *= Quaternion.Euler(90, 0, 0); // 90도 회전

            // 미사일의 회전을 설정
            missile.transform.rotation = missileRotation;
        }
    }
}
