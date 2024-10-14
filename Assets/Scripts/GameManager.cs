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
    public Transform playerCamera; // �÷��̾� ī�޶� �߰�

    public float moveSpeed = 5f;
    public static float distanceFromMiddle = 0f;

    public static float timer = 0f;

    public Vector3 startPoint;
    public Quaternion startRotation; // �ʱ� ȸ���� �߰�

    private bool isCountingDown = false;
    private float countdownTimer = 0f;
    private int countdownValue = 3;

    private void Awake()
    {
        instance = this;

        startPoint = new Vector3(0, 0, 0);
        startRotation = Quaternion.identity; // �ʱ� ȸ���� ����
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
        // ī��Ʈ�ٿ� ���� ��
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
        Debug.Log("PauseGame ȣ��� - pauseMenuUICanvas ����: " + UIManager.instance.pauseMenuUICanvas.activeSelf);
        UIManager.instance.ShowPauseMenu();
        Debug.Log("PauseMenu ǥ�õ� - pauseMenuUICanvas ����: " + UIManager.instance.pauseMenuUICanvas.activeSelf);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // WarningEffect�� Ȱ��ȭ�Ǿ� ������ �Ͻ� ����
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

        // ��� ȿ���� �ٽ� �ʿ����� Ȯ���ϰ� �簳
        if (UIManager.instance.warningEffect.gameObject.activeSelf)
        {
            UIManager.instance.StartBlinkWarningEffect();
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // ���� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.ShowGameOverUI(); // ���� ���� UI ǥ��
    }

    public void TryAgain()
    {
        isGameOver = false;
        InitializePlayer(); // �÷��̾� �ʱ�ȭ
        UIManager.instance.HideGameOverUI(); // ���� ���� UI ����
        StartCountdown(); // ī��Ʈ�ٿ� ����
    }

    public void RestartGame()
    {
        isGameOver = false;
        isPaused = false;

        InitializePlayer(); // �÷��̾� �ʱ�ȭ
        StartCountdown(); // ī��Ʈ�ٿ� ����
    }

    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // ���� �ð� �簳
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.instance.ShowGamePlayUI(); // ���� UI ǥ��
    }

    // �÷��̾� �ʱ�ȭ �Լ�
    void InitializePlayer()
    {
        player.position = startPoint; // ��ġ �ʱ�ȭ
        player.rotation = startRotation; // �÷��̾� ȸ�� �ʱ�ȭ
        playerCamera.localRotation = Quaternion.identity; // ī�޶� ȸ�� �ʱ�ȭ (ù �������� ���ư�����)
        timer = 0f; // Ÿ�̸� �ʱ�ȭ
    }

    // ī��Ʈ�ٿ� ����
    private void StartCountdown()
    {
        isCountingDown = true;
        countdownTimer = 0f;
        countdownValue = 3;
        Time.timeScale = 0f; // Ÿ�ӽ������� 0���� �����Ͽ� ���� ���¸� ����
        UIManager.instance.ShowCountdown(countdownValue); // ī��Ʈ�ٿ� UI ������Ʈ
    }

    // ī��Ʈ�ٿ� ó��
    private void HandleCountdown()
    {
        countdownTimer += Time.unscaledDeltaTime; // Time.timeScale�� 0�̾ �帣�� �ð�
        if (countdownTimer >= 1f)
        {
            countdownTimer = 0f;
            countdownValue--;
            if (countdownValue > 0)
            {
                UIManager.instance.ShowCountdown(countdownValue); // ī��Ʈ�ٿ� UI ������Ʈ
            }
            else
            {
                isCountingDown = false;
                StartGame(); // ī��Ʈ�ٿ� ������ ���� ����
            }
        }
    }
}