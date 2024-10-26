using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool isPaused;
    public static bool isGameOver;

    public Transform player;
    public Transform middlePoint;
    public Transform playerCamera; // 플레이어 카메라 추가

    public float moveSpeed = 5f;
    public static float distanceFromMiddle = 0f;

    public static float timer = 0f;

    public Vector3 startPoint;
    public Quaternion startRotation; // 초기 회전값 추가

    private bool isCountingDown = false;
    private float countdownTimer = 0f;
    private int countdownValue = 3;

    private void Awake()
    {
        instance = this;

        startPoint = new Vector3(0, 0, 0);
        startRotation = Quaternion.identity; // 초기 회전값 설정
    }

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        timer = 0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 카운트다운 중일 때
        if (isCountingDown)
        {
            HandleCountdown();
            return;
        }

        HandlePause();
        HandleGameOver();
        HandleSkills();

        if (isPaused || isGameOver) return;

        timer += Time.deltaTime;
        distanceFromMiddle = Vector3.Distance(player.position, middlePoint.position);
    }

    void HandleSkills()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Skill01On();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Skill01Off();
        }
    }

    void Skill01On()
    {
        UIManager.instance.StartSkill01Effect();
    }

    void Skill01Off()
    {
        UIManager.instance.StopSkill01Effect();
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGameOver) return;

            if (!isPaused)
            {
                PauseGame();
            }
        }
    }

    void HandleGameOver()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGameOver)
            {
                GameOver();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Debug.Log("PauseGame 호출됨 - pauseMenuUICanvas 상태: " + UIManager.instance.pauseMenuUICanvas.activeSelf);
        UIManager.instance.ShowPauseMenu();
        Debug.Log("PauseMenu 표시됨 - pauseMenuUICanvas 상태: " + UIManager.instance.pauseMenuUICanvas.activeSelf);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // WarningEffect가 활성화되어 있으면 일시 중지
        if (UIManager.instance.warningEffect.gameObject.activeSelf)
        {
            UIManager.instance.StopBlinkWarningEffect();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        UIManager.instance.HidePauseMenu();
        StartCountdown();

        // 경고 효과가 다시 필요한지 확인하고 재개
        if (UIManager.instance.warningEffect.gameObject.activeSelf)
        {
            UIManager.instance.StartBlinkWarningEffect();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // 게임 정지
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.ShowGameOverUI(); // 게임 오버 UI 표시
    }

    public void TryAgain()
    {
        isGameOver = false;
        InitializePlayer(); // 플레이어 초기화
        UIManager.instance.HideGameOverUI(); // 게임 오버 UI 숨김
        StartCountdown(); // 카운트다운 시작
    }

    public void RestartGame()
    {
        isGameOver = false;
        isPaused = false;

        InitializePlayer(); // 플레이어 초기화
        StartCountdown(); // 카운트다운 시작
    }

    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // 게임 시간 재개
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.instance.ShowGamePlayUI(); // 게임 UI 표시
    }

    // 플레이어 초기화 함수
    void InitializePlayer()
    {
        player.position = startPoint; // 위치 초기화
        player.rotation = startRotation; // 플레이어 회전 초기화
        playerCamera.localRotation = Quaternion.identity; // 카메라 회전 초기화 (첫 시점으로 돌아가도록)
        timer = 0f; // 타이머 초기화
    }

    // 카운트다운 시작
    private void StartCountdown()
    {
        isCountingDown = true;
        countdownTimer = 0f;
        countdownValue = 3;
        Time.timeScale = 0f; // 타임스케일을 0으로 설정하여 정지 상태를 유지
        UIManager.instance.ShowCountdown(countdownValue); // 카운트다운 UI 업데이트
    }

    // 카운트다운 처리
    private void HandleCountdown()
    {
        countdownTimer += Time.unscaledDeltaTime; // Time.timeScale이 0이어도 흐르는 시간
        if (countdownTimer >= 1f)
        {
            countdownTimer = 0f;
            countdownValue--;
            if (countdownValue > 0)
            {
                UIManager.instance.ShowCountdown(countdownValue); // 카운트다운 UI 업데이트
            }
            else
            {
                isCountingDown = false;
                StartGame(); // 카운트다운 끝나면 게임 시작
            }
        }
    }
}