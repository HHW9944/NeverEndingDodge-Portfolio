using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly string X_PARAM_NAME = "X";
    private readonly string Y_PARAM_NAME = "Y";
    private readonly string SPEED_PARAM_NAME = "Speed";
    private readonly string DAMAGED_PARAM_NAME = "Damaged";
    private readonly string SKILL_1_PARAM_NAME = "Skill_1";
    private readonly string SKILL_2_PARAM_NAME = "Skill_2";
    private readonly string SKILL_3_PARAM_NAME = "Skill_3";
    private readonly string SKILL_4_PARAM_NAME = "Skill_4";

    [SerializeField] private Speed _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMove _playerMove;

    private Vector2 _smoothInputVector;
    
    void FixedUpdate()
    {
        _smoothInputVector = Vector2.Lerp(_smoothInputVector, _playerMove.LocalDirection, 0.15f);

        _animator.SetFloat(X_PARAM_NAME, _smoothInputVector.x);
        _animator.SetFloat(Y_PARAM_NAME, _smoothInputVector.y);

        _animator.SetFloat(SPEED_PARAM_NAME, Mathf.Max(_playerMove.Velocity.magnitude / _speed.InitValue, 1));
    }

    public void OnDamaged()
    {
        _animator.SetTrigger(DAMAGED_PARAM_NAME);
    }

    public void OnSkill_1()
    {
        _animator.SetBool(SKILL_1_PARAM_NAME, true);
    }

    public void OnSkill_2()
    {
        _animator.SetBool(SKILL_2_PARAM_NAME, true);
    }

    public void OnSkill_3()
    {
        _animator.SetBool(SKILL_3_PARAM_NAME, true);
    }

    public void OnSkill_4()
    {
        _animator.SetBool(SKILL_4_PARAM_NAME, true);
    }

    public void OnSkill_1End()
    {
        _animator.SetBool(SKILL_1_PARAM_NAME, false);
    }

    public void OnSkill_2End()
    {
        _animator.SetBool(SKILL_2_PARAM_NAME, false);
    }

    public void OnSkill_3End()
    {
        _animator.SetBool(SKILL_3_PARAM_NAME, false);
    }

    public void OnSkill_4End()
    {
        _animator.SetBool(SKILL_4_PARAM_NAME, false);
    }
}
