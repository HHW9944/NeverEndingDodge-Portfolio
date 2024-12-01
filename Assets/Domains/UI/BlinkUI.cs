using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkUI : MonoBehaviour
{
    public float blinkSpeed = 1.0f;
    private Image[] images;
    private TextMeshProUGUI[] texts;
    private float[] originalImageAlphas;
    private float[] originalTextAlphas;
    
    void Start()
    {
        images = GetComponentsInChildren<Image>(true);
        texts = GetComponentsInChildren<TextMeshProUGUI>(true);
        
        originalImageAlphas = new float[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            originalImageAlphas[i] = images[i].color.a;
        }
        
        originalTextAlphas = new float[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            originalTextAlphas[i] = texts[i].color.a;
        }
    }

    void Update()
    {
        float ratio = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);

        for (int i = 0; i < images.Length; i++)
        {
            Color color = images[i].color;
            color.a = originalImageAlphas[i] * ratio;
            images[i].color = color;
        }

        for (int i = 0; i < texts.Length; i++)
        {
            Color color = texts[i].color;
            color.a = originalTextAlphas[i] * ratio;
            texts[i].color = color;
        }
    }
}
