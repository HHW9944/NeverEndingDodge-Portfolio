using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [Header("References")]
    public Button skillButton; // ��ų ��ư
    public Image cooldownOverlay; // ��Ÿ�� �̹��� (���� ������)
    public Skill skill;

    private float cooldownTimer;
    private bool isCooldown = false;

    public void OnSkillActiveListener()
    {
        if (skill.GetCooldownTime() > 0)
        {
            isCooldown = true;
            cooldownTimer = skill.GetCooldownTime();

            cooldownOverlay.fillAmount = 0; // ��Ÿ�� �ʱ�ȭ
            skillButton.interactable = false; // ��ư ��Ȱ��ȭ
        }
    }

    void Start()
    {
        cooldownOverlay.fillAmount = 1; // ��Ÿ�� �ʱ�ȭ
        skillButton.interactable = true; // ��ư�� ó������ Ȱ��ȭ
    }

    void Update()
    {
        // ��Ÿ���� Ȱ��ȭ�� ���
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownOverlay.fillAmount = 1 - (cooldownTimer / skill.GetCooldownTime()); // �������� ä������ ȿ��

            // ��Ÿ�� ����
            if (cooldownTimer <= 0)
            {
                isCooldown = false;
                cooldownOverlay.fillAmount = 1;
                skillButton.interactable = true; // ��ư Ȱ��ȭ
            }
        }
    }
}