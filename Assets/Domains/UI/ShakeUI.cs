using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeUI : MonoBehaviour
{
    public float shakeAmount = 5f;        // 흔들리는 정도
    public float shakeSpeed = 5f;         // 흔들리는 속도
    public float scaleAmount = 0.1f;      // 확대/축소 정도 (0.1 = 10%)
    public float scaleSpeed = 3f;         // 확대/축소 속도

    private Vector3 originalPosition;      // 원래 위치
    private Vector3 originalScale;         // 원래 크기
    private float seedX;                  // X축 노이즈 시드
    private float seedY;                  // Y축 노이즈 시드
    
    void Start()
    {
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        originalScale = transform.localScale;
        
        // 각 UI 객체마다 다른 랜덤한 시작점을 가지도록 설정
        seedX = Random.Range(0f, 1000f);
        seedY = Random.Range(0f, 1000f);
    }

    void Update()
    {
        // Perlin Noise를 사용한 랜덤한 흔들림 효과
        float offsetX = (Mathf.PerlinNoise(seedX + Time.time * shakeSpeed, seedX) * 2f - 1f) * shakeAmount;
        float offsetY = (Mathf.PerlinNoise(seedY, seedY + Time.time * shakeSpeed) * 2f - 1f) * shakeAmount;
        
        // 크기 변화 효과
        float scale = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount + 1f;
        
        // 위치와 크기 적용
        GetComponent<RectTransform>().anchoredPosition = new Vector2(originalPosition.x + offsetX, originalPosition.y + offsetY);
        transform.localScale = new Vector3(originalScale.x * scale, originalScale.y * scale, originalScale.z);
    }
}
