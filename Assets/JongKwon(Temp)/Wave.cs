using System.Collections;
using UnityEngine;

public class Wave : MonoBehaviour
{
    float spawnDepth = 100.0f;
    GameObject normalMissile; // 일반 미사일 프리팹
    GameObject hugeMissile;   // 거대 미사일 프리팹
    GameObject redObstacle;
    GameObject blueObstacle;
    GameObject enemyA;
    GameObject enemyB;
    GameObject enemyC;
    public GameObject player; // 플레이어 객체

    public float duration = 120.0f; // 총 실행 시간
    private float startTime;        // 시작 시간

    // 소환 정보 (소환 시간, 위치, 프리팹)
    public struct SpawnInfo
    {
        public float time;         // 소환 시간
        public Vector3 position;   // 소환 위치
        public Quaternion rotation; // 소환 회전
        public Vector3 scale; // 소환 크기
        public GameObject prefab;  // 소환할 프리팹

        // 기본값을 설정하는 생성자
        public SpawnInfo(float time, Vector3 position, GameObject prefab, Quaternion? rotation = null, Vector3? scale = null)
        {
            this.time = time;
            this.position = position;
            this.prefab = prefab;
            this.rotation = rotation ?? Quaternion.identity; // 기본값: Quaternion.identity
            this.scale = scale ?? Vector3.one; // 기본값: Vector3.one
        }
    }

    // 여러 스케줄을 나눠서 관리
    public SpawnInfo[][] spawnSchedules;
    private int currentScheduleIndex = 0;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
        }

        enemyA = gameObject.GetComponent<WaveSpawner>().enemyPrefabs[0];
        enemyB = gameObject.GetComponent<WaveSpawner>().enemyPrefabs[1];
        enemyC = gameObject.GetComponent<WaveSpawner>().enemyPrefabs[2];
        redObstacle = gameObject.GetComponent<WaveSpawner>().obastaclePrefabs[0];
        blueObstacle = gameObject.GetComponent<WaveSpawner>().obastaclePrefabs[1];
        normalMissile = gameObject.GetComponent<WaveSpawner>().missilePrefabs[0];
        hugeMissile = gameObject.GetComponent<WaveSpawner>().missilePrefabs[1];

        // 각 스케줄 설정
        spawnSchedules = new SpawnInfo[][]
        {
            // Schedule 1
            new SpawnInfo[]
            {
                // new SpawnInfo { time = 5.0f, position = new Vector3(spawnDepth, player.transform.position.y+5, player.transform.position.z+3), prefab = blueObstacle },
                // new SpawnInfo { time = 6.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = redObstacle },
                // new SpawnInfo { time = 6.8f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = redObstacle },
                // new SpawnInfo { time = 6.9f, position = new Vector3(spawnDepth, player.transform.position.y+6, player.transform.position.z-5), prefab = blueObstacle },
                // new SpawnInfo { time = 8.2f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z+1), prefab = redObstacle },
                // new SpawnInfo { time = 8.6f, position = new Vector3(spawnDepth, player.transform.position.y+5, player.transform.position.z+4), prefab = blueObstacle },
                // new SpawnInfo { time = 9.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
                // new SpawnInfo { time = 9.5f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 9.7f, position = new Vector3(spawnDepth, player.transform.position.y-4, player.transform.position.z-5), prefab = redObstacle },
                // new SpawnInfo { time = 10.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+1), prefab = blueObstacle },
                // new SpawnInfo { time = 10.8f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = redObstacle },
                // new SpawnInfo { time = 12.0f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 14.5f, position = new Vector3(spawnDepth, player.transform.position.y-4, player.transform.position.z-3), prefab = blueObstacle },
                // new SpawnInfo { time = 15.5f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z), prefab = redObstacle },
                // new SpawnInfo { time = 16.2f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+2), prefab = blueObstacle },
                // new SpawnInfo { time = 18.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z-2), prefab = redObstacle },
                // new SpawnInfo { time = 18.9f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z+1), prefab = redObstacle },
                // new SpawnInfo { time = 20.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z-1), prefab = blueObstacle },
                // new SpawnInfo { time = 21.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z+1), prefab = blueObstacle },
                // new SpawnInfo { time = 22.0f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z), prefab = redObstacle },
                // new SpawnInfo { time = 24.0f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z+3), prefab = blueObstacle },
                // new SpawnInfo { time = 24.3f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z-4), prefab = redObstacle },
                // new SpawnInfo { time = 25.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z+1), prefab = blueObstacle },
                // new SpawnInfo { time = 26.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+1), prefab = redObstacle },
                // new SpawnInfo { time = 27.0f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z-3), prefab = redObstacle },
                // new SpawnInfo { time = 27.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = blueObstacle },
                // new SpawnInfo { time = 28.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
                // new SpawnInfo { time = 28.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 29.5f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z+2), prefab = redObstacle },
                // new SpawnInfo { time = 30.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
                // new SpawnInfo { time = 30.3f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 30.9f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = blueObstacle },
                // new SpawnInfo { time = 31.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+2), prefab = redObstacle },
                // new SpawnInfo { time = 31.5f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 32.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+4), prefab = blueObstacle },
                // new SpawnInfo { time = 32.3f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = blueObstacle },
                // new SpawnInfo { time = 32.7f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z-1), prefab = redObstacle },
                // new SpawnInfo { time = 33.0f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = redObstacle },
                // new SpawnInfo { time = 33.6f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z+3), prefab = blueObstacle },
                // new SpawnInfo { time = 33.8f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z-3), prefab = redObstacle },
                // new SpawnInfo { time = 34.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+2), prefab = blueObstacle },
                // new SpawnInfo { time = 34.6f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z), prefab = blueObstacle },
                // new SpawnInfo { time = 34.9f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z-2), prefab = redObstacle },
                // 추가 소환 정보...
            },
            // Schedule 2
            new SpawnInfo[]
            {
                // enemyA 생성
                new SpawnInfo { 
                    time = 5.0f, position = new Vector3(spawnDepth + 200.0f, player.transform.position.y, player.transform.position.z+15), prefab = enemyA 
                },
                new SpawnInfo { time = 5.1f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z-3), prefab = redObstacle },
                new SpawnInfo { time = 5.8f, position = new Vector3(spawnDepth, player.transform.position.y+6, player.transform.position.z+7), prefab = blueObstacle },
                new SpawnInfo { time = 6.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
                new SpawnInfo { time = 6.6f, position = new Vector3(spawnDepth, player.transform.position.y-2, player.transform.position.z+5), prefab = redObstacle },
                new SpawnInfo { time = 6.9f, position = new Vector3(spawnDepth, player.transform.position.y+8, player.transform.position.z-6), prefab = blueObstacle },
                
                new SpawnInfo { time = 10.2f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = redObstacle },
                new SpawnInfo { time = 12.0f, position = new Vector3(spawnDepth, player.transform.position.y-8, player.transform.position.z-3), prefab = redObstacle },
                new SpawnInfo { time = 12.0f, position = new Vector3(spawnDepth, player.transform.position.y+9, player.transform.position.z-5), prefab = blueObstacle },
                // EnemyB 생성
                // new SpawnInfo { 
                //     time = 10.0f, position = new Vector3(spawnDepth + 200.0f, player.transform.position.y+25, player.transform.position.z), prefab = enemyB 
                // },
            },
            // Schedule 3
            new SpawnInfo[]
            {
                // new SpawnInfo { time = 2.0f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z+4), prefab = blueObstacle },
                // new SpawnInfo { time = 6.0f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z+1), prefab = redObstacle },
                // 추가 소환 정보...
            }
        };
    }

    void OnEnable()
    {
        StartCoroutine(SpawnAllSchedules());
    }

    IEnumerator SpawnAllSchedules()
    {
        for (currentScheduleIndex = 0; currentScheduleIndex < spawnSchedules.Length; currentScheduleIndex++)
        {
            yield return StartCoroutine(SpawnWithSchedule(spawnSchedules[currentScheduleIndex]));
        }
    }

    IEnumerator SpawnWithSchedule(SpawnInfo[] schedule)
    {
        float scheduleStartTime = Time.time;
        int scheduleIndex = 0;

        while (scheduleIndex < schedule.Length)
        {
            float elapsedTime = Time.time - scheduleStartTime;

            // 현재 스케줄의 소환 시간이 되었는지 확인
            while (scheduleIndex < schedule.Length && schedule[scheduleIndex].time <= elapsedTime)
            {
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);

                // 스케줄에 따라 프리팹 소환
                Instantiate(
                    schedule[scheduleIndex].prefab,
                    schedule[scheduleIndex].position,
                    spawnRotation
                );

                scheduleIndex++;
            }

            // 다음 프레임까지 대기
            yield return null;
        }

        // 스케줄 종료 후 0초 대기
        yield return new WaitForSeconds(0.0f);
    }
}
