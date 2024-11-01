using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs; // 운석 프리팹 배열 (3개 프리팹 설정)
    public int meteorCount = 500;       // 생성할 운석 개수
    public Vector3 spawnAreaSize = new Vector3(500, 500, 500); // 맵 크기에 맞춘 생성 범위

    void Start()
    {
        SpawnMeteors();
    }

    void SpawnMeteors()
    {
        for (int i = 0; i < meteorCount; i++)
        {
            // 생성 위치를 맵 크기 내 랜덤 위치로 설정
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
            );

            // 랜덤한 운석 프리팹 선택
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obj = Instantiate(obstaclePrefabs[randomIndex], randomPosition, Quaternion.identity);
            obj.transform.SetParent(gameObject.transform);
        }
    }
}
