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

    public UnityEvent onBarrierEnable;
    public UnityEvent onBarrierDisable;
    public UnityEvent onBarrierCollision;

    private bool _isBarrierEnabled = false;

    public override void UseSkill()
    {
        if (_playerCost.Value < Cost)
        {
            return;
        }

        onBarrierEnable?.Invoke();
        _isBarrierEnabled = true;
    }

    public override float GetCost()
    {
        return 0;
    }

    public void Cancel()
    {
        onBarrierDisable?.Invoke();
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
}