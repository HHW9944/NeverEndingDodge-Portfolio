using UnityEngine;
using UnityEngine.Events;

public class Barrier : Skill, ICancellable, ICollisionDamageable
{
    [Header("Barrier Settings")]
    [Tooltip("Barrier Skill이 소모하는 초당 Cost")]
    public float Cost = 10f;

    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _positionOffset = new Vector3(0, 0, 2.0f);
    [SerializeField] private EnergyShield _energyShield;
    [SerializeField] private Cost _playerCost;

    [Header("Events")]
    public UnityEvent onBarrierEnable;  // Barrier 활성화 이벤트
    public UnityEvent onBarrierDisable; // Barrier 비활성화 이벤트
    public UnityEvent onBarrierCollision; // Barrier 충돌 이벤트

    private bool _isBarrierEnabled = false;
    private bool _isTouching = false; // 터치 상태 추적
    private float _holdTime = 0f; // 터치 시간이 경과하는 시간
    public float requiredHoldTime = 1f; // 스킬 발동에 필요한 시간 (1초)

    public override void UseSkill()
    {
        if (_playerCost.Value < Cost)
        {
            return;
        }

        onBarrierEnable?.Invoke(); // Barrier 활성화 이벤트 호출
        _isBarrierEnabled = true;
    }

    public override float GetCost()
    {
        return 0;
    }

    public override float GetCooldownTime()
    {
        return 0;
    }

    public void Cancel()
    {
        onBarrierDisable?.Invoke(); // Barrier 비활성화 이벤트 호출
        _isBarrierEnabled = false;
    }

    public void TakeDamage(IAttackable attacker, float damage)
    {
        // Do nothing
    }

    public void TakeDamage(IAttackable attacker, float damage, Collision other)
    {
        TakeDamage(attacker, damage);

        _energyShield.GetHit(other);
        onBarrierCollision?.Invoke();
    }

    private void Update()
    {
        HandleTouchInput();

        // 위치 업데이트: 플레이어 위치 + 오프셋
        if (_playerTransform != null)
        {
            transform.position = _playerTransform.position + _playerTransform.TransformDirection(_positionOffset);

            // 회전 업데이트: 플레이어가 바라보는 방향
            transform.rotation = Quaternion.LookRotation(_playerTransform.forward);
        }

        if (_isBarrierEnabled)
        {
            _playerCost.UseCost(Cost * Time.deltaTime);

            if (_playerCost.Value <= 0)
            {
                Cancel();
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 첫 번째 터치 가져오기

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 터치가 시작되면 스킬 활성화 준비
                    _isTouching = false; // 초기화
                    _holdTime = 0f;
                    break;

                case TouchPhase.Moved:
                    // 터치가 이동하면 누른 시간이 증가
                    if (_isTouching)
                    {
                        _holdTime += Time.deltaTime;
                        // 일정 시간이 지나면 Barrier 활성화
                        if (_holdTime >= requiredHoldTime)
                        {
                            UseSkill(); // Barrier 발동
                            _isTouching = false; // 발동 후 더 이상 활성화되지 않도록
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    // 터치가 끝나면 Barrier 비활성화
                    Cancel();
                    _isTouching = false;
                    break;
            }
        }
    }
}
