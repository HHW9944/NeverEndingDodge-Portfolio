using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float wave01StartTime = 5;
    public float wave02StartTime = 30;
    public float wave03StartTime = 5;
    public float wave04StartTime = 5;
    public float wave05StartTime = 5;

    // 적, 미사일, 소행성 프리팹
    public GameObject[] enemyPrefabs;
    public GameObject[] missilePrefabs;
    public GameObject[] obastaclePrefabs;

    // wave 프리팹
    // public GameObject wave01;
    // public GameObject wave02;
    // public GameObject wave03;
    // public GameObject wave04;
    // public GameObject wave05;

    public Wave01 wave01;
    public Wave02 wave02;
    // Wave03 wave03;
    // Wave04 wave04;
    // Wave05 wave05;

    void Start()
    {
        wave01 = GetComponent<Wave01>();
        wave02 = GetComponent<Wave02>();
        // wave03.GetComponent<Wave03>();
        // wave04.GetComponent<Wave04>();
        // wave05.GetComponent<Wave05>();

        // 코루틴 시작
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // wave01StartTime에 따라 대기 후 활성화
        yield return new WaitForSeconds(wave01StartTime);
        Debug.Log($"Wave01 enabled at {wave01StartTime} seconds!");
        wave01.enabled = true;

        // wave02StartTime에 따라 대기 후 활성화
        yield return new WaitForSeconds(wave02StartTime - wave01StartTime);
        Debug.Log($"Wave02 enabled at {wave02StartTime} seconds!");
        wave02.enabled = true;

        // wave03StartTime에 따라 대기 후 활성화
        yield return new WaitForSeconds(wave03StartTime - wave02StartTime);
        Debug.Log($"Wave03 enabled at {wave03StartTime} seconds!");

        // wave04StartTime에 따라 대기 후 활성화
        yield return new WaitForSeconds(wave04StartTime - wave03StartTime);
        Debug.Log($"Wave04 enabled at {wave04StartTime} seconds!");

        // wave05StartTime에 따라 대기 후 활성화
        yield return new WaitForSeconds(wave05StartTime - wave04StartTime);
        Debug.Log($"Wave05 enabled at {wave05StartTime} seconds!");
    }
}
