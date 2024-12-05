using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LifeImageAdapter : MonoBehaviour
{
    [SerializeField] private Life _life;
    [SerializeField] private Image _image;

    [Header("Tweening Setting")]
    public float TweenDuration = 0.5f;
    public Ease TweenEase = Ease.Linear;

    private float _fillAmount;

    void Start()
    {
        _fillAmount = _image.fillAmount;
    }

    public void UpdateMPImage(int prev, int current)
    {
        float targetFillAmount = (float)current / _life.MaxValue * _fillAmount;
        _image.DOFillAmount(targetFillAmount, TweenDuration).SetEase(TweenEase);
    }
}