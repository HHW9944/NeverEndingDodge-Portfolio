using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkSpeed = 1.0f;  // 깜빡이는 속도
    [SerializeField] private TextMeshProUGUI tmpText;

    void Update()
    {
        // 알파 값 계산 (0과 1 사이를 반복)
        float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1.0f);

        // TextMeshProUGUI의 알파 값을 변경
        if (tmpText != null)
        {
            Color color = tmpText.color;
            color.a = alpha;
            tmpText.color = color;
        }
    }
}
