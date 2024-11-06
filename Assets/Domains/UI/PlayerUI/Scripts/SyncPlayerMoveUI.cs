using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SyncPlayerMoveUI : MonoBehaviour
{
    public PlayerMove playerMove;             // PlayerMove 컴포넌트를 참조
    public float uiMoveMultiplier = 0.5f;     // UI 이동 거리 가중치
    public float speedWeight = 1.0f;          // 속도에 따른 가중치
    public float smoothTime = 0.3f;           // UI가 목표 위치로 이동하는 속도 (느긋함 조절)

    private RectTransform _uiRectTransform;
    private Vector2 _originalPosition;
    private Vector2 _currentVelocity;         // SmoothDamp에서 사용할 현재 속도

    void Start()
    {
        // UI의 RectTransform을 가져옵니다.
        _uiRectTransform = GetComponent<RectTransform>();
        _originalPosition = _uiRectTransform.anchoredPosition; // 초기 위치 저장

        // PlayerMove 컴포넌트가 할당되지 않았다면 찾아서 할당합니다.
        if (playerMove == null)
        {
            playerMove = FindObjectOfType<PlayerMove>();
            if (playerMove == null)
            {
                Debug.LogError("PlayerMove component not found in the scene.");
            }
        }
    }

    void Update()
    {
        if (playerMove == null || _uiRectTransform == null)
            return;

        // 목표 위치 계산
        Vector2 targetPosition;
        
        if (playerMove.IsMoving)
        {
            // 플레이어의 이동 방향과 속도를 가져옵니다.
            Vector2 moveDirection = playerMove.GetMovementDirection();
            float speed = playerMove.Speed;

            // UI 이동 방향과 거리 계산
            Vector2 uiMoveDirection = new Vector2(moveDirection.x, moveDirection.y) * uiMoveMultiplier;
            Vector2 weightedMovement = -uiMoveDirection * (1 + speed * speedWeight);

            // 목표 위치를 원래 위치에서 offset된 위치로 설정
            targetPosition = _originalPosition + weightedMovement;
        }
        else
        {
            // 움직임이 없을 때는 원래 위치를 목표 위치로 설정
            targetPosition = _originalPosition;
        }

        // SmoothDamp를 사용해 부드럽게 목표 위치로 이동
        _uiRectTransform.anchoredPosition = Vector2.SmoothDamp(
            _uiRectTransform.anchoredPosition,
            targetPosition,
            ref _currentVelocity,
            smoothTime
        );
    }
}
