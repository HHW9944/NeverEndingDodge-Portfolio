using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Life))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Skill Slots")]
    public Skill QSkill;
    public Skill RSkill;
    public Skill ShiftSkill;
    public Skill SpaceSkill;

    [Header("Properties")]
    public float DamageCooldown = 0.5f;

    private Life _life;
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
        yield return new WaitForSeconds(DamageCooldown);
        _canTakeDamage = true;
    }

    public void OnQ(InputValue value)
    {
        UseSkill(QSkill, value);
    }

    public void OnR(InputValue value)
    {
        UseSkill(RSkill, value);
    }

    public void OnShift(InputValue value)
    {
        UseSkill(ShiftSkill, value);
    }

    public void OnSpace(InputValue value)
    {
        UseSkill(SpaceSkill, value);
    }

    private void UseSkill(Skill skill, InputValue value)
    {
        if (value.isPressed)
        {
            skill.UseSkill();
        }
        else
        {
            if (skill is ICancellable cancellableSkill)
            {
                cancellableSkill.Cancel();
            }
        }
    }
}