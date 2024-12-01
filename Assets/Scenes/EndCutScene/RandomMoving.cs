using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving : MonoBehaviour
{
    [SerializeField] private float moveRange = 1f;    // 움직임 범위
    [SerializeField] private float moveSpeed = 2f;    // 움직임 속도
    [SerializeField] private float smoothness = 5f;   // 부드러움 정도를 조절하는 계수 추가
    
    private Vector3 startPosition;                     // 시작 위치
    private Vector3 targetPosition;                    // 목표 위치
    private Vector3 nextTargetPosition;               // 다음 목표 위치 추가
    
    void Start()
    {
        startPosition = transform.position;
        SetNewTargetPosition();
        nextTargetPosition = targetPosition;          // 초기 다음 목표 위치 설정
    }
    
    void Update()
    {
        // 현재 위치가 목표 위치에 가까워지면 미리 다음 목표 위치로 전환
        if (Vector3.Distance(transform.position, targetPosition) < 2f)
        {
            targetPosition = nextTargetPosition;
            SetNewNextTargetPosition();
        }
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime * smoothness);
    }
    
    private void SetNewTargetPosition()
    {
        // 시작 위치를 중심으로 moveRange 내에서 랜덤한 위치 설정
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        float randomZ = Random.Range(-moveRange, moveRange);
        
        targetPosition = startPosition + new Vector3(randomX, randomY, randomZ);
    }
    
    private void SetNewNextTargetPosition()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomY = Random.Range(-moveRange, moveRange);
        float randomZ = Random.Range(-moveRange, moveRange);
        
        nextTargetPosition = startPosition + new Vector3(randomX, randomY, randomZ);
    }
}
