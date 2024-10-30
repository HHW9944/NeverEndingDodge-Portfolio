using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Life))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Skill Slots")]
    public Skill LeftClickSkill;
    public Skill RightClickSkill;
    public Skill ShiftSkill;
    public Skill SpaceSkill;

    private Life _life;
    private float _damageCooldown = 0.5f;
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

    public void OnLeftClick(InputValue value)
    {
        UseSkill(LeftClickSkill, value);
    }

    public void OnRightClick(InputValue value)
    {
        UseSkill(RightClickSkill, value);
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