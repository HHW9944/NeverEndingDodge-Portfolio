using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Cinemachine")]
    [Tooltip("카메라가 따라갈 Cinemachine Virtual Camera에 설정된 대상")]
    public GameObject CinemachineCameraTarget;

    [Header("Settings")]
    [Tooltip("카메라를 위로 얼마나 회전할 수 있는지 (도 단위)")]
    public float TopClamp = 90.0f;

    [Tooltip("카메라를 아래로 얼마나 회전할 수 있는지 (도 단위)")]
    public float BottomClamp = -90.0f;

    [Tooltip("캐릭터의 회전 속도")]
    public float Sensitivity = 1.0f;

    [Header("Mouse Cursor Settings")]
    public bool CursorLocked = true;

    private const float _threshold = 0.01f;
    private Vector2 _lookInput;
    private float _rotationVelocity;
    private float _cinemachineTargetPitch;

    private void OnApplicationFocus(bool hasFocus)
    {
        if (CursorLocked)
        {
            SetCursorState(CursorLocked);
        }
    }

    private void FixedUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        if (Cursor.lockState == CursorLockMode.None) return;

        // 입력이 있을 경우
        if (_lookInput.sqrMagnitude >= _threshold)
        {
            // 좌우 회전 처리
            _rotationVelocity = _lookInput.x * Sensitivity;
            transform.Rotate(Vector3.up * _rotationVelocity);

            // 위아래(피치) 회전 처리
            _cinemachineTargetPitch += _lookInput.y * Sensitivity;

            // 피치 회전을 제한
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // 플레이어 전체를 기울임 (카메라 대상이 아닌 플레이어 자체를 회전)
            transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, transform.eulerAngles.y, 0.0f);

            // OnLook.Invoke(new PlayerLookEvent(_rotationVelocity, _cinemachineTargetPitch));
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
