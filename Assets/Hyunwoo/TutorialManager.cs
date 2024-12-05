using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public Transform player;

    public bool isInputActive = false;

    public static bool keyboardWPressed = false;
    public static bool keyboardAPressed = false;
    public static bool keyboardSPressed = false;
    public static bool keyboardDPressed = false;
    public static bool keyboardSpacePressed = false;

    private float keyWPressStartTime = -1f; // W Ű�� ������ ������ �ð�
    private float keyAPressStartTime = -1f;
    private float keySPressStartTime = -1f;
    private float keyDPressStartTime = -1f;
    private float pressDurationThreshold = 0.5f; // 0.5�� �̻� ������ ��

    private float spacePressStartTime = -1f; // Space Ű�� ������ ������ �ð�
    private float spacePressDurationThreshold = 1f; // 1�� �̻� ������ ��
    public static float spacePressProgress = 0f;

    public static bool tutorial1 = false;
    public static bool tutorial2 = false;
    public static bool tutorial3 = false;

    public static bool tutorialQuest1 = false; //Quest 1 ~ 4 : WASD Movement Quest in Tutorial1
    public static bool tutorialQuest2 = false;
    public static bool tutorialQuest3 = false;
    public static bool tutorialQuest4 = false;

    private InputAction moveAction;

    public GameObject tutorialEnemy1;
    public GameObject shootPointLeft;
    public GameObject enemyLocation1;
    public GameObject enemyLocation2;

    public Transform shootPoint;
    private Shooter shootScript;

    private int barrierCount = 0;
    [SerializeField] private Timer timer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        shootPoint = tutorialEnemy1.transform.Find("ShootPoint(left)");
        if (shootPoint == null)
        {
            Debug.LogError("shooterpoint not found in tutorialEnemy1");
        }

        shootScript = shootPoint.GetComponent<Shooter>();
        if (shootScript == null)
        {
            Debug.LogError("shootScipt not found in shootPoint");
        }

        shootScript.enabled = false;
        barrierCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorial1)
        {
            if(tutorialQuest1 && tutorialQuest2 && tutorialQuest3 && tutorialQuest4)
            {
                tutorial1 = false;
                timer.Resume();
            }

            keyboardWPressed = Input.GetKey(KeyCode.W);
            keyboardAPressed = Input.GetKey(KeyCode.A);
            keyboardSPressed = Input.GetKey(KeyCode.S);
            keyboardDPressed = Input.GetKey(KeyCode.D);

            if (keyboardWPressed)
            {
                if (keyboardSPressed) return;

                if (keyWPressStartTime < 0f) // ������ ������ �ð��� ����
                {
                    keyWPressStartTime = Time.time;
                }
                else if (Time.time - keyWPressStartTime >= pressDurationThreshold) // 0.5�� �̻� ���ȴ��� Ȯ��
                {
                    tutorialQuest1 = true;
                }
            }
            else
            {
                keyWPressStartTime = -1f; // Ű�� ���� �ʱ�ȭ
            }

            // A Ű �Է� ó��
            if (Input.GetKey(KeyCode.A))
            {
                if (keyboardDPressed) return;

                if (keyAPressStartTime < 0f)
                {
                    keyAPressStartTime = Time.time;
                }
                else if (Time.time - keyAPressStartTime >= pressDurationThreshold)
                {
                    tutorialQuest2 = true;
                }
            }
            else
            {
                keyAPressStartTime = -1f;
            }

            // S Ű �Է� ó��
            if (Input.GetKey(KeyCode.S))
            {
                if (keyboardWPressed) return;

                if (keySPressStartTime < 0f)
                {
                    keySPressStartTime = Time.time;
                }
                else if (Time.time - keySPressStartTime >= pressDurationThreshold)
                {
                    tutorialQuest3 = true;
                }
            }
            else
            {
                keySPressStartTime = -1f;
            }

            // D Ű �Է� ó��
            if (Input.GetKey(KeyCode.D))
            {
                if (keyboardAPressed) return;

                if (keyDPressStartTime < 0f)
                {
                    keyDPressStartTime = Time.time;
                }
                else if (Time.time - keyDPressStartTime >= pressDurationThreshold)
                {
                    tutorialQuest4 = true;
                }
            }
            else
            {
                keyDPressStartTime = -1f;
            }
        }
        else if (tutorial2)
        {
            keyboardSpacePressed = Input.GetKey(KeyCode.Space);

            // Space Ű �Է� ó��
            if (keyboardSpacePressed)
            {
                if (spacePressStartTime < 0f) // Space ������ ������ �ð��� ����
                {
                    spacePressStartTime = Time.time;
                }
                else
                {
                    // ���� ������ �ִ� �ð��� �������� �����̴� �� ���
                    float elapsedTime = Time.time - spacePressStartTime;
                    spacePressProgress = Mathf.Clamp01(elapsedTime / spacePressDurationThreshold); // 0~1 ���̷� ����

                    if (elapsedTime >= spacePressDurationThreshold) // 1�� �̻� ���ȴ��� Ȯ��
                    {
                        Debug.Log("Space �Է� �Ϸ�");

                        tutorial2 = false;
                        timer.Resume();

                        spacePressStartTime = -1f; // �ʱ�ȭ
                    }
                }
            }
            else
            {
                spacePressStartTime = -1f; // Space�� ���� �ʱ�ȭ
                spacePressProgress = 0f;
            }
        }
    }

    public void getTutorial1()
    {
        Debug.Log("Tutorial1");
        tutorial1 = true;
    }

    public void getTutorial2()
    {
        Debug.Log("Tutorial2");
        tutorial2 = true;
    }

    public void getTutorial3()
    {
        Debug.Log("Tutorial3");
        tutorial3 = true;
    }

    public void clearTutorial3()
    {
        //�踮��� ��� ������ ���
        tutorial3 = false;
        timer.Resume();
        
    }
    public void enemyAppear()
    {
        tutorialEnemy1.SetActive(true);
        StartCoroutine(MoveObjectWithDelay(tutorialEnemy1, enemyLocation1 ,1f));
    }

    public void enemyDisapper()
    {
        tutorialEnemy1.SetActive(true);
        StartCoroutine(MoveObjectWithDelay(tutorialEnemy1, enemyLocation2, 1f));
    }
    public void enemyShootActive()
    {
        shootScript.enabled = true;
    }

    public void enemyShootDeactive()
    {
        shootScript.enabled = false;
        shootScript.CancelInvoke();
    }
    private IEnumerator MoveObjectWithDelay(GameObject movingObject, GameObject targetLocation, float delay)
    {
        Debug.Log("���ʹ� �̵�");
        // ������ ����
        yield return new WaitForSeconds(delay);

        // null üũ
        if (movingObject != null && targetLocation != null)
        {
            // �ε巴�� �̵�
            float elapsedTime = 0f;
            float moveDuration = 1f; // �̵� �ð�
            Vector3 startPos = movingObject.transform.position;

            while (elapsedTime < moveDuration)
            {
                movingObject.transform.position = Vector3.Lerp(startPos, targetLocation.transform.position, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            movingObject.transform.position = targetLocation.transform.position;
        }
        else
        {
            Debug.LogError("movingObject or targetLocation is not assigned!");
        }
    }

}
