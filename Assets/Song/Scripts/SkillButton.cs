using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("References")]
    public Button skillButton; // 스킬 버튼
    public Image cooldownOverlay; // 쿨타임 이미지 (투명도 조절용)
    public Skill skill;

    private float cooldownTimer;
    private bool isCooldown = false;

    public bool isPressed = false; // 버튼이 눌려 있는 상태
    private float pressStartTime = 0f; // 눌린 시간 기록
    public float holdDuration = 1f; // 꾹 눌러야 하는 최소 시간

    public void OnSkillActiveListener()
    {
        if (skill.GetCooldownTime() > 0)
        {
            isCooldown = true;
            cooldownTimer = skill.GetCooldownTime();

            cooldownOverlay.fillAmount = 0; // 쿨타임 초기화
            skillButton.interactable = false; // 버튼 비활성화
        }
    }

    void Start()
    {
        cooldownOverlay.fillAmount = 1; // 쿨타임 초기화
        skillButton.interactable = true; // 버튼은 처음부터 활성화
    }

    void Update()
    {
        // 쿨타임이 활성화된 경우
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (cooldownTimer / skill.GetCooldownTime()); // 아이콘이 채워지는 효과

            // 쿨타임 종료
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                cooldownOverlay.fillAmount = 1;
                skillButton.interactable = true; // 버튼 활성화
            }
        }

        // 꾹 누르는 상태 처리
        if (isPressed)
        {
            if (Time.time - pressStartTime >= holdDuration) // 꾹 누른 상태가 유지되었는지 확인
            {
                Debug.Log("Skill Activated by Hold");
                ActivateSkill();
                isPressed = false; // 상태 초기화
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCooldown)
        {
            isPressed = true;
            pressStartTime = Time.time;
            Debug.Log("Button Pressed");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        pressStartTime = 0f;
        Debug.Log("Button Released");
    }

    private void ActivateSkill()
    {
        // Space 키 눌림과 동일한 효과를 여기에 구현
        OnSkillActiveListener(); // 스킬 활성화 로직 호출
    }
}
