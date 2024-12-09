using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Speed))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player")]
    // [Tooltip("가속도")]
    // public float Acceleration = 10.0f;
    
    [Tooltip("기울일 정도 (각도)")]
    public float TiltAmount = 15f;

    [Tooltip("기울임 회전 속도")]
    public float RotationSpeed = 5f;

    public float Speed
    {
        get
        {
            return _speed.Value;
        }
        set
        {
            _speed.Value = value;
        }
    }

    public Vector3 Velocity
    {
        get { return _rigid.velocity; }
    }

    public bool IsMoving
    {
        get 
        {
            return _moveInput.sqrMagnitude > 0.1f;
        }
    }

    public Vector2 LocalDirection
    {
        get 
        {
            return _moveInput;
        }
    }

    /* privite fields */
    private Rigidbody _rigid;
    private Speed _speed;
    private Vector2 _moveInput;
    
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _speed = GetComponent<Speed>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnMove(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    public Vector2 GetMovementDirection()
    {
        return _moveInput;
    }

    private void FixedUpdate()
    {
        // 2D 입력을 3D로 변환 (z축으로 좌우 이동, y축으로 위아래 이동)
        Vector3 moveDirection = new Vector3(_moveInput.x, _moveInput.y, 0f);

        // moveDirection을 로컬 좌표계 기준으로 변환 (플레이어가 바라보는 방향으로 이동)
        Vector3 forceDirection = transform.TransformDirection(moveDirection);

        // 이동 적용
        _rigid.MovePosition(_rigid.position + forceDirection * Speed * Time.fixedDeltaTime);

        // 위치 제한 (y: 위아래, z: 좌우 제한)
        Vector3 clampedPosition = _rigid.position;

        // y축 제한 (-15에서 +15까지)
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -6f, 6f);

        // z축 제한 (-20에서 +20까지)
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, -8f, 8f);

        // 제한된 위치로 강제로 설정
        _rigid.position = clampedPosition;
    }


    public void AddForce(Vector3 force, ForceMode mode)
    {
        _rigid.AddForce(force, mode);
    }
}
