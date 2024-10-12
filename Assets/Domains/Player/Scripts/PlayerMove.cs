using UnityEngine.InputSystem;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Speed))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("가속도")]
    public float Acceleration = 10.0f;
    
    [Tooltip("기울일 정도 (각도)")]
    public float TiltAmount = 15f;

    [Tooltip("기울임 회전 속도")]
    public float RotationSpeed = 5f;

    public float Speed
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
    private Speed _speedComp;
    private Vector2 _moveInput;
    
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _speedComp = GetComponent<Speed>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // 2D 입력을 3D로 변환 (z축으로 전진, x축으로 좌우 이동)
        Vector3 moveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);

        // moveDirection을 로컬 좌표계 기준으로 변환 (플레이어가 바라보는 방향으로 이동)
        Vector3 forceDirection = transform.TransformDirection(moveDirection);

        // AddForce로 힘을 가해 이동 적용
        _rigid.AddForce(forceDirection * Acceleration, ForceMode.Acceleration);

        // 최대 속도 제한
        ClampMaxSpeed();

        // 이동 방향에 따라 플레이어 기울이기
        // ApplyTilt();
    }



    void ApplyTilt()
    {
        float speed = Velocity.magnitude;
        // 이동 중인 경우에만 기울임 적용
        if (Velocity.magnitude > 0.1f)
        {
            // 기울일 방향을 결정 (현재 이동 방향에 따른 X축 회전)
            float tiltAngle = Mathf.Clamp(speed * TiltAmount, -TiltAmount, TiltAmount);

            // Z축 기울이기 (좌우 이동 방향에 따라 기울이기)
            float targetTilt = _moveInput.x * TiltAmount;

            // 부드럽게 회전 (기울이기 적용)
            Quaternion targetRotation = Quaternion.Euler(tiltAngle, 0, -targetTilt);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    private void ClampMaxSpeed()
    {
        // 현재 속도가 최대 속도를 초과할 경우, 속도를 제한
        if (_rigid.velocity.magnitude > Speed)
        {
            // 속도의 방향을 유지하면서 최대 속도로 제한
            _rigid.velocity = _rigid.velocity.normalized * Speed;
        }
    }
}
