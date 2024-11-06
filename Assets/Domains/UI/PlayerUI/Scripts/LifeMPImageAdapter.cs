using MPUIKIT;
using UnityEngine;
using DG.Tweening;

public class LifeMPImageAdapter : MonoBehaviour
{
    [SerializeField] private Life _life;
    [SerializeField] private MPImage _mpImage;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private float _fillAmount;

    void Start()
    {
        _fillAmount = _mpImage.fillAmount;
    }

    public void UpdateMPImage(int prev, int current)
    {
        float targetFillAmount = (float)current / _life.MaxValue * _fillAmount;
        _mpImage.DOFillAmount(targetFillAmount, TweenDuration).SetEase(TweenEase);
    }
}