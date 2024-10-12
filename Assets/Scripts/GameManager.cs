using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isPaused;
    public static bool isGameOver;

    public Transform player;
    public Transform middlePoint;
    public Transform cylinder;
    public Transform sphere;

    public float moveSpeed = 5f;
    public static float distanceFromMiddle = 0f;

    public static float timer = 0f;

    private Rigidbody rb;

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        timer = 0f;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandlePause();
        HandleGameOver();

        if (isPaused || isGameOver) return;

        timer += Time.deltaTime;

        Vector3 playerPosition = player.position;
        float playerX = playerPosition.x;
        float playerY = playerPosition.y;
        float playerZ = playerPosition.z;

        Vector3 targetPosition = middlePoint.position;
        float middleX = targetPosition.x;
        float middleY = targetPosition.y;
        float middleZ = targetPosition.z;

        distanceFromMiddle = Vector3.Distance(player.position, middlePoint.position);
        MovePlayer();
        PositionCylinder();
    }

    void MovePlayer()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        float moveUpDown = 0f;

        // A�� D Ű �Է� ó��
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
        }

        // W�� S Ű �Է� ó��
        if (Input.GetKey(KeyCode.W))
        {
            moveVertical = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -1f;
        }

        // Q�� E Ű �Է� ó��
        if (Input.GetKey(KeyCode.Q))
        {
            moveUpDown = -1f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveUpDown = 1f;
        }

        // �̵� ���� ����
        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveVertical);
        rb.MovePosition(transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void PositionCylinder()
    {
        if (distanceFromMiddle < 10f || distanceFromMiddle > 100f)
        {
            cylinder.gameObject.SetActive(false);
        }
        else
        {
            cylinder.gameObject.SetActive(true);
        }


        if (sphere == null || cylinder == null) return;

        Vector3 directionToMiddlePoint = (middlePoint.position - sphere.position).normalized;

        SphereCollider sphereCollider = sphere.GetComponent<SphereCollider>();
        float sphereRadius = sphereCollider != null ? sphereCollider.radius * sphere.lossyScale.x : 1f;

        Vector3 cylinderPosition = sphere.position + directionToMiddlePoint * sphereRadius;

        cylinder.position = cylinderPosition;

        Quaternion lookAtMiddleRotation = Quaternion.LookRotation(directionToMiddlePoint);

        cylinder.rotation = lookAtMiddleRotation * Quaternion.Euler(90, 0, 0);
    }

    void HandlePause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void HandleGameOver()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isGameOver)
            {
                RestartGame();
            }
            else
            {
                GameOver();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // ���� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.ShowPauseMenu(); // ���� �޴� ǥ��
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // ���� �簳
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.instance.HidePauseMenu(); // ���� �޴� ����
    }
    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f; // ���� ����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.instance.ShowGameOverUI(); // ���� ���� UI ǥ��
    }
    public void RestartGame()
    {
        isGameOver = false;
        timer = 0f;
        Time.timeScale = 1f; // ���� �簳
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.instance.HideGameOverUI(); // ���� ���� UI ����
        player.position = Vector3.zero; // �÷��̾� ��ġ �ʱ�ȭ (����)
    }
}
