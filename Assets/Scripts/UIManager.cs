using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenuUICanvas;
    public GameObject gamePlayUICanvas;
    public GameObject gameOverUICanvas; // 기존 Game Over UI 사용

    public TextMeshProUGUI countdownText; // 카운트다운 텍스트 추가
    public TextMeshProUGUI timerText; // 타이머 텍스트
    public Image[] life; // 생명 UI 배열 추가

    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    // TryAgain 버튼
    public Button tryAgainYesButton;
    public Button tryAgainNoButton;

    public Image distance;
    public int distanceMiddle;
    public TextMeshProUGUI distanceMiddleText;

    public Image warningEffect;
    public Color warningEffectColor;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playTimeResult;
    public Image skillIcon01;

    private bool WarningEffectBlinking = false;

    public RectTransform enemyIndicatorPrefab; // 적 방향을 표시할 UI 프리팹
    private Dictionary<Transform, RectTransform> enemyIndicators = new Dictionary<Transform, RectTransform>();

    private Volume volume;
    private Vignette vignette;

    private float targetIntensity = 2f;
    private float intensityChangeSpeed = 1f;

    private bool shutdown = false;

    private void Awake()
    {
        instance = this;
        Debug.Log("UI 초기화");

        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        tryAgainYesButton.onClick.AddListener(OnTryAgainYesButtonClick);
        tryAgainNoButton.onClick.AddListener(OnTryAgainNoButtonClick);

        countdownText.gameObject.SetActive(false); // 처음에는 카운트다운 비활성화

        volume = FindObjectOfType<Volume>();
        if (volume != null)
        {
            volume.profile.TryGet<Vignette>(out vignette);
        }
    }

    void Update()
    {
        Debug.Log(vignette.intensity.value);

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

        // 타이머 UI 업데이트
        timerText.text = GameManager.timer.ToString("F1");

        // 거리 텍스트 업데이트
        distanceMiddle = (int)GameManager.distanceFromMiddle;
        if (distanceMiddle >= 20)
        {
            distanceMiddleText.text = "? M"; // 100 이상일 때는 "? M"
            distanceMiddleText.color = Color.white; // 기본 색상 (예: 흰색)
        }
        else if (distanceMiddle >= 5)
        {
            distance.color = Color.red;
            distanceMiddleText.text = distanceMiddle.ToString() + " M"; // 거리 표시
            distanceMiddleText.color = Color.red; // 빨간색으로 표시
        }
        else
        {
            distance.color = Color.white;
            distanceMiddleText.text = distanceMiddle.ToString() + " M"; // 거리 표시
            distanceMiddleText.color = Color.white; // 기본 색상 (예: 흰색)
        }

        if (distanceMiddle >= 20)
        {
            shutdown = true;

            if (WarningEffectBlinking)
            {
                StopBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(false);
            /*ResetVignetteEffect();*/
            vignette.color.Override(Color.black);
            float currentIntensity = vignette.intensity.value;
            // 강도를 목표 강도로 천천히 증가
            if (currentIntensity < targetIntensity)
            {
                vignette.intensity.Override(Mathf.MoveTowards(currentIntensity, targetIntensity, intensityChangeSpeed * Time.deltaTime));
            }
        }
        else if (distanceMiddle >= 5)
        {
            float currentIntensity = vignette.intensity.value;

            if (currentIntensity > 0 && shutdown)
            {
                vignette.intensity.Override(Mathf.MoveTowards(currentIntensity, 0, 2 * intensityChangeSpeed * Time.deltaTime));
            }
            else
            {
                shutdown = false;
                if (!WarningEffectBlinking)
                {
                    StartBlinkWarningEffect();
                }
                warningEffect.gameObject.SetActive(true);
            }
        }
        else
        {
            if (WarningEffectBlinking)
            {
                StopBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(false);
            ResetVignetteEffect();
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

    // 타이머 UI 업데이트
    public void UpdateTimerUI(int minute, int second)
    {
        timerText.text = string.Format("{0:00} : {1:00}", minute, second);
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
    public void HideAllUI()
    {
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(false);
        gameOverUICanvas.SetActive(false);
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
        if (warningEffect.color.a != 0f)
        {
            warningEffect.color = new Color(warningEffect.color.r, warningEffect.color.g, warningEffect.color.b, 0f);
            vignette.color.Override(Color.red);
            vignette.intensity.Override(0.7f);
        }
        else
        {
            warningEffect.color = new Color(warningEffect.color.r, warningEffect.color.g, warningEffect.color.b, 50 / 255f);
            vignette.color.Override(Color.red);
            vignette.intensity.Override(0.8f);
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

    public void UpdateEnemyIndicator(Transform enemyTransform)
    {
        if (!enemyIndicators.ContainsKey(enemyTransform))
        {
            // 적의 UI 인디케이터가 없으면 생성
            RectTransform indicator = Instantiate(enemyIndicatorPrefab, gamePlayUICanvas.transform);
            enemyIndicators[enemyTransform] = indicator;
        }

        RectTransform indicatorUI = enemyIndicators[enemyTransform];

        // 적과의 거리 계산
        float distance = Vector3.Distance(Camera.main.transform.position, enemyTransform.position);

        // 거리가 30 이상이면 인디케이터 숨김
        if (distance >= 30)
        {
            indicatorUI.gameObject.SetActive(false);
            return;
        }

        // 적의 위치를 카메라 좌표계로 변환
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemyTransform.position);

        // z 값이 음수인 경우, 적이 카메라 뒤에 있는 것임.
        if (screenPoint.z < 0)
        {
            // 카메라 뒤에 있을 때 반대 방향으로 인디케이터를 표시하도록 수정
            screenPoint.x = 1f - screenPoint.x;
            screenPoint.y = 1f - screenPoint.y;
            screenPoint.z = 0;
        }

        // 화면 범위 클램프
        screenPoint.x = Mathf.Clamp(screenPoint.x, 0f, 1f);
        screenPoint.y = Mathf.Clamp(screenPoint.y, 0f, 1f);

        // 화면의 중심으로부터 방향을 계산하여 화면 가장자리로 표시
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = new Vector2(screenPoint.x - screenCenter.x, screenPoint.y - screenCenter.y);
        direction.Normalize();

        // 화면 가장자리로 위치 계산
        float edgeBuffer = 50f; // 가장자리로부터의 여유 거리
        float x = direction.x * (Screen.width / 2 - edgeBuffer);
        float y = direction.y * (Screen.height / 2 - edgeBuffer);

        indicatorUI.anchoredPosition = new Vector2(x, y);

        // 적과의 거리 기반으로 크기 조정 (가까울수록 더 잘 보이도록 크기 증가)
        // 최대 거리가 30이므로 이 범위 내에서 크기를 조정
        float size = Mathf.Lerp(100, 20, distance / 30); // 거리에 따라 선형적으로 크기 감소
        indicatorUI.sizeDelta = new Vector2(size, size);

        // 인디케이터가 표시되도록 설정
        indicatorUI.gameObject.SetActive(true);

        // 인디케이터 깜빡이는 효과 추가
        float alpha = Mathf.PingPong(Time.time * 2, 1); // 0에서 1 사이의 값을 반복적으로 반환 (깜빡임)
        indicatorUI.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }

    public void RemoveEnemyIndicator(Transform enemyTransform)
    {
        if (enemyIndicators.ContainsKey(enemyTransform))
        {
            // 적이 화면에 보이면 인디케이터 삭제
            Destroy(enemyIndicators[enemyTransform].gameObject);
            enemyIndicators.Remove(enemyTransform);
        }
    }
    public void OnDamage(int currentLife, int damage)
    {
        for (int life = currentLife; life < currentLife + damage; life++)
        {
            this.life[life].color = new Color(1, 1, 1, 0.1f);
        }
    }

    public void OnDie()
    {
        /*UI[0].SetActive(true);*/
    }

    private void StartVignetteEffect()
    {
        if (vignette != null)
        {
            vignette.color.Override(new Color(1f, 110 / 255f, 0f, 1f));
            vignette.intensity.Override(0.8f); // Vignette 강도 설정 (필요에 따라 조절)
        }
    }

    private void ResetVignetteEffect()
    {
        if (vignette != null)
        {
            vignette.intensity.Override(0.0f); // Vignette 강도 리셋
        }
    }
}
