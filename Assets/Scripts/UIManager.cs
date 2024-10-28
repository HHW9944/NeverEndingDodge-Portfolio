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

    public TextMeshProUGUI countdownText; // ī��Ʈ�ٿ� �ؽ�Ʈ �߰�

    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    // TryAgain ��ư
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
        Debug.Log("UI �ʱ�ȭ");
        /* pauseMenuUICanvas.SetActive(false);*/  // PauseMenu UI�� ó���� ��Ȱ��ȭ�Ǿ� �־�� ��
        Debug.Log("pauseMenuUICanvas ���� (�ʱ�): " + pauseMenuUICanvas.activeSelf);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        tryAgainYesButton.onClick.AddListener(OnTryAgainYesButtonClick);
        tryAgainNoButton.onClick.AddListener(OnTryAgainNoButtonClick);

        countdownText.gameObject.SetActive(false); // ó������ ī��Ʈ�ٿ� ��Ȱ��ȭ
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

    // TryAgainYes ��ư Ŭ�� �� ���� �����
    private void OnTryAgainYesButtonClick()
    {
        GameManager.instance.TryAgain();
    }

    // TryAgainNo ��ư Ŭ�� �� ���� ����
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

    // ī��Ʈ�ٿ� UI ������Ʈ
    public void ShowCountdown(int count)
    {
        countdownText.gameObject.SetActive(true); // ī��Ʈ�ٿ� �ؽ�Ʈ ǥ��
        countdownText.text = count.ToString(); // ī��Ʈ�ٿ� ���� ǥ��
    }

    // ���� �÷��� UI�� �ٽ� �����ֱ�
    public void ShowGamePlayUI()
    {
        countdownText.gameObject.SetActive(false); // ī��Ʈ�ٿ� �ؽ�Ʈ �����
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        Debug.Log("ShowPauseMenu ȣ���");
        pauseMenuUICanvas.SetActive(true);  // PauseMenu UI Ȱ��ȭ
        Debug.Log("pauseMenuUICanvas ���� (Ȱ��ȭ �õ� ��): " + pauseMenuUICanvas.activeSelf);
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
        InvokeRepeating("BlinkWarningEffect", 0f, 0.5f); // 0.5�ʸ��� ��� ȿ�� ������
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
        InvokeRepeating("BlinkSkillIcon", 0f, 0.1f); // 0.1�ʸ��� �ݺ� ȣ���Ͽ� �����̴� ȿ��
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