using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private readonly string X_PARAM_NAME = "X";
    private readonly string Y_PARAM_NAME = "Y";
    private readonly string SPEED_PARAM_NAME = "Speed";
    private readonly string DAMAGED_PARAM_NAME = "Damaged";

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
}
