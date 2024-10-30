using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float speed = 5.0f;        // 이동 속도
    private Vector3 direction;         // 이동 방향

    void Start()
    {
        // 시작할 때 랜덤한 방향을 설정
        direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void Update()
    {
        // 설정된 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime);

        // 맵 경계를 벗어나면 방향 전환 (간단한 방식)
        if (Mathf.Abs(transform.position.x) > 250 || Mathf.Abs(transform.position.y) > 250 || Mathf.Abs(transform.position.z) > 250)
        {
            direction = -direction; // 반대 방향으로 전환
        }
    }
}
