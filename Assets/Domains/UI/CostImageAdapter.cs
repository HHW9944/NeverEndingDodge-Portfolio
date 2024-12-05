using MPUIKIT;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CostImageAdapter : MonoBehaviour
{
    [SerializeField] private Cost _cost;
    [SerializeField] private Image _image;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private float _fillAmount;

    void Start()
    {
        _fillAmount = _image.fillAmount;
    }

    public void UpdateMPImage(float prev, float current)
    {
        float targetFillAmount = current / _cost.MaxCost * _fillAmount;
        _image.DOFillAmount(targetFillAmount, TweenDuration).SetEase(TweenEase);
    }
}