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
    public static bool isPaused = false;

    [SerializeField]
    private RectTransform joystickHandle; // Joystick의 Handle RectTransform 참조

    [SerializeField]
    private RectTransform joystickBackground; // Joystick의 Background RectTransform 참조

    [SerializeField]
    private float joystickThreshold = 0.8f; // 입력 임계값
    Vector2 joystickInput;

    public static bool keyboardWPressed = false;
    public static bool keyboardAPressed = false;
    public static bool keyboardSPressed = false;
    public static bool keyboardDPressed = false;
    public static bool keyboardQPressed = false;
    public static bool keyboardSpacePressed = false;
    public static bool keyboardShiftPressed = false;
    
    private float keyWPressStartTime = -1f; // W 키가 눌리기 시작한 시간
    private float keyAPressStartTime = -1f;
    private float keySPressStartTime = -1f;
    private float keyDPressStartTime = -1f;
    private float pressDurationThreshold = 0.5f; // 0.5초 이상 눌러야 함

    private float joystickVertical;
    private float joystickHorizontal;

    private float spacePressStartTime = -1f; // Space 키가 눌리기 시작한 시간
    private float spacePressDurationThreshold = 1f; // 1초 이상 눌러야 함
    public static float spacePressProgress = 0f;

    public static bool step1 = false;
    public static bool step2 = false;
    public static bool step3 = false;
    public static bool step4 = false;
    public static bool step5 = false;
    public static bool step6 = false;

    public static bool step1Started = false;
    public static bool step3Started = false;
    public static bool step5Started = false;
    public static bool step6Started = false;

    public static bool tutorialQuest1 = false; //Quest 1 ~ 4 : WASD Movement Quest in step1
    public static bool tutorialQuest2 = false;
    public static bool tutorialQuest3 = false;
    public static bool tutorialQuest4 = false;

    public InputAction moveAction;
    public InputAction skillAction;

    public GameObject tutorialEnemy1;
    public GameObject shootPointLeft;
    public GameObject enemyLocation1;
    public GameObject enemyLocation2;

    public Transform shootPoint;
    private Shooter shootScript;

    private int barrierCount = 0;
    [SerializeField] private Timer timer;
    [SerializeField] private Cost _playerCost;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        isPaused = false;
        /*Cursor.lockState = CursorLockMode.Locked;*/
        Cursor.visible = true;

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

        /*Time.timeScale = 2f;*/
    }

    // Update is called once per frame
    void Update()
    {
        _playerCost.Value = 10;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // 게임 재개
            }
            else
            {
                PauseGame(); // 게임 일시정지
            }
        }

        if (step1)
        {
            if(tutorialQuest1 && tutorialQuest2 && tutorialQuest3 && tutorialQuest4)
            {
                step1 = false;
                timer.Resume();
            }

            joystickVertical = Input.GetAxis("Vertical");
            joystickHorizontal = Input.GetAxis("Horizontal");

            keyboardWPressed = Input.GetKey(KeyCode.W); // W 키 또는 조이스틱 위쪽
            keyboardSPressed = Input.GetKey(KeyCode.S); // S 키 또는 조이스틱 아래쪽
            keyboardAPressed = Input.GetKey(KeyCode.A); // A 키 또는 조이스틱 왼쪽
            keyboardDPressed = Input.GetKey(KeyCode.D); // D 키 또는 조이스틱 오른쪽

            if(joystickBackground != null && joystickHandle != null)
            {
                joystickInput = joystickHandle.anchoredPosition / (joystickBackground.sizeDelta / 2f); // 로컬 좌표 기준 정규화
                bool joystickWPressed = joystickInput.y > joystickThreshold; // 위쪽
                bool joystickSPressed = joystickInput.y < -joystickThreshold; // 아래쪽
                bool joystickAPressed = joystickInput.x < -joystickThreshold; // 왼쪽
                bool joystickDPressed = joystickInput.x > joystickThreshold; // 오른쪽

                Debug.Log($"Joystick Input: {joystickInput}");
                Debug.Log($"W: {joystickWPressed}, S: {joystickSPressed}, A: {joystickAPressed}, D: {joystickDPressed}");

                keyboardWPressed |= joystickWPressed;
                keyboardSPressed |= joystickSPressed;
                keyboardAPressed |= joystickAPressed;
                keyboardDPressed |= joystickDPressed;
            }
            
            if (keyboardWPressed)
            {
                if (keyboardSPressed) return;

                if (keyWPressStartTime < 0f) // 눌리기 시작한 시간을 저장
                {
                    keyWPressStartTime = Time.time;
                }
                else if (Time.time - keyWPressStartTime >= pressDurationThreshold) // 0.5초 이상 눌렸는지 확인
                {
                    tutorialQuest1 = true;
                }
            }
            else
            {
                keyWPressStartTime = -1f; // 키를 떼면 초기화
            }

            // A 키 입력 처리
            if (keyboardAPressed)
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

            // S 키 입력 처리
            if (keyboardSPressed)
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

            // D 키 입력 처리
            if (keyboardDPressed)
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
        if (step3)
        {
            if (!keyboardSpacePressed)
            {
                keyboardSpacePressed = Input.GetKey(KeyCode.Space);
            }
            // Space 키 입력 처리
            if (keyboardSpacePressed)
            {
                ProcessSpacePress();
            }
            else
            {
                ResetSpacePress();
            }
        }
        
        if(step5)
        {
            keyboardWPressed = Input.GetKey(KeyCode.W);
            keyboardAPressed = Input.GetKey(KeyCode.A);
            keyboardSPressed = Input.GetKey(KeyCode.S);
            keyboardDPressed = Input.GetKey(KeyCode.D);
            keyboardShiftPressed = Input.GetKey(KeyCode.LeftShift);
        }

        if (step6)
        {
            keyboardQPressed = Input.GetKey(KeyCode.Q);
        }
    }

    public void ProcessSpacePress()
    {
        if (spacePressStartTime < 0f)
        {
            spacePressStartTime = Time.time;
        }
        else
        {
            float elapsedTime = Time.time - spacePressStartTime;
            spacePressProgress = Mathf.Clamp01(elapsedTime / spacePressDurationThreshold);

            if (elapsedTime >= spacePressDurationThreshold) // 1초 이상 눌렸는지 확인
            {
                Debug.Log("Space 입력 완료");

                step3 = false;
                timer.Resume();

                spacePressStartTime = -1f; // 초기화
            }
        }
    }

    public void ResetSpacePress()
    {
        spacePressStartTime = -1f;
        spacePressProgress = 0f;
    }

    public void OnSpaceSkillButtonPress()
    {
        // Space 키 입력과 동일한 처리
        keyboardSpacePressed = true;
    }

    public void OutSpaceSkillButtonPress()
    {
        keyboardSpacePressed = false;
    }

    public void OnQSkillButtonPress()
    {
        keyboardQPressed = true;
    }
    public void getStep1()
    {
        //움직임 커맨드 튜토리얼
        Debug.Log("step1");
        step1 = true;
        step1Started = true;
    }

    public void getStep2()
    {
        Debug.Log("step2");
        step2 = true;
    }
    public void getStep3()
    {
        //배리어 사용 튜토리얼
        Debug.Log("step3");
        step3 = true;
        step3Started = true;
    }
    public void getStep4()
    {
        //배리어로 공격 막기 튜토리얼
        Debug.Log("step4");
        step4 = true;
    }
    public void getStep5()
    {
        Debug.Log("step5");
        step5 = true;
        step5Started = true;
    }
    public void getStep6()
    {
        Debug.Log("step6");
        step6 = true;
        step6Started = true;
    }

    public void clearstep4()
    {
        //배리어로 방어 성공할 경우
        step4 = false;
        timer.Resume();
        
    }
    public void clearStep5()
    {
        if(step5)
        {
            step5 = false;
            timer.Resume();
        }
    }
    public void clearStep6()
    {
        if (step6)
        {
            step6 = false;
            timer.Resume();
        }
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
        Debug.Log("에너미 이동");
        // 딜레이 적용
        yield return new WaitForSeconds(delay);

        // null 체크
        if (movingObject != null && targetLocation != null)
        {
            // 부드럽게 이동
            float elapsedTime = 0f;
            float moveDuration = 1f; // 이동 시간
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
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
        Debug.Log("Game Resumed");
    }
}
