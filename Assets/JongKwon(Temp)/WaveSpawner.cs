using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    //public float waveStartTime = 0;

    // 적, 미사일, 소행성 프리팹
    public GameObject[] enemyPrefabs;
    public GameObject[] missilePrefabs;
    public GameObject[] obastaclePrefabs;

    public Wave wave;

    void Start()
    {
        wave = GetComponent<Wave>();
        wave.enabled = true;

        // 코루틴 시작
        //StartCoroutine(SpawnWave());
    }

    // IEnumerator SpawnWave()
    // {
    //     // waveStartTime에 따라 대기 후 활성화
    //     //yield return new WaitForSeconds(waveStartTime);
    //     wave.enabled = true;
    // }
}
