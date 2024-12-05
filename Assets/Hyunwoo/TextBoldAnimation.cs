using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextBoldEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText; // 애니메이션 적용 대상
    public float boldIntensity = 0.7f; // 굵기(외곽선) 최대값
    public float duration = 1f; // 애니메이션 주기
    private Tween boldTween; // DOTween 트윈 객체

    private void OnEnable()
    {
        StartBoldEffect(); // 활성화 시 애니메이션 시작
    }

    private void OnDisable()
    {
        StopBoldEffect(); // 비활성화 시 애니메이션 정지
    }

    private void StartBoldEffect()
    {
        if (boldTween != null && boldTween.IsPlaying())
        {
            boldTween.Kill(); // 기존 트윈 정지
        }

        // DOTween으로 외곽선 굵기 애니메이션 설정
        boldTween = DOTween.To(
                () => targetText.outlineWidth, // 현재 외곽선 굵기 가져오기
                x => targetText.outlineWidth = x, // 외곽선 굵기 설정
                boldIntensity, // 목표 외곽선 굵기
                duration // 애니메이션 지속 시간
            )
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복 (Yoyo: 증가 후 감소)
            .SetEase(Ease.InOutSine); // 부드러운 속도 변화
    }

    private void StopBoldEffect()
    {
        if (boldTween != null)
        {
            boldTween.Kill(); // 애니메이션 정지
            boldTween = null;
        }

        // 외곽선 굵기 초기화 (필요한 경우)
        targetText.outlineWidth = 0f;
    }
}
