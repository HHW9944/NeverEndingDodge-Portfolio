using System.Collections;
using UnityEngine;

public class Wave01 : MonoBehaviour
{
    public GameObject normalMissile; // 일반 미사일 프리팹
    public GameObject hugeMissile;  // 거대 미사일 프리팹
    public GameObject player;       // 플레이어 객체
    public float spawnInterval = 0.04f; // 생성 간격 (초)
    public float spawnDuration = 25.0f; // 생성 지속 시간 (초)
    public float spawnRangeY = 24.0f; // Y축 랜덤 범위
    public float spawnRangeZ = 24.0f; // Z축 랜덤 범위
    private float spawnTimer = 0.0f;
    private bool[] hugeMissileSpawned = new bool[4]; // 거대 미사일 생성 상태

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
            return;
        }

        // 코루틴 시작
        StartCoroutine(SpawnMissiles());
    }

    IEnumerator SpawnMissiles()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < spawnDuration)
        {
            // 플레이어 주변 랜덤 위치 계산 (x=100 고정)
            Vector3 spawnPosition = new Vector3(
                100, 
                player.transform.position.y + Random.Range(-spawnRangeY, spawnRangeY),
                player.transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            // +y를 -x로 향하도록 회전값 설정
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);

            // 일반 미사일 생성
            Instantiate(normalMissile, spawnPosition, spawnRotation);

            // 거대 미사일 생성 로직
            if (!hugeMissileSpawned[0] && elapsedTime >= 5.0f)
            {
                SpawnHugeMissile();
                hugeMissileSpawned[0] = true;
            }
            else if (!hugeMissileSpawned[1] && elapsedTime >= 10.0f)
            {
                SpawnHugeMissile();
                hugeMissileSpawned[1] = true;
            }
            else if (!hugeMissileSpawned[2] && elapsedTime >= 15.0f)
            {
                SpawnHugeMissile();
                hugeMissileSpawned[2] = true;
            }
            else if (!hugeMissileSpawned[3] && elapsedTime >= 20.0f)
            {
                SpawnHugeMissile();
                hugeMissileSpawned[3] = true;
            }

            // 시간 갱신
            elapsedTime += spawnInterval;

            // 다음 생성까지 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnHugeMissile()
    {
        Vector3 hugeMissilePosition = player.transform.position + new Vector3(100, 0, 0); // 플레이어 정면
        Instantiate(hugeMissile, hugeMissilePosition, Quaternion.Euler(0, 0, 90));
    }
}
