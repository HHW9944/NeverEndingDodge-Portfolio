using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float wave01StartTime = 5;
    public float wave02StartTime = 3;
    public float wave03StartTime = 3;
    public float wave04StartTime = 3;
    public float wave05StartTime = 3;

    // 적, 미사일, 소행성 프리팹
    public GameObject[] enemyPrefabs;
    public GameObject[] missilePrefabs;
    public GameObject[] obastaclePrefabs;

    void Start()
    {
        // 코루틴 시작
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(wave01StartTime);
        Debug.Log($"First action after {wave01StartTime} seconds!");

        yield return new WaitForSeconds(wave02StartTime);
        Debug.Log($"Second action after {wave02StartTime} seconds!");

        yield return new WaitForSeconds(wave03StartTime);

        yield return new WaitForSeconds(wave04StartTime);

        yield return new WaitForSeconds(wave05StartTime);
    }
}
