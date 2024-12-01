using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    public float Speed;
    [SerializeField] private RectTransform _creditPanel;
    public UnityEvent OnCreditEnd;

    private float _relativeHeight;

    void Start()
    {
        StartCoroutine(InitializeCredit());
    }

    private IEnumerator InitializeCredit()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_creditPanel);
        
        yield return null;
        
        _relativeHeight = _creditPanel.rect.height - Screen.height;
        _creditPanel.anchoredPosition = new Vector2(_creditPanel.anchoredPosition.x, -_relativeHeight);
    }
    
    public void StartCredit()
    {
        float duration = _relativeHeight / Speed;

        _creditPanel.DOAnchorPosY(_relativeHeight, duration).SetEase(Ease.Linear).OnComplete(() => OnCreditEnd.Invoke());
    }
}
