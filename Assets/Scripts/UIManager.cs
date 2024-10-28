using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenuUICanvas;
    public GameObject gamePlayUICanvas;
    public GameObject gameOverUICanvas;

    public TextMeshProUGUI countdownText; // 카운트다운 텍스트 추가

    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    // TryAgain 버튼
    public Button tryAgainYesButton;
    public Button tryAgainNoButton;

    public int distanceMiddle;
    public TextMeshProUGUI distanceMiddleText;

    public Image warningEffect;
    public Color warningEffectColor;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playTimeResult;
    public Image skillIcon01;

    private bool WarningEffectBlinking = false;

    private void Awake()
    {
        instance = this;
        Debug.Log("UI 초기화");
        /* pauseMenuUICanvas.SetActive(false);*/  // PauseMenu UI가 처음에 비활성화되어 있어야 함
        Debug.Log("pauseMenuUICanvas 상태 (초기): " + pauseMenuUICanvas.activeSelf);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        tryAgainYesButton.onClick.AddListener(OnTryAgainYesButtonClick);
        tryAgainNoButton.onClick.AddListener(OnTryAgainNoButtonClick);

        countdownText.gameObject.SetActive(false); // 처음에는 카운트다운 비활성화
    }

    private void OnResumeButtonClick()
    {
        GameManager.instance.ResumeGame();
    }

    private void OnRestartButtonClick()
    {
        GameManager.instance.RestartGame();
    }

    private void OnQuitButtonClick()
    {
        Application.Quit();
    }

    // TryAgainYes 버튼 클릭 시 게임 재시작
    private void OnTryAgainYesButtonClick()
    {
        GameManager.instance.TryAgain();
    }

    // TryAgainNo 버튼 클릭 시 게임 종료
    private void OnTryAgainNoButtonClick()
    {
        Application.Quit();
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            pauseMenuUICanvas.SetActive(false);
            gamePlayUICanvas.SetActive(false);
            gameOverUICanvas.SetActive(true);
            return;
        }

        if (GameManager.isPaused)
        {
            if (WarningEffectBlinking)
            {
                WarningEffectBlinking = false;
                StopBlinkWarningEffect();
            }

            pauseMenuUICanvas.SetActive(true);
            gamePlayUICanvas.SetActive(false);
            gameOverUICanvas.SetActive(false);
        }
        else
        {
            pauseMenuUICanvas.SetActive(false);
            gamePlayUICanvas.SetActive(true);
            gameOverUICanvas.SetActive(false);
        }

        timerText.text = GameManager.timer.ToString("F1");

        distanceMiddle = (int)GameManager.distanceFromMiddle;
        distanceMiddleText.text = distanceMiddle.ToString() + " M";

        if (distanceMiddle > 90)
        {
            if (!WarningEffectBlinking)
            {
                StartBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(true);
        }
        else
        {
            if (WarningEffectBlinking)
            {
                StopBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(false);
        }
    }

    // 카운트다운 UI 업데이트
    public void ShowCountdown(int count)
    {
        countdownText.gameObject.SetActive(true); // 카운트다운 텍스트 표시
        countdownText.text = count.ToString(); // 카운트다운 값을 표시
    }

    // 게임 플레이 UI를 다시 보여주기
    public void ShowGamePlayUI()
    {
        countdownText.gameObject.SetActive(false); // 카운트다운 텍스트 숨기기
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        Debug.Log("ShowPauseMenu 호출됨");
        pauseMenuUICanvas.SetActive(true);  // PauseMenu UI 활성화
        Debug.Log("pauseMenuUICanvas 상태 (활성화 시도 후): " + pauseMenuUICanvas.activeSelf);
        gamePlayUICanvas.SetActive(false);
        gameOverUICanvas.SetActive(false);
    }

    public void HidePauseMenu()
    {
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
    }

    public void ShowGameOverUI()
    {
        playTimeResult.text = "Play Time :  " + timerText.text;
        gameOverUICanvas.SetActive(true);
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(false);
    }

    public void HideGameOverUI()
    {
        gameOverUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        pauseMenuUICanvas.SetActive(false);
    }

    public void StartBlinkWarningEffect()
    {
        WarningEffectBlinking = true;
        InvokeRepeating("BlinkWarningEffect", 0f, 0.5f); // 0.5초마다 경고 효과 깜빡임
    }

    public void StopBlinkWarningEffect()
    {
        WarningEffectBlinking = false;
        CancelInvoke("BlinkWarningEffect");
    }

    private void BlinkWarningEffect()
    {
        if (warningEffect.color.a == 1f)
        {
            warningEffect.color = new Color(warningEffect.color.r, warningEffect.color.g, warningEffect.color.b, 0f);
        }
        else
        {
            warningEffect.color = new Color(warningEffect.color.r, warningEffect.color.g, warningEffect.color.b, 1f);
        }
    }

    public void StartSkill01Effect()
    {
        Skill01On();
        InvokeRepeating("BlinkSkillIcon", 0f, 0.1f); // 0.1초마다 반복 호출하여 깜빡이는 효과
    }

    public void StopSkill01Effect()
    {
        CancelInvoke("BlinkSkillIcon");
        Skill01Off();
    }

    private void BlinkSkillIcon()
    {
        if (skillIcon01.color.a == 1f)
        {
            skillIcon01.color = new Color(skillIcon01.color.r, skillIcon01.color.g, skillIcon01.color.b, 0.3f);
        }
        else
        {
            skillIcon01.color = new Color(skillIcon01.color.r, skillIcon01.color.g, skillIcon01.color.b, 1f);
        }
    }

    public void Skill01On()
    {
        skillIcon01.color = new Color(skillIcon01.color.r, skillIcon01.color.g, skillIcon01.color.b, 1f);
    }

    public void Skill01Off()
    {
        skillIcon01.color = new Color(skillIcon01.color.r, skillIcon01.color.g, skillIcon01.color.b, 5f / 255f);
    }
}