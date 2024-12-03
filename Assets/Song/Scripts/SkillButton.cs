using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button skillButton; // 스킬 버튼
    public Image cooldownOverlay; // 쿨타임 이미지 (투명도 조절용)
    public float cooldownTime = 5f; // 쿨타임(초)
    public float slowMotionDuration = 3f; // 슬로우 모션 지속 시간
    public float slowMotionScale = 0.2f; // 슬로우 모션 속도
    private bool isCooldown = false;
    private bool isSkillActive = false; // 스킬 활성화 여부
    private float cooldownTimer;

    public AudioSource bgmAudioSource; // BGM AudioSource


    void Start()
    {
        skillButton.onClick.AddListener(ActivateSkill);
        cooldownOverlay.fillAmount = 1; // 쿨타임 초기화
        skillButton.interactable = true; // 버튼은 처음부터 활성화
    }

    void Update()
    {
        // Q 키로 스킬 활성화
        if (Input.GetKeyDown(KeyCode.Q) && !isSkillActive && !isCooldown)
        {
            ActivateSkill();
        }

        // 쿨타임이 활성화된 경우
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (cooldownTimer / cooldownTime); // 아이콘이 채워지는 효과


            // 쿨타임 종료
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                cooldownOverlay.fillAmount = 1;
                skillButton.interactable = true; // 버튼 활성화
            }
        }
    }

    void ActivateSkill()
    {
        if (!isSkillActive && !isCooldown)
        {
            // 스킬 발동 처리
            Debug.Log("스킬 발동!");
            StartCoroutine(SlowMotionEffect());
            isSkillActive = true; // 스킬 활성화 상태
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
        if (bgmAudioSource != null)
        {
            bgmAudioSource.pitch = slowMotionScale; // BGM 속도를 슬로우 모션 속도에 맞춤
        }
        Debug.Log("슬로우 모션 시작!");
    }

    void EndSlowMotion()
    {
        Time.timeScale = 1f; // 시간 복구
        Time.fixedDeltaTime = 0.02f; // 기본값 복구
         // BGM 속도 복구
        if (bgmAudioSource != null)
        {
            bgmAudioSource.pitch = 1f; // BGM 속도를 원래대로 복구
        }
        Debug.Log("슬로우 모션 종료!");
        StartCooldown(); // 슬로우 모션 종료 후 쿨타임 시작
        isSkillActive = false; // 스킬 비활성화 상태
    }

    void StartCooldown()
    {
        isCooldown = true; // 쿨타임 활성화
        cooldownTimer = cooldownTime; // 쿨타임 설정
        cooldownOverlay.fillAmount = 0; // 쿨타임 초기화
        skillButton.interactable = false; // 버튼 비활성화
    }
}
