using UnityEngine;

public class Dash : Skill
{
    [Header("Dash Settings")]
    [Tooltip("대시 쿨다운 시간")]
    public float Cost = 20f;

    [Tooltip("대시의 힘 또는 속도")]
    public float Force = 20f;

    [Tooltip("대시 지속 시간")]
    public float Duration = 0.2f;

    [Tooltip("대시 재사용 대기 시간")]
    public float Cooldown = 1f;

    [Header("References")]
    [SerializeField] private PlayerMove _playerMove;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private float _cooldownTimer = 0f;

    private void Update()
    {
        // 대시 중일 때 시간 업데이트
        if (_isDashing)
        {
            _dashTimer += Time.deltaTime;
            if (_dashTimer >= Duration)
            {
                EndDash();
            }
        }

        // 쿨다운 시간 업데이트
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    public override void UseSkill()
    {
        // 쿨다운 중이거나 이미 대시 중이면 스킬 사용 불가
        if (_cooldownTimer > 0f || _isDashing)
        {
            return;
        }

        StartDash();
    }

    private void StartDash()
    {
        Vector3 movementDirection = _playerMove.GetMovementDirection();

        if (movementDirection == Vector3.zero)
        {
            // 움직임 입력이 없으면 현재 바라보는 방향으로 대시
            movementDirection = transform.forward;
        }

        // 대시 시작
        _isDashing = true;
        _dashTimer = 0f;
        _cooldownTimer = Cooldown;
        _playerMove.AddForce(movementDirection.normalized * Force, ForceMode.Impulse);
    }

    private void EndDash()
    {
        _isDashing = false;
    }

    public override float GetCost()
    {
        return 0;
    }
}
