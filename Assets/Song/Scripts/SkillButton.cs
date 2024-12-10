using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Button skillButton; // 스킬 버튼
    public Skill skill; // 해당 스킬

    private bool isSkillActive = false; // 스킬 활성화 상태 추적
    private float holdTime = 0f; // 스킬을 꾹 누른 시간 추적
    public float requiredHoldTime = 1f; // 스킬 발동에 필요한 시간

    void Update()
    {
        // 터치 입력을 체크
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 첫 번째 터치 가져오기

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // 터치가 시작되면 스킬 활성화 준비
                    isSkillActive = false;
                    holdTime = 0f;
                    break;

                case TouchPhase.Moved:
                    // 터치가 이동하면 누른 시간이 증가
                    if (isSkillActive)
                    {
                        holdTime += Time.deltaTime;
                        // 일정 시간이 지나면 스킬 발동
                        if (holdTime >= requiredHoldTime)
                        {
                            skill.UseSkill(); // 스킬 발동
                            isSkillActive = false; // 발동 후 더 이상 활성화되지 않도록
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    // 터치가 끝날 때 스킬 비활성화
                    isSkillActive = false;
                    break;
            }
        }
    }

    public void OnButtonClicked()
    {
        if (!isSkillActive)
        {
            isSkillActive = true;
            holdTime = 0f;
        }
    }
}