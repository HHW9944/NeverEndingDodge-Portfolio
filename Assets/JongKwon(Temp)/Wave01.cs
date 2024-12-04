using System.Collections;
using UnityEngine;

public class Wave01 : MonoBehaviour
{
    GameObject normalMissile; // 일반 미사일 프리팹
    GameObject hugeMissile;  // 거대 미사일 프리팹
    GameObject redObstacle;
    GameObject blueObstacle;
    public GameObject player;       // 플레이어 객체
    public float spawnInterval = 5.0f; // 실제 시간 기준으로 5초 간격
    public float spawnDuration = 75.0f; // 생성 지속 시간 (초)
    public float spawnRangeY = 12.0f; // Y축 랜덤 범위
    public float spawnRangeZ = 12.0f; // Z축 랜덤 범위
    private float lastSpawnTime = 0f; // 마지막 생성 시간 기록

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
            return;
        }

        redObstacle = GetComponent<WaveSpawner>().obastaclePrefabs[0];
        blueObstacle = GetComponent<WaveSpawner>().obastaclePrefabs[1];
    }

    void OnEnable()
    {
        // 코루틴 시작
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // 현재 시간(게임의 실제 경과 시간) 가져오기
            float currentTime = Time.time;

            // 마지막 생성 시간 + spawnInterval이 현재 시간보다 작으면 소행성 생성
            if (currentTime - lastSpawnTime >= spawnInterval)
            {
                // 플레이어 주변 랜덤 위치 계산 (x=200 고정)
                Vector3 spawnPosition = new Vector3(
                    200, 
                    player.transform.position.y + Random.Range(-spawnRangeY, spawnRangeY),
                    player.transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ)
                );

                // 회전값 설정 (y축을 따라 회전)
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);

                // 소행성 생성
                Instantiate(redObstacle, spawnPosition, spawnRotation);

                // 마지막 생성 시간 갱신
                lastSpawnTime = currentTime;
            }

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}
