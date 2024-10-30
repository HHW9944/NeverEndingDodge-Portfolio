using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool isPaused;
    public static bool isGameOver;

    public Transform player;
    public Transform middlePoint;
    public Transform playerCamera; // 플레이어 카메라 추가

    public GameObject[] enemies;

    public float moveSpeed = 5f;
    public static float distanceFromMiddle = 0f;

    public static float timer = 180f; // 기존 timer 변수 유지
    public bool isTimeOut = false;

    public Vector3 playerStartPoint;
    public Vector3 cameraStartPoint;
    public Quaternion startRotation; // 초기 회전값 추가

    // Life 객체 참조
    public Life playerLife;

    private bool isCountingDown = false;
    private float countdownTimer = 0f;
    private int countdownValue = 3;

    private Coroutine timerCoroutine; // 타이머 코루틴을 위한 변수 추가

    private void Awake()
    {
        instance = this;
        playerStartPoint = new Vector3(0, 0, 0);
        cameraStartPoint = playerCamera.position;
        startRotation = Quaternion.identity; // 초기 회전값 설정
    }

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        timer = 180f; // 타이머 초기화
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UIManager.instance.ShowCountdown(3);
        StartCountdown();

        // 타이머 카운트다운 시작
        StartTimerCountdown();
    }

    void Update()
    {
        // 게임 오버 상태에서는 게임 진행을 정지 (애니메이션 제외)
        if (isGameOver) return;

        // 카운트다운 중일 때
        if (isCountingDown)
        {
            HandleCountdown();
            return;
        }

        HandlePause();
        HandleGameOver();
        HandleSkills();

        if (isPaused) return;

        UIManager.instance.UpdateTimerUI((int)timer / 60, (int)timer % 60); // 타이머 UI 업데이트
        distanceFromMiddle = Vector3.Distance(player.position, middlePoint.position);

        Debug.Log(distanceFromMiddle + "m\n");

        /*if (distanceFromMiddle >= 100)
        {
            Debug.Log("100m 초과\n");
            UIManager.instance.StopBlinkWarningEffect();
        }
        else if (distanceFromMiddle >= 90)
        {
            Debug.Log("경고 이펙트 on");
            UIManager.instance.StartBlinkWarningEffect();
        }
        else
        {
            UIManager.instance.StopBlinkWarningEffect();
        }*/

        UpdateEnemyIndicators();
    }

    // 타이머 카운트다운 시작 함수
    void StartTimerCountdown()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine); // 기존 타이머 코루틴 중지
        }
        timerCoroutine = StartCoroutine(TimerCountDown()); // 새로운 타이머 카운트다운 시작
    }

    IEnumerator TimerCountDown()
    {
        while (timer > 0 && !isGameOver)
        {
            timer--;
            int minute = (int)timer / 60;
            int second = (int)timer % 60;
            UIManager.instance.UpdateTimerUI(minute, second); // UI 업데이트
            yield return new WaitForSeconds(1f);
        }

        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("타임아웃. 생존하셨습니다.");
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        UIManager.instance.ShowPauseMenu();
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
        /*if (UIManager.instance.warningEffect.gameObject.activeSelf)
        {
            UIManager.instance.StartBlinkWarningEffect();
        }*/
    }

    // 수정된 GameOver 메서드
    public void GameOver()
    {
        isGameOver = true;

        // 모든 UI 숨기기
        UIManager.instance.HideAllUI();

        // 1초 동안 대기 후 GameOver 처리
        StartCoroutine(GameOverTransition());
    }

    // GameOver 화면 전환 코루틴
    private IEnumerator GameOverTransition()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(1f);

        // GameOverUI 표시
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        UIManager.instance.ShowGameOverUI();
    }

    public void TryAgain()
    {
        isGameOver = false;
        InitializePlayer(); // 플레이어 초기화
        // playerLife.ResetLife(); // Life 초기화
        timer = 180f;
        UIManager.instance.HideGameOverUI(); // 게임 오버 UI 숨김
        StartCountdown(); // 카운트다운 시작

        // 타이머 카운트다운 재시작
        StartTimerCountdown();
    }

    public void RestartGame()
    {
        /*isGameOver = false;
        isPaused = false;

        InitializePlayer(); // 플레이어 초기화
        // playerLife.ResetLife(); // Life 초기화

        // 타이머 초기화 및 재시작
        timer = 2000f;
        StartTimerCountdown();

        StartCountdown(); // 카운트다운 시작*/

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        player.position = playerStartPoint; // 위치 초기화
        player.rotation = startRotation; // 플레이어 회전 초기화
        playerCamera.position = cameraStartPoint;
        playerCamera.localRotation = Quaternion.identity; // 카메라 회전 초기화 (첫 시점으로 돌아가도록)
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

    void UpdateEnemyIndicators()
    {
        if (enemies == null || enemies.Length == 0)
            return;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                Vector3 screenPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);

                Debug.Log($"Enemy Position: {enemy.transform.position}, Screen Point: {screenPoint}");

                bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (!isVisible)
                {
                    // 적이 화면에 보이지 않으면 UI 업데이트
                    UIManager.instance.UpdateEnemyIndicator(enemy.transform);
                }
                else
                {
                    // 적이 화면에 보일 경우 UI에서 제거
                    UIManager.instance.RemoveEnemyIndicator(enemy.transform);
                }
            }
        }
    }
}

