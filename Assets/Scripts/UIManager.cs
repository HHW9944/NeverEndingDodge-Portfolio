using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject pauseMenuUICanvas;
    public GameObject gamePlayUICanvas;
    public GameObject gameOverUICanvas;

    public int distanceMiddle;
    public TextMeshProUGUI distanceMiddleText;

    public Image warningEffect;
    public Color warningEffectColor;
    public TextMeshProUGUI warningText;
    public Color warningTextColor;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    private void Awake()
    {
        instance = this;
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
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
            pauseMenuUICanvas.SetActive(true);
            gamePlayUICanvas.SetActive(false);
            gameOverUICanvas.SetActive(false);
            return;
        }
        else
        {
            pauseMenuUICanvas.SetActive(false);
            gamePlayUICanvas.SetActive(true);
            gameOverUICanvas.SetActive(false);
        }

        timerText.text = GameManager.timer.ToString("F2");

        distanceMiddle = (int)GameManager.distanceFromMiddle;
        distanceMiddleText.text = distanceMiddle.ToString() + " M";
        if (distanceMiddle > 90)
        {
            warningEffect.gameObject.SetActive(true);
            warningText.gameObject.SetActive(true);

        }
        else
        {
            warningEffect.gameObject.SetActive(false);
            warningText.gameObject.SetActive(false);
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenuUICanvas.SetActive(true);
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
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(false);
        gameOverUICanvas.SetActive(true);
    }
    public void HideGameOverUI()
    {
        pauseMenuUICanvas.SetActive(false);
        gamePlayUICanvas.SetActive(true);
        gameOverUICanvas.SetActive(false);
    }
}
