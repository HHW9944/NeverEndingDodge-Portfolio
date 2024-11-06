using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Rendering;

public class CinematicBarsFadeScene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Volume _volume;
    private CinematicBars _cinematicBars;

    [Header("Settings")]
    [Tooltip("Fade 효과가 적용되는 시간")]
    public float FadeDuration = 1f;

    [Tooltip("트윈을 시작하기 전 지연 시간")]
    public float DelayBeforeFade = 0f;

    [Tooltip("Fade 효과에 사용할 Ease 타입")]
    public Ease FadeEase = Ease.Linear;

    void Start()
    {
        _volume.profile.TryGet(out _cinematicBars);
    }

    public void LoadScene(string sceneName)
    {
        // DOTween을 사용하여 fade와 amount 값을 트윈
        Sequence sequence = DOTween.Sequence();

        // 지연 시간 설정
        sequence.AppendInterval(DelayBeforeFade);

        // fade 값을 0에서 1f로 트윈
        sequence.Append(DOTween.To(() => _cinematicBars.fade.value, x => _cinematicBars.fade.value = x, 1f, FadeDuration)
            .SetEase(FadeEase));
        
        // amount 값을 0에서 0.51f로 트윈
        sequence.Join(DOTween.To(() => _cinematicBars.amount.value, x => _cinematicBars.amount.value = x, 0.51f, FadeDuration)
            .SetEase(FadeEase));

        // 트윈이 끝난 후 씬 전환
        sequence.OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
