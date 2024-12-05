using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public RectTransform Background;    // 조이스틱 배경
    public RectTransform Handle;        // 조이스틱 핸들
    
    [SerializeField]
    private PlayerMove playerMove;      // PlayerMove 참조 추가
    
    private float _maxDistance;
    
    void Start()
    {
        _maxDistance = Background.sizeDelta.x / 2f;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateHandlePosition(eventData);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        UpdateHandlePosition(eventData);
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        UpdateHandlePosition(eventData);
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        Handle.anchoredPosition = Vector2.zero;
        playerMove.OnMove(Vector2.zero);
    }
    
    private void UpdateHandlePosition(PointerEventData eventData)
    {
        Vector2 localPoint;
        // 스크린 좌표를 Background(RectTransform)의 로컬 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Background, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localPoint))
        {
            // 로컬 좌표 기준 방향(벡터) 계산 및 정규화
            Vector2 direction = localPoint;
            Vector2 normalizedInput = Vector2.ClampMagnitude(direction / _maxDistance, 1f);
            
            // Handle을 로컬 좌표 기준으로 이동
            Handle.anchoredPosition = normalizedInput * _maxDistance;
            
            // 플레이어 이동에 반영
            playerMove.OnMove(normalizedInput);
        }
    }
}
