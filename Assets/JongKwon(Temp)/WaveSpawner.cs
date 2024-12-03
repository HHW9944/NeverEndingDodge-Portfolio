using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject wavePrefab; // 생성할 웨이브의 프리팹
    public Transform spawnPoint; // 웨이브가 생성되는 위치
    public float waveDuration = 40f; // 각 웨이브의 지속 시간
    public float intervalBetweenWaves = 40f; // 웨이브 간 간격
    public int totalGameTime = 120; // 총 게임 시간

    public float elapsedTime = 0f; // 경과 시간 (고정)

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (elapsedTime < totalGameTime)
        {
            // 웨이브 생성
            SpawnWave();

            // 다음 웨이브까지 대기
            yield return new WaitForSeconds(intervalBetweenWaves);

            // 경과 시간 업데이트
            elapsedTime += intervalBetweenWaves;
        }

        Debug.Log("Game Over! All waves spawned.");
    }

    void SpawnWave()
    {
        if (wavePrefab != null && spawnPoint != null)
        {
            GameObject wave = Instantiate(wavePrefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(wave, waveDuration); // 웨이브 지속 시간 후 제거
            Debug.Log($"Wave spawned at {elapsedTime} seconds.");
        }
        else
        {
            Debug.LogWarning("WavePrefab or SpawnPoint is not assigned.");
        }
    }
}
