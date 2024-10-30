using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    GameObject missilePrefab;

    [SerializeField]
    float initialDelay = 2.0f; // 첫 발사까지의 대기 시간
    [SerializeField]
    float fireRate = 1.5f;     // 미사일 발사 주기

    void Start()
    {
        // 지정된 시간 이후부터 일정한 간격으로 미사일 발사
        InvokeRepeating(nameof(FireMissile), initialDelay, fireRate);
    }

    void FireMissile()
    {
        Instantiate(missilePrefab, transform.position, transform.rotation);
    }
}
