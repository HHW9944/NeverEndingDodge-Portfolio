using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubtitleManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text _subtitleText;

    [Header("Settings")]
    [Tooltip("글자당 나타나는 속도")]
    public float TypingSpeed = 0.05f;

    [Tooltip("텍스트가 화면에 유지되는 시간")]
    public float DisplayDuration = 2f;

    [Tooltip("사라지는 데 걸리는 시간")]
    public float FadeOutDuration = 1f;

    [Header("Events")]
    public UnityEvent OnSubtitleShown;

    private Sequence _currentSubtitleSequence; // 현재 재생 중인 자막 시퀀스

    public void ShowSubtitle(string text)
    {
        // 현재 재생 중인 자막이 있으면 중단
        _currentSubtitleSequence?.Kill();

        // 텍스트 설정 및 초기화
        _subtitleText.text = text;
        _subtitleText.maxVisibleCharacters = 0; // 처음엔 보이지 않음
        _subtitleText.alpha = 1f; // 투명도 초기화

        OnSubtitleShown?.Invoke();

        // 새로운 시퀀스 생성
        _currentSubtitleSequence = DOTween.Sequence();
        
        // 타이핑 효과
        _currentSubtitleSequence.Append(
            DOTween.To(() => _subtitleText.maxVisibleCharacters, x => _subtitleText.maxVisibleCharacters = x, text.Length, text.Length * TypingSpeed)
            .SetEase(Ease.Linear)
        );
        
        // 화면에 일정 시간 유지
        _currentSubtitleSequence.AppendInterval(DisplayDuration);
        
        // 페이드 아웃 효과
        _currentSubtitleSequence.Append(_subtitleText.DOFade(0, FadeOutDuration));
    }
}
