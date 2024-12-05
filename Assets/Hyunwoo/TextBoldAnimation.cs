using UnityEngine;
using TMPro;
using DG.Tweening;

public class TextBoldEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText; // �ִϸ��̼� ���� ���
    public float boldIntensity = 0.7f; // ����(�ܰ���) �ִ밪
    public float duration = 1f; // �ִϸ��̼� �ֱ�
    private Tween boldTween; // DOTween Ʈ�� ��ü

    private void OnEnable()
    {
        StartBoldEffect(); // Ȱ��ȭ �� �ִϸ��̼� ����
    }

    private void OnDisable()
    {
        StopBoldEffect(); // ��Ȱ��ȭ �� �ִϸ��̼� ����
    }

    private void StartBoldEffect()
    {
        if (boldTween != null && boldTween.IsPlaying())
        {
            boldTween.Kill(); // ���� Ʈ�� ����
        }

        // DOTween���� �ܰ��� ���� �ִϸ��̼� ����
        boldTween = DOTween.To(
                () => targetText.outlineWidth, // ���� �ܰ��� ���� ��������
                x => targetText.outlineWidth = x, // �ܰ��� ���� ����
                boldIntensity, // ��ǥ �ܰ��� ����
                duration // �ִϸ��̼� ���� �ð�
            )
            .SetLoops(-1, LoopType.Yoyo) // ���� �ݺ� (Yoyo: ���� �� ����)
            .SetEase(Ease.InOutSine); // �ε巯�� �ӵ� ��ȭ
    }

    private void StopBoldEffect()
    {
        if (boldTween != null)
        {
            boldTween.Kill(); // �ִϸ��̼� ����
            boldTween = null;
        }

        // �ܰ��� ���� �ʱ�ȭ (�ʿ��� ���)
        targetText.outlineWidth = 0f;
    }
}
