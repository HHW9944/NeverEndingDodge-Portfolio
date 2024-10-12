using UnityEngine.InputSystem;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Speed))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("가속 및 감속")]
    public float SpeedChangeRate = 10.0f;

    public float MoveSpeed
    {
        get
        {
            return _speedComp.Value;
        }
        set
        {
            _speedComp.Value = value;
        }
    }

    private Vector2 _moveDir;
    private Rigidbody _rigid;
    private Speed _speedComp;
    
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _speedComp = GetComponent<Speed>();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float targetSpeed = _moveDir == Vector2.zero ? 0.0f : MoveSpeed;
        float speed;

        // 플레이어의 현재 수평 속도 계산
        Vector3 horizontalVelocity = new Vector3(_rigid.velocity.x, 0.0f, _rigid.velocity.z);
        float currentHorizontalSpeed = horizontalVelocity.magnitude;

        // 가속 및 감속 처리
        float speedOffset = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }


        // 이동 방향 벡터 계산
        Vector3 inputDirection = new Vector3(_moveDir.x, 0.0f, _moveDir.y).normalized;
        if (_moveDir != Vector2.zero)
        {
            inputDirection = transform.right * _moveDir.x + transform.forward * _moveDir.y;
        }

        // Rigidbody로 이동 처리
        Vector3 moveVector = inputDirection * speed;
        _rigid.MovePosition(_rigid.position + moveVector * Time.fixedDeltaTime);
    }

    public void OnMove(InputValue value)
    {
        _moveDir = value.Get<Vector2>();
    }
}
