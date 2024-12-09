using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Timer GlobalTimer;

    public GameObject pauseMenuUICanvas;
    public GameObject gamePlayUICanvas;
    public GameObject gameOverUICanvas; // ???? Game Over UI ???

    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    // TryAgain ???
    public Button tryAgainYesButton;
    public Button tryAgainNoButton;

    public Image distance;
    public int distanceMiddle;
    public TextMeshProUGUI distanceMiddleText;

    private const float warningArea = 99999f;
    private const float shutdownArea = 99999f;

    public Image warningEffect;
    public Color warningEffectColor;

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI playTimeResult;

    public Image skillIcon01;
    public Image skillIcon02;
    public Image skillIcon03;

    private bool WarningEffectBlinking = false;

    public RectTransform enemyIndicatorPrefab; // ?? ?????? ????? UI ??????
    private Dictionary<Transform, RectTransform> enemyIndicators = new Dictionary<Transform, RectTransform>();

    // Vigette Effect
    private Volume volume;
    private Vignette vignette;
    private float initialIntensity;
    private Color initialColor;

    private float targetIntensity = 2f;
    private float intensityChangeSpeed = 1f;

    private bool shutdown = false;

    [SerializeField] private Cost _playerCost;
    [SerializeField] private Dash _dash;
    [SerializeField] private Barrier _barrier;

    private void Awake()
    {
        instance = this;

        gamePlayUICanvas.SetActive(false);
        gameOverUICanvas.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        restartButton.onClick.AddListener(OnRestartButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);

        tryAgainYesButton.onClick.AddListener(OnTryAgainYesButtonClick);
        tryAgainNoButton.onClick.AddListener(OnTryAgainNoButtonClick);

        volume = FindObjectOfType<Volume>();
        if (volume != null)
        {
            volume.profile.TryGet<Vignette>(out vignette);

            if (vignette != null)
            {
                initialIntensity = vignette.intensity.value;
                initialColor = vignette.color.value;
            }
        }
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        // Debug.Log(vignette.intensity.value);

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

        if (_playerCost.Value >= _dash.Cost)
        {
            Skill01On();
        }
        else
        {
            Skill01Off();
        }

        if (_playerCost.Value >= _barrier.Cost)
        {
            Skill02On();
        }
        else
        {
            Skill02Off();
        }

        // ??? ???? ???????
        distanceMiddle = (int)GameManager.distanceFromMiddle;
        if (distanceMiddle >= shutdownArea)
        {
            gamePlayUICanvas.SetActive(false);
            distanceMiddleText.text = "? M"; // 100 ????? ???? "? M"
            distanceMiddleText.color = Color.white; // ?? ???? (??: ???)

            shutdown = true;

            if (WarningEffectBlinking)
            {
                StopBlinkWarningEffect();
            }
            warningEffect.gameObject.SetActive(false);
            /*ResetVignetteEffect();*/
            vignette.color.Override(Color.black);
            float currentIntensity = vignette.intensity.value;
            // ?????? ??? ?????? ???? ????
            if (currentIntensity < targetIntensity)
            {
                vignette.intensity.Override(Mathf.MoveTowards(currentIntensity, targetIntensity, intensityChangeSpeed * Time.deltaTime));
            }
        }
        else if (distanceMiddle >= warningArea)
        {
            gamePlayUICanvas.SetActive(true);
            distance.color = Color.red;
            distanceMiddleText.text = distanceMiddle.ToString() + " M"; // ??? ???
            distanceMiddleText.color = Color.red; // ?????????? ???

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
            distance.color = Color.white;
            distanceMiddleText.text = distanceMiddle.ToString() + " M"; // ??? ???
            distanceMiddleText.color = Color.white; // ?? ???? (??: ???)

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
        /*GameManager.instance.RestartGame();*/
    }

    private void OnQuitButtonClick()
    {
        /*Application.Quit();*/
    }

    private void OnTryAgainYesButtonClick()
    {
        /*GameManager.instance.TryAgain();*/
    }

    private void OnTryAgainNoButtonClick()
    {
        /*Application.Quit();*/
    }

    // public void ShowCountdown(int count)
    // {
    //     countdownText.gameObject.SetActive(true);
    //     countdownText.text = count.ToString();
    // }

    public void ShowGamePlayUI()
    {
        // countdownText.gameObject.SetActive(false);
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        Debug.Log("ShowPauseMenu ????");
        pauseMenuUICanvas.SetActive(true);  // PauseMenu UI ????
        Debug.Log("pauseMenuUICanvas ???? (???? ??? ??): " + pauseMenuUICanvas.activeSelf);
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
        playTimeResult.text = "Play Time :  " + (GlobalTimer.InitValue - GlobalTimer.Value).ToString("F1");
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
        InvokeRepeating("BlinkWarningEffect", 0f, 0.5f); // 0.5????? ??? ??? ??????
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
        InvokeRepeating("BlinkSkillIcon", 0f, 0.1f); // 0.1????? ??? ?????? ??????? ???
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

    public void Skill02On()
    {
        skillIcon02.color = new Color(skillIcon02.color.r, skillIcon02.color.g, skillIcon02.color.b, 1f);
    }

    public void Skill02Off()
    {
        skillIcon02.color = new Color(skillIcon02.color.r, skillIcon02.color.g, skillIcon02.color.b, 5f / 255f);
    }

    public void UpdateEnemyIndicator(Transform enemyTransform)
    {
        if (!enemyIndicators.ContainsKey(enemyTransform))
        {
            // Create a new indicator UI under the canvas
            RectTransform indicator = Instantiate(enemyIndicatorPrefab, gamePlayUICanvas.transform);
            enemyIndicators[enemyTransform] = indicator;
        }

        RectTransform indicatorUI = enemyIndicators[enemyTransform];

        // Calculate the distance to the enemy (if needed elsewhere)
        float distance = Vector3.Distance(Camera.main.transform.position, enemyTransform.position);

        // Convert the enemy's world position to viewport coordinates
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemyTransform.position);

        // If the enemy is behind the camera, flip the point
        if (screenPoint.z < 0)
        {
            screenPoint.x = 1f - screenPoint.x;
            screenPoint.y = 1f - screenPoint.y;
            screenPoint.z = 0;
        }

        // Clamp the screen point to the viewport bounds
        screenPoint.x = Mathf.Clamp(screenPoint.x, 0f, 1f);
        screenPoint.y = Mathf.Clamp(screenPoint.y, 0f, 1f);

        // Calculate direction from the center of the screen to the enemy
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Vector2 direction = new Vector2(screenPoint.x - screenCenter.x, screenPoint.y - screenCenter.y);
        direction.Normalize();

        // Get the canvas dimensions
        RectTransform canvasRect = gamePlayUICanvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Calculate the position within the canvas
        float edgeBuffer = 50f; // Buffer from the edge of the canvas
        float x = direction.x * (canvasWidth / 2 - edgeBuffer);
        float y = direction.y * (canvasHeight / 2 - edgeBuffer);

        indicatorUI.anchoredPosition = new Vector2(x, y);

        // Set the size of the indicator UI
        float size = 200f;
        indicatorUI.sizeDelta = new Vector2(size, size);

        // Activate the indicator UI
        indicatorUI.gameObject.SetActive(true);

        // Animate the indicator's alpha
        float alpha = Mathf.PingPong(Time.time * 2, 1); // Value oscillates between 0 and 1
        indicatorUI.GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
    }


    public void RemoveEnemyIndicator(Transform enemyTransform)
    {
        if (enemyIndicators.ContainsKey(enemyTransform))
        {
            // ???? ??? ????? ?��??????? ????
            Destroy(enemyIndicators[enemyTransform].gameObject);
            enemyIndicators.Remove(enemyTransform);
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
            vignette.intensity.Override(0.8f);
        }
    }

    private void ResetVignetteEffect()
    {
        if (vignette != null)
        {
            vignette.color.Override(initialColor);
            vignette.intensity.Override(initialIntensity);
        }
    }
}
