using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("References")]
    public Button skillButton; // 스킬 버튼
    public Image cooldownOverlay; // 쿨타임 이미지 (투명도 조절용)
    public Skill skill;

    private float cooldownTimer;
    private bool isCooldown = false;

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
    }
}