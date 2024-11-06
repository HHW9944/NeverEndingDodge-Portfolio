using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public static GameManager instance;  // 싱글톤 패턴으로 모든 씬에서 GamaManager에 쉽게 접근할 수 있도록 함.
    public Dropdown graphicsDropdown; //그래픽 설정을 위한 Dropdown 변수
    public Slider soundSlider; // 사운드 볼륨을 위한 Slider 변수
    public GameObject settingsPanel; // 설정 창 Panel 변수

    public AudioSource backgroundMusic;  // 배경음악 재생을 위한 AudioSource 변수

    private void Awake()
    {
#if UNITY_STANDALONE
        Screen.SetResolution(1920, 1080, false);
        Screen.fullScreen = false;
#endif
    }

    private void Start()
    {
        // 배경음악 재생
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;  // 음악이 계속 반복되도록 설정
            backgroundMusic.Play();       // 배경음악 재생 시작
        }

        //Dropdown에서 선택된 값이 변경될 때 호출되는 리스너
        if (graphicsDropdown != null)
        {
            graphicsDropdown.onValueChanged.AddListener(delegate { SetGraphicsQuality(graphicsDropdown.value); });
        }
        //Slider 값이 변경될 때 호출되는 리스너 추가
        if (soundSlider != null)
        {
            soundSlider.onValueChanged.AddListener(delegate { SetSoundVolume(soundSlider.value); });
            soundSlider.value = AudioListener.volume; // 시작할 때 현재 볼륨으로 초기화
        }

    }
    // 설정 버튼을 눌렀을 때 설정 패널을 표시하는 함수
    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            bool isActive = settingsPanel.activeSelf;
            settingsPanel.SetActive(!isActive); // 현재 상태의 반대로 설정 (켜기/끄기)
        }
    }

    //그래픽 품질 설정 함수
    public void SetGraphicsQuality(int index)
    {
        switch (index)
        {
            case 0:
                QualitySettings.SetQualityLevel(0); //Low
                Debug.Log("그래픽 품질 낮음으로 설정");
                break;
            case 1:
                QualitySettings.SetQualityLevel(2); //Middle
                Debug.Log("그래픽 품질 보통으로 설정");
                break;
            case 2:
                QualitySettings.SetQualityLevel(5); //High
                Debug.Log("그래픽 품질 높음으로 설정");
                break;
        }
    }

    //사운드 볼륨 설정 함수
    public void SetSoundVolume(float volume)
    {
        AudioListener.volume = volume; //전체 사운드 볼륨 설정
        Debug.Log("사운드 볼륨: " + volume);
    }

    //씬 전환 함수
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Debug.Log("게임 종료");
        Application.Quit();
    }
    // public void LoadSceneWithDelay(string sceneName, float delay)
    // {
    //     StartCoroutine(LoadSceneAfterDelay(sceneName, delay));
    // }
    // private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     SceneManager.LoadScene(sceneName);
    // }
    //=======================>이거는 씬이 즉시 전환되지않고 약간의 딜레이를 주려고 넣은 함수인데 일단 빼겠습니다.
}