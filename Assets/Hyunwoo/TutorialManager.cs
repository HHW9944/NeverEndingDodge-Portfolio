using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public Transform player;

    public bool isInputActive = false;
    public static bool tutorial1 = false;
    public static bool tutorial2 = false;

    public static bool tutorialQuest1 = false; //Quest 1 ~ 4 : WASD Movement Quest in Tutorial1
    public static bool tutorialQuest2 = false;
    public static bool tutorialQuest3 = false;
    public static bool tutorialQuest4 = false;

    private InputAction moveAction;

    public GameObject tutorialEnemy;
    public GameObject enemyLocation;

    public Transform shootPoint;
    private Shooter shootScript;

    [SerializeField] private Timer timer;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        shootPoint = tutorialEnemy.transform.Find("ShootPoint(left)");
        if (shootPoint == null)
        {
            Debug.LogError("shooterpoint not found in tutorialEnemy");
        }

        shootScript = shootPoint.GetComponent<Shooter>();
        if (shootScript != null)
        {
            Debug.LogError("shootScipt not found in shootPoint");
        }

        shootScript.enabled = false;
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

            if (Input.GetKeyDown(KeyCode.W))
            {          
                tutorialQuest1 = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                tutorialQuest2 = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                tutorialQuest3 = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                tutorialQuest4 = true;
            }
        }
        else if(tutorial2)
        {
            if (Input.GetKeyDown (KeyCode.Space))
            {
                Debug.Log("Space 입력");
                
                tutorial2 = false;
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

    public void enemyAppear()
    {
        tutorialEnemy.SetActive(true);
        StartCoroutine(ActivateEnemyWithDelay(1f));
    }

    public void enemyShootActive()
    {
        shootScript.enabled = true;
    }

    private IEnumerator ActivateEnemyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 1초 대기

        if (tutorialEnemy != null && enemyLocation != null)
        {
            tutorialEnemy.SetActive(true); // 적 활성화

            // 부드럽게 이동
            float elapsedTime = 0f;
            float moveDuration = 1f; // 이동 시간
            Vector3 startPos = tutorialEnemy.transform.position;

            while (elapsedTime < moveDuration)
            {
                tutorialEnemy.transform.position = Vector3.Lerp(startPos, enemyLocation.transform.position, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            tutorialEnemy.transform.position = enemyLocation.transform.position; // 최종 위치
        }
        else
        {
            Debug.LogError("tutorialEnemy or enemyLocation is not assigned!");
        }
    }
}
