using MPUIKIT;
using UnityEngine;
using DG.Tweening;

public class CostMPImageAdapter : MonoBehaviour
{
    [SerializeField] private Cost _cost;
    [SerializeField] private MPImage _mpImage;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private float _fillAmount;

    void Start()
    {
        _fillAmount = _mpImage.fillAmount;
    }

    public void UpdateMPImage(float prev, float current)
    {
        float targetFillAmount = current / _cost.MaxCost * _fillAmount;
        _mpImage.DOFillAmount(targetFillAmount, TweenDuration).SetEase(TweenEase);
    }
}