using UnityEngine;

[RequireComponent(typeof(Damage))]
public class Enemy : MonoBehaviour, IAttackable
{
    private Damage _damage;

    private void Awake()
    {
        _damage = GetComponent<Damage>();
    }

    public float GetDamage()
    {
        return _damage.Value;
    }

    public void Attack(IDamageable target)
    {
        target.TakeDamage(this, GetDamage());
    }

    public void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            Attack(target);
        }
    }
}