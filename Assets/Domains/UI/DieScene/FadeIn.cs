using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private Image _blackImage;

    [Header("Settings")]
    public float FadeDuration = 2f;
    private float elapsedTime = 0f;

    void Start()
    {
        _blackImage = GetComponent<Image>();
    }

    void Update()
    {
        if (elapsedTime < FadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / FadeDuration);
            _blackImage.color = new Color(0f, 0f, 0f, alpha);
        }
        else
        {
            // 페이드인이 완료되면 이미지 비활성화
            gameObject.SetActive(false);
        }
    }
}
