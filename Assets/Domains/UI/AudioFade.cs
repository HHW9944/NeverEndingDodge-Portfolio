using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Fade 효과가 적용되는 시간")]
    public float Duration = 2f;

    [Tooltip("Fade 효과에 사용할 Ease 타입")]
    public Ease EaseType = Ease.InOutQuad;

    [Tooltip("Scene 시작시 FadeIn 효과를 적용할지 여부")]
    public bool FadeInOnStart = false;

    void Start()
    {
        if (FadeInOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Fades(1f, Duration, EaseType);
    }

    public void FadeOut()
    {
        Fades(0f, Duration, EaseType);
    }

    private void Fades(float targetVolume, float duration, Ease easeType)
    {
        AudioListener.volume = 1f - targetVolume;

        DOTween.To(() => AudioListener.volume, v => AudioListener.volume = v, targetVolume, duration).SetUpdate(true)
            .SetEase(easeType);
            // .OnComplete(() => 
            // {
            //     if (targetVolume == 0f)
            //     {
            //         StopAllAudioSources();
            //         StopAllPlayableDirectors();
            //     }
                
            //     AudioListener.volume = 1f;
            // });
    }

    // private void StopAllAudioSources()
    // {
    //     AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
    //     foreach (AudioSource audioSource in allAudioSources)
    //     {
    //         audioSource.Stop();
    //     }
    // }

    // private void StopAllPlayableDirectors()
    // {
    //     UnityEngine.Playables.PlayableDirector[] directors = FindObjectsByType<UnityEngine.Playables.PlayableDirector>(FindObjectsSortMode.None);
    //     foreach (var director in directors)
    //     {
    //         director.Stop();
    //     }
    // }
}
