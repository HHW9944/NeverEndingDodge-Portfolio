using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CinematicBarFade : MonoBehaviour
{
    [SerializeField] private Volume _volume;

    [Header("Settings")]
    [Tooltip("Fade In 효과가 적용되는 시간")]
    public float FadeInDuration = 2f;

    [Tooltip("Fade Out 효과가 적용되는 시간")]
    public float FadeOutDuration = 2f;

    [Tooltip("Fade 효과에 사용할 Ease 타입")]
    public Ease EaseType = Ease.InOutQuad;

    [Tooltip("Scene 시작시 FadeIn 효과를 적용할지 여부")]
    public bool FadeInOnStart = false;

    [Header("Events")]
    public UnityEvent OnFadeInCompletes;
    public UnityEvent OnFadeOutCompletes;

    private CinematicBars _cinematicBars;

    void Start()
    {
        _volume.profile.TryGet(out _cinematicBars);

        if (FadeInOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fades(0.01f, FadeInDuration, EaseType, OnFadeInCompletes);
    }
    
    public void FadeOut()
    {
        Fades(0.51f, FadeOutDuration, EaseType, OnFadeOutCompletes);
    }

    private void Fades(float targetValue, float duration, Ease easeType, UnityEvent action)
    {
        _cinematicBars.amount.value = 0.51f - targetValue;
        DOTween.To(() => _cinematicBars.amount.value, x => _cinematicBars.amount.value = x, targetValue, duration).SetUpdate(true)
            .SetEase(easeType)
            .OnComplete(() => 
            {
                action?.Invoke();
            });
    }
}