using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Life))]
public class Player : MonoBehaviour, IDamageable
{
    private Life _life;

    [SerializeField] 
    private float _damageCooldown = 0.1f;
    private bool _canTakeDamage = true;

    private void Awake()
    {
        _life = GetComponent<Life>();
    }

    public void TakeDamage(IAttackable attacker, float damage)
    {
        if (_canTakeDamage)
        {
            _life.TakeDamage((int) damage);
            StartCoroutine(DamageCooldownRoutine());
        }
    }

    /* Hit Delay */
    private IEnumerator DamageCooldownRoutine()
    {
        _canTakeDamage = false;
        yield return new WaitForSeconds(_damageCooldown);
        _canTakeDamage = true;
    }
}