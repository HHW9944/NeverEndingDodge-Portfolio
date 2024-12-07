using System.Collections;
using UnityEngine;

public class Wave01 : MonoBehaviour
{
    float spawnDepth = 100.0f;
    GameObject normalMissile; // 일반 미사일 프리팹
    GameObject hugeMissile;  // 거대 미사일 프리팹
    GameObject redObstacle;
    GameObject blueObstacle;
    public GameObject player;       // 플레이어 객체
    
    public float duration = 30.0f; // 총 실행 시간
    private float startTime; // 시작 시간

    // 소환 정보 (소환 시간, 위치, 프리팹)
    public struct SpawnInfo
    {
        public float time; // 소환 시간
        public Vector3 position; // 소환 위치
        public GameObject prefab; // 소환할 프리팹
    }

    public SpawnInfo[] spawnSchedule; // 소환 스케줄

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

        redObstacle = gameObject.GetComponent<WaveSpawner>().obastaclePrefabs[0];
        blueObstacle = gameObject.GetComponent<WaveSpawner>().obastaclePrefabs[1];

        // 기본 소환 스케줄 설정
        spawnSchedule = new SpawnInfo[]
        {
            new SpawnInfo { time = 0.0f, position = new Vector3(spawnDepth, player.transform.position.y+5, player.transform.position.z+3), prefab = blueObstacle },
            new SpawnInfo { time = 1.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = redObstacle },
            new SpawnInfo { time = 1.8f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 1.9f, position = new Vector3(spawnDepth, player.transform.position.y+6, player.transform.position.z-5), prefab = blueObstacle },
            new SpawnInfo { time = 3.2f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z+1), prefab = redObstacle },
            new SpawnInfo { time = 3.6f, position = new Vector3(spawnDepth, player.transform.position.y+5, player.transform.position.z+4), prefab = blueObstacle },
            new SpawnInfo { time = 4.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
            new SpawnInfo { time = 4.5f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
            new SpawnInfo { time = 4.7f, position = new Vector3(spawnDepth, player.transform.position.y-4, player.transform.position.z-5), prefab = redObstacle },
            new SpawnInfo { time = 5.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+1), prefab = blueObstacle },
            new SpawnInfo { time = 5.8f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 7.0f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z-1), prefab = redObstacle },
            new SpawnInfo { time = 9.5f, position = new Vector3(spawnDepth, player.transform.position.y-4, player.transform.position.z-3), prefab = blueObstacle },
            new SpawnInfo { time = 10.5f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 11.2f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+2), prefab = blueObstacle },
            new SpawnInfo { time = 13.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z-2), prefab = redObstacle },
            new SpawnInfo { time = 13.9f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z+1), prefab = redObstacle },
            new SpawnInfo { time = 15.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z-1), prefab = blueObstacle },
            new SpawnInfo { time = 16.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z+1), prefab = blueObstacle },
            new SpawnInfo { time = 17.0f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 19.0f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z+3), prefab = blueObstacle },
            new SpawnInfo { time = 19.3f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z-4), prefab = redObstacle },

            new SpawnInfo { time = 20.5f, position = new Vector3(spawnDepth, player.transform.position.y+3, player.transform.position.z+1), prefab = blueObstacle },
            new SpawnInfo { time = 21.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+1), prefab = redObstacle },
            new SpawnInfo { time = 22.0f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z-3), prefab = redObstacle },
            new SpawnInfo { time = 22.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = blueObstacle },
            new SpawnInfo { time = 23.1f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
            new SpawnInfo { time = 23.4f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
            new SpawnInfo { time = 24.5f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z+2), prefab = redObstacle },
            new SpawnInfo { time = 25.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = blueObstacle },
            new SpawnInfo { time = 25.1f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z-1), prefab = redObstacle },
            new SpawnInfo { time = 25.5f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = blueObstacle },
            new SpawnInfo { time = 25.8f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+2), prefab = redObstacle },
            new SpawnInfo { time = 26.3f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z-1), prefab = redObstacle },
            new SpawnInfo { time = 27.0f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z+4), prefab = blueObstacle },
            new SpawnInfo { time = 27.3f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = blueObstacle },
            new SpawnInfo { time = 27.7f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 28.0f, position = new Vector3(spawnDepth, player.transform.position.y+2, player.transform.position.z), prefab = redObstacle },
            new SpawnInfo { time = 28.4f, position = new Vector3(spawnDepth, player.transform.position.y+4, player.transform.position.z+3), prefab = blueObstacle },
            new SpawnInfo { time = 29.1f, position = new Vector3(spawnDepth, player.transform.position.y-1, player.transform.position.z+2), prefab = blueObstacle },
            new SpawnInfo { time = 29.6f, position = new Vector3(spawnDepth, player.transform.position.y+1, player.transform.position.z), prefab = blueObstacle },
            new SpawnInfo { time = 29.9f, position = new Vector3(spawnDepth, player.transform.position.y, player.transform.position.z-3), prefab = redObstacle },
        };
    }

    void OnEnable()
    {
        // 시작 시간 기록
        startTime = Time.time;

        // 코루틴 시작
        StartCoroutine(SpawnWithSchedule());
    }

    IEnumerator SpawnWithSchedule()
    {
        int scheduleIndex = 0;

        while (Time.time - startTime < duration)
        {
            // 현재 시간
            float currentTime = Time.time - startTime;

            // 현재 스케줄의 소환 시간이 되었는지 확인
            while (scheduleIndex < spawnSchedule.Length && spawnSchedule[scheduleIndex].time <= currentTime)
            {
                Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);
                // 스케줄에 따라 프리팹 소환
                Instantiate(
                    spawnSchedule[scheduleIndex].prefab,
                    spawnSchedule[scheduleIndex].position,
                    spawnRotation
                );

                // 다음 스케줄로 이동
                scheduleIndex++;
            }

            // 다음 프레임까지 대기
            yield return null;
        }
    }
}
