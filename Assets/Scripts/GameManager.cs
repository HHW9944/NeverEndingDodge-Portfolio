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
    public Transform playerCamera; // �÷��̾� ī�޶� �߰�

    public GameObject[] enemies;

    public float moveSpeed = 5f;
    public static float distanceFromMiddle = 0f;
    public float warningArea = 99999f;
    public float shutdownArea = 99999f;
    public bool isTimeOut = false;

    public Vector3 playerStartPoint;
    public Vector3 cameraStartPoint;
    public Quaternion startRotation; // �ʱ� ȸ���� �߰�

    // Life ��ü ����
    public Life playerLife;

    private bool isCountingDown = false;
    // private float countdownTimer = 0f;
    // private int countdownValue = 3;

    private Coroutine timerCoroutine; // Ÿ�̸� �ڷ�ƾ�� ���� ���� �߰�

    [SerializeField] private Cost _playerCost;
    [SerializeField] private Speed _playerSpeed;
    private void Awake()
    {
        Debug.Log(_playerCost.Value);
        instance = this;
        playerStartPoint = new Vector3(0, 0, 0);
        cameraStartPoint = playerCamera.position;
        startRotation = Quaternion.identity; // �ʱ� ȸ���� ����
    }

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        // UIManager.instance.ShowCountdown(3);
        // StartCountdown();
        StartGame();

        // Ÿ�̸� ī��Ʈ�ٿ� ����
    }

    void Update()
    {
        // ���� ���� ���¿����� ���� ������ ���� (�ִϸ��̼� ����)
        if (isGameOver) return;

        // ī��Ʈ�ٿ� ���� ��
        if (isCountingDown)
        {
            // HandleCountdown();
            return;
        }

        HandlePause();
        HandleGameOver();
        HandleSkills();

        if (isPaused) return;

        distanceFromMiddle = Vector3.Distance(player.position, middlePoint.position);

        // if (distanceFromMiddle < shutdownArea)
        // {
        //     _playerSpeed.Value = 10f; // plz don't hard coding
        // }else
        // {
        //     _playerSpeed.Value = 1f;
        // }
        // Debug.Log(distanceFromMiddle + "m\n");

        /*if (distanceFromMiddle >= 100)
        {
            Debug.Log("100m �ʰ�\n");
            UIManager.instance.StopBlinkWarningEffect();
        }
        else if (distanceFromMiddle >= 90)
        {
            Debug.Log("��� ����Ʈ on");
            UIManager.instance.StartBlinkWarningEffect();
        }
        else
        {
            UIManager.instance.StopBlinkWarningEffect();
        }*/

        UpdateEnemyIndicators();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        isPaused = true;
        UIManager.instance.ShowPauseMenu();
        Time.timeScale = 0f;
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;

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
        // StartCountdown();
        StartGame();

        // ��� ȿ���� �ٽ� �ʿ����� Ȯ���ϰ� �簳
        /*if (UIManager.instance.warningEffect.gameObject.activeSelf)
        {
            UIManager.instance.StartBlinkWarningEffect();
        }*/
    }

    // ������ GameOver �޼���
    public void GameOver()
    {
        isGameOver = true;

        // ��� UI �����
        UIManager.instance.HideAllUI();

        // 1�� ���� ��� �� GameOver ó��
        StartCoroutine(GameOverTransition());
    }

    // GameOver ȭ�� ��ȯ �ڷ�ƾ
    private IEnumerator GameOverTransition()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.6f);

        // GameOverUI ǥ��
        Time.timeScale = 1f;
        // Cursor.visible = true;
        // Cursor.lockState = CursorLockMode.None;
        // UIManager.instance.ShowGameOverUI();
    }

    public void TryAgain()
    {
        isGameOver = false;
        InitializePlayer(); // �÷��̾� �ʱ�ȭ
        UIManager.instance.HideGameOverUI(); // ���� ���� UI ����
        // StartCountdown(); // ī��Ʈ�ٿ� ����
        StartGame();
    }

    public void RestartGame()
    {
        /*isGameOver = false;
        isPaused = false;

        InitializePlayer(); // �÷��̾� �ʱ�ȭ
        // playerLife.ResetLife(); // Life �ʱ�ȭ

        // Ÿ�̸� �ʱ�ȭ �� �����
        timer = 2000f;
        StartTimerCountdown();

        StartCountdown(); // ī��Ʈ�ٿ� ����*/
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // ���� �ð� �簳
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        UIManager.instance.ShowGamePlayUI(); // ���� UI ǥ��
    }

    // �÷��̾� �ʱ�ȭ �Լ�
    void InitializePlayer()
    {
        player.position = playerStartPoint; // ��ġ �ʱ�ȭ
        player.rotation = startRotation; // �÷��̾� ȸ�� �ʱ�ȭ
        playerCamera.position = cameraStartPoint;
        playerCamera.localRotation = Quaternion.identity; // ī�޶� ȸ�� �ʱ�ȭ (ù �������� ���ư�����)
    }

    // ī��Ʈ�ٿ� ����
    // private void StartCountdown()
    // {
    //     isCountingDown = true;
    //     countdownTimer = 0f;
    //     countdownValue = 3;
    //     Time.timeScale = 0f; // Ÿ�ӽ������� 0���� �����Ͽ� ���� ���¸� ����
    //     UIManager.instance.ShowCountdown(countdownValue); // ī��Ʈ�ٿ� UI ������Ʈ
    // }

    // // ī��Ʈ�ٿ� ó��
    // private void HandleCountdown()
    // {
    //     countdownTimer += Time.unscaledDeltaTime; // Time.timeScale�� 0�̾ �帣�� �ð�
    //     if (countdownTimer >= 1f)
    //     {
    //         countdownTimer = 0f;
    //         countdownValue--;
    //         if (countdownValue > 0)
    //         {
    //             UIManager.instance.ShowCountdown(countdownValue); // ī��Ʈ�ٿ� UI ������Ʈ
    //         }
    //         else
    //         {
    //             isCountingDown = false;
    //             StartGame(); // ī��Ʈ�ٿ� ������ ���� ����
    //         }
    //     }
    // }

    void HandleSkills()
    {
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            Skill01On();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Skill01Off();
        }*/
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
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            /*if (isGameOver) return;

            if (!isPaused)
            {
                PauseGame();
            }*/
        }
    }

    void HandleGameOver()
    {
        /*if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isGameOver)
            {
                GameOver();
            }
        }*/
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

                /*Debug.Log($"Enemy Position: {enemy.transform.position}, Screen Point: {screenPoint}");*/

                bool isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

                if (!isVisible)
                {
                    // ���� ȭ�鿡 ������ ������ UI ������Ʈ
                    UIManager.instance.UpdateEnemyIndicator(enemy.transform);
                }
                else
                {
                    // ���� ȭ�鿡 ���� ��� UI���� ����
                    UIManager.instance.RemoveEnemyIndicator(enemy.transform);
                }
            }
        }
    }
}

