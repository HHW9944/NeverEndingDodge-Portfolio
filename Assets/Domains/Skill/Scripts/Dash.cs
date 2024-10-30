using UnityEngine;
using UnityEngine.Events;

public class Dash : Skill
{
    [Header("Dash Settings")]
    [Tooltip("대시 Skill이 소모하는 Cost")]
    public float Cost = 20f;

    [Tooltip("대시의 힘 또는 속도")]
    public float Force = 20f;

    [Tooltip("대시 재사용 대기 시간")]
    public float Cooldown = 1f;

    [Header("Trail Settings")]
    [Tooltip("Trail의 활성화 시간")]
    public float TrailDuration = 0.5f;

    [Tooltip("Trail의 활성화 간격")]
    public float MeshRefreshRate = 0.1f;

    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private Cost _playerCost;
    private SkinnedMeshRenderer[] _skinnedMeshRanderers;

    [Header("Event")]
    public UnityEvent OnUse;
    public UnityEvent OnEnd;

    private float _cooldownTimer = 0f;
    private bool _isDashing = false;

    private void Update()
    {
        // 쿨다운 시간 업데이트
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }
        else
        {
            if (_isDashing)
            {
                _isDashing = false;
                OnEnd?.Invoke();
            }
        }
    }

    public override void UseSkill()
    {
        // 쿨다운 중이거나 이미 대시 중이면 스킬 사용 불가
        if (_cooldownTimer > 0f || _playerCost.Value < Cost)
        {
            return;
        }

        _playerCost.UseCost(Cost);
        StartDash();
    }

    private void StartDash()
    {
        Vector3 movementDirection = _playerMove.GetMovementDirection();

        if (movementDirection == Vector3.zero)
        {
            movementDirection = _playerTransform.forward;
        }
        else
        {
            movementDirection.z = movementDirection.y;
            movementDirection.y = 0f;
            movementDirection = _playerTransform.rotation * movementDirection.normalized;
        }

        // 대시 시작
        _cooldownTimer = Cooldown;
        _isDashing = true;
        _playerMove.AddForce(movementDirection.normalized * Force, ForceMode.Impulse);
        OnUse?.Invoke();
        StartCoroutine(ActivateTrail(TrailDuration));
    }

    private System.Collections.IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= MeshRefreshRate;

            yield return new WaitForSeconds(MeshRefreshRate);
        }
    }

    public override float GetCost()
    {
        return Cost;
    }
}
