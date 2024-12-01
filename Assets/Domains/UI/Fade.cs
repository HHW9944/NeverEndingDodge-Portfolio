using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image _image;

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

    void Start()
    {
        if (FadeInOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fades(0f, FadeInDuration, EaseType, OnFadeInCompletes);
    }
    
    public void FadeOut()
    {
        Fades(1f, FadeOutDuration, EaseType, OnFadeOutCompletes);
    }

    private void Fades(float targetAlpha, float duration, Ease easeType, UnityEvent action)
    {
        _image.gameObject.SetActive(true);
        _image.color = new Color(0f, 0f, 0f, 1f - targetAlpha);
        
        DOTween.To(() => _image.color, v => _image.color = v, new Color(0f, 0f, 0f, targetAlpha), duration).SetUpdate(true)
            .SetEase(easeType)
            .OnComplete(() => 
            {
                action?.Invoke();
            });
    }
}