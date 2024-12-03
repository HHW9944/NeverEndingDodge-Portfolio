using System.Collections;
using UnityEngine;

public class Wave01 : MonoBehaviour
{
    public GameObject normalMissile; // 미사일 프리팹
    public GameObject player;       // 플레이어 객체
    public float spawnInterval = 0.2f; // 생성 간격 (초)
    public float spawnDuration = 30.0f; // 생성 지속 시간 (초)
    public float spawnRangeY = 8.0f; // Y축 랜덤 범위
    public float spawnRangeZ = 8.0f; // Z축 랜덤 범위
    private float spawnTimer = 0.0f;

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
            // 플레이어 주변 랜덤 위치 계산 (x=50 고정)
            Vector3 spawnPosition = new Vector3(
                50, 
                player.transform.position.y + Random.Range(-spawnRangeY, spawnRangeY),
                player.transform.position.z + Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            // +y를 -x로 향하도록 회전값 설정
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);

            // 미사일 생성
            Instantiate(normalMissile, spawnPosition, spawnRotation);

            // 시간 갱신
            elapsedTime += spawnInterval;

            // 다음 생성까지 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
