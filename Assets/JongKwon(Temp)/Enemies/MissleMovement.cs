using UnityEngine;

public class MissleMovement : MonoBehaviour
{
    public float speed;
    private GameObject player;
    private Vector3 directionToPlayer;

    void Start()
    {
        // 플레이어를 찾아서 할당
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 플레이어 방향을 계산하여 미사일의 초기 방향 설정
            directionToPlayer = (player.transform.position - transform.position + new Vector3(0, 3.5f, 0)).normalized;

            // 미사일이 플레이어를 향하도록 회전
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 미사일을 설정한 방향으로 계속 이동
            transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
        }
    }
}
