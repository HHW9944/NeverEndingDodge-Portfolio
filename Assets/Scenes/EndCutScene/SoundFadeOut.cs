using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFadeOut : MonoBehaviour
{
    public float fadeOutDuration = 2f;  // 페이드 아웃에 걸리는 시간
    public float targetVolume = 0f;     // 목표 볼륨 (보통 0)
    
    public void StartFadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startVolume = AudioListener.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float newVolume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeOutDuration);
            AudioListener.volume = newVolume;
            yield return null;
        }

        // 먼저 볼륨을 0으로 설정
        AudioListener.volume = targetVolume;
        
        // 모든 AudioSource 찾아서 정지
        AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.volume = 0f;  // 볼륨을 0으로 설정
            audioSource.Stop();
        }
        
        // 타임라인 정지
        UnityEngine.Playables.PlayableDirector[] directors = 
            FindObjectsByType<UnityEngine.Playables.PlayableDirector>(FindObjectsSortMode.None);
        foreach (var director in directors)
        {
            director.Stop();
        }
        
        yield return new WaitForSeconds(0.1f);  // 약간의 딜레이 추가
        
        // 이후에 볼륨 초기화
        AudioListener.volume = 1f;
    }
}
