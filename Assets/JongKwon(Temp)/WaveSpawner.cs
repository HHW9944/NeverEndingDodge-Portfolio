using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float wave01StartTime = 5;
    public float wave02StartTime = 10;

    void Start()
    {
        // 코루틴 시작
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // x초 대기
        yield return new WaitForSeconds(wave01StartTime);
        Debug.Log($"First action after {wave01StartTime} seconds!");

        // y초 대기
        yield return new WaitForSeconds(wave02StartTime);
        Debug.Log($"Second action after {wave02StartTime} seconds!");
    }
}
