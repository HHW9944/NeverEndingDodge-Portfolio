using UnityEngine;

public interface IDamageable
{
    void TakeDamage(IAttackable attacker, float damage);
}

public interface ICollisionDamageable : IDamageable
{
    void TakeDamage(IAttackable attacker, float damage, Collision other);
}