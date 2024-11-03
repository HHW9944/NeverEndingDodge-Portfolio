using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyMove : MonoBehaviour
{
    public Transform Target;
    public float MoveSpeed = 5f;

    private Vector3 _moveDirection;

    void Start()
    {
        // Player의 방향을 계산
        if (Target != null)
        {
            _moveDirection = (Target.position - transform.position).normalized;

            transform.rotation = Quaternion.LookRotation(_moveDirection);
        }
    }

    void Update()
    {
        transform.position += _moveDirection * MoveSpeed * Time.deltaTime;
    }
}