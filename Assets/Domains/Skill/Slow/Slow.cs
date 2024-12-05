using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slow : Skill
{
    [Header("Attributes")]
    [Tooltip("Skill이 소모하는 Cost")]
    public float Cost = 20f;

    [Tooltip("슬로우 모션 지속 시간")]
    public float slowMotionDuration = 3f;

    [Tooltip("슬로우 모션 속도")]
    public float slowMotionScale = 0.2f;

    [Tooltip("쿨타임(초)")]
    public float cooldownTime = 5f;

    [Tooltip("Solid Color 설정")]
    public Color solidColor = Color.black;

    [Header("References")]
    [SerializeField] private AudioSource _bgmAudioSource; // BGM AudioSource
    [SerializeField] private Camera _mainCamera; // 메인 카메라 참조
    [SerializeField] private Cost _playerCost;
    
    [Header("Events")]
    public UnityEvent onSkillActivate;

    [Header("Debug")]
    public bool IsDebug = false;

    private CameraClearFlags _originalClearFlags; // 원래의 Clear Flags 저장
    private Color _originalBackgroundColor; // 원래의 Background Color 저장
    private float cooldownTimer;
    private bool _isCooldown = false;
    private bool _isActive = false;

    public override float GetCost()
    {
        throw new System.NotImplementedException();
    }

    public override void UseSkill()
    {
        if (_isCooldown || _isActive) return;
        _playerCost.UseCost(Cost);
        _isActive = true;
        StartCoroutine(SlowMotionEffect());
        onSkillActivate?.Invoke();
    }

    public override float GetCooldownTime()
    {
        return cooldownTime;
    }

    private void Start()
    {
        // Main Camera 설정 저장
        if (_mainCamera != null)
        {
            _originalClearFlags = _mainCamera.clearFlags; // 원래 Clear Flags 저장
            _originalBackgroundColor = _mainCamera.backgroundColor; // 원래 배경색 저장
        }
    }

    IEnumerator SlowMotionEffect()
    {
        StartSlowMotion();
        yield return new WaitForSecondsRealtime(slowMotionDuration); // 슬로우 모션 지속 시간
        EndSlowMotion();
    }

    void StartSlowMotion()
    {
        Time.timeScale = slowMotionScale; // 슬로우 모션 적용
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // FixedUpdate 시간 조정
        // BGM 속도 조절
        if (_bgmAudioSource != null)
        {
            _bgmAudioSource.pitch = slowMotionScale; // BGM 속도를 슬로우 모션 속도에 맞춤
        }
        // 카메라 Background Type 변경 (Solid Color)
        if (_mainCamera != null)
        {
            _mainCamera.clearFlags = CameraClearFlags.SolidColor; // 배경을 Solid Color로 변경
            _mainCamera.backgroundColor = solidColor; // 배경색 설정
        }

        if (IsDebug) Debug.Log("슬로우 모션 시작");
    }

    void EndSlowMotion()
    {
        Time.timeScale = 1f; // 시간 복구
        Time.fixedDeltaTime = 0.02f; // 기본값 복구
        // BGM 속도 복구
        if (_bgmAudioSource != null)
        {
            _bgmAudioSource.pitch = 1f; // BGM 속도를 원래대로 복구
        }
        // 카메라 Background Type 복구 (Skybox)
        if (_mainCamera != null)
        {
            _mainCamera.clearFlags = _originalClearFlags; // 원래 Clear Flags 복구
            _mainCamera.backgroundColor = _originalBackgroundColor; // 원래 배경색 복구
        }

        if (IsDebug) Debug.Log("슬로우 모션 종료");
        _isActive = false;
        StartCooldown(); // 슬로우 모션 종료 후 쿨타임 시작
    }

    void StartCooldown()
    {
        _isCooldown = true; // 쿨타임 활성화
        cooldownTimer = cooldownTime; // 쿨타임 설정
    }

    void Update()
    {
        // 쿨타임이 활성화된 경우
        if (_isCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            // 쿨타임 종료
            if (cooldownTimer <= 0)
            {
                _isCooldown = false;
            }
        }
    }
}
