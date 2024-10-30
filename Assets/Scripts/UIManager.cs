using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenuUICanvas;
    public GameObject gamePlayUICanvas;
    public GameObject gameOverUICanvas; // ���� Game Over UI ���

    public TextMeshProUGUI countdownText; // ī��Ʈ�ٿ� �ؽ�Ʈ �߰�
    public TextMeshProUGUI timerText; // Ÿ�̸� �ؽ�Ʈ
    public Image[] life; // ���� UI �迭 �߰�

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

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playTimeResult;
    public Image skillIcon01;

    private bool WarningEffectBlinking = false;

    public RectTransform enemyIndicatorPrefab; // �� ������ ǥ���� UI ������
    private Dictionary<Transform, RectTransform> enemyIndicators = new Dictionary<Transform, RectTransform>();

    private void Awake()
    {
        instance = this;
        Debug.Log("UI �ʱ�ȭ");

        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        tryAgainYesButton.onClick.AddListener(OnTryAgainYesButtonClick);
        tryAgainNoButton.onClick.AddListener(OnTryAgainNoButtonClick);

        countdownText.gameObject.SetActive(false); // ó������ ī��Ʈ�ٿ� ��Ȱ��ȭ
    }

    void Update()
    {
        if (GameManager.isGameOver)
        {
            pauseMenuUICanvas.SetActive(false);
            gamePlayUICanvas.SetActive(false);
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

        // Ÿ�̸� UI ������Ʈ
        timerText.text = GameManager.timer.ToString("F1");

        // �Ÿ� �ؽ�Ʈ ������Ʈ
        distanceMiddle = (int)GameManager.distanceFromMiddle;
        distanceMiddleText.text = distanceMiddle.ToString() + " M";

        if (distanceMiddle >= 100)
        {
            if (WarningEffectBlinking)
            {
                StopBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(false);
        }
        else if(distanceMiddle >= 90)
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

    // Ÿ�̸� UI ������Ʈ
    public void UpdateTimerUI(int minute, int second)
    {
        timerText.text = string.Format("{0:00} : {1:00}", minute, second);
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
    public void HideAllUI()
    {
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(false);
        gameOverUICanvas.SetActive(false);
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

    public void UpdateEnemyIndicator(Transform enemyTransform)
    {
        if (!enemyIndicators.ContainsKey(enemyTransform))
        {
            // ���� UI �ε������Ͱ� ������ ����
            RectTransform indicator = Instantiate(enemyIndicatorPrefab, gamePlayUICanvas.transform);
            enemyIndicators[enemyTransform] = indicator;
        }

        RectTransform indicatorUI = enemyIndicators[enemyTransform];

        // ������ �Ÿ� ���
        float distance = Vector3.Distance(Camera.main.transform.position, enemyTransform.position);

        // �Ÿ��� 30 �̻��̸� �ε������� ����
        if (distance >= 30)
        {
            indicatorUI.gameObject.SetActive(false);
            return;
        }

        // ���� ��ġ�� ī�޶� ��ǥ��� ��ȯ
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemyTransform.position);

        // z ���� ������ ���, ���� ī�޶� �ڿ� �ִ� ����.
        if (screenPoint.z < 0)
        {
            // ī�޶� �ڿ� ���� �� �ݴ� �������� �ε������͸� ǥ���ϵ��� ����
            screenPoint.x = 1f - screenPoint.x;
            screenPoint.y = 1f - screenPoint.y;
            screenPoint.z = 0;
        }

        // ȭ�� ���� Ŭ����
        screenPoint.x = Mathf.Clamp(screenPoint.x, 0f, 1f);
        screenPoint.y = Mathf.Clamp(screenPoint.y, 0f, 1f);

        // ȭ���� �߽����κ��� ������ ����Ͽ� ȭ�� �����ڸ��� ǥ��
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = new Vector2(screenPoint.x - screenCenter.x, screenPoint.y - screenCenter.y);
        direction.Normalize();

        // ȭ�� �����ڸ��� ��ġ ���
        float edgeBuffer = 50f; // �����ڸ��κ����� ���� �Ÿ�
        float x = direction.x * (Screen.width / 2 - edgeBuffer);
        float y = direction.y * (Screen.height / 2 - edgeBuffer);

        indicatorUI.anchoredPosition = new Vector2(x, y);

        // ������ �Ÿ� ������� ũ�� ���� (�������� �� �� ���̵��� ũ�� ����)
        // �ִ� �Ÿ��� 30�̹Ƿ� �� ���� ������ ũ�⸦ ����
        float size = Mathf.Lerp(100, 20, distance / 30); // �Ÿ��� ���� ���������� ũ�� ����
        indicatorUI.sizeDelta = new Vector2(size, size);

        // �ε������Ͱ� ǥ�õǵ��� ����
        indicatorUI.gameObject.SetActive(true);

        // �ε������� �����̴� ȿ�� �߰�
        float alpha = Mathf.PingPong(Time.time * 2, 1); // 0���� 1 ������ ���� �ݺ������� ��ȯ (������)
        indicatorUI.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }

    public void RemoveEnemyIndicator(Transform enemyTransform)
    {
        if (enemyIndicators.ContainsKey(enemyTransform))
        {
            // ���� ȭ�鿡 ���̸� �ε������� ����
            Destroy(enemyIndicators[enemyTransform].gameObject);
            enemyIndicators.Remove(enemyTransform);
        }
    }
}
