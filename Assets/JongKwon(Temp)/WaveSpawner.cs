using System.Collections;
using MoreMountains.Feedbacks;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public float wave01StartTime = 5;
    public float wave02StartTime = 5;
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
        yield return new WaitForSeconds(wave01StartTime);
        Debug.Log($"First action after {wave01StartTime} seconds!");
        wave01.enabled = true;

        yield return new WaitForSeconds(wave02StartTime);
        Debug.Log($"Second action after {wave02StartTime} seconds!");
        //wave02.enabled = true;

        yield return new WaitForSeconds(wave03StartTime);

        yield return new WaitForSeconds(wave04StartTime);

        yield return new WaitForSeconds(wave05StartTime);
    }
}
