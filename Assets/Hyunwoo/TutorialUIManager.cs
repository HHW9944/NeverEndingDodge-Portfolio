using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class TutorialUIManager : MonoBehaviour
{
    public static TutorialUIManager instance;

    public static bool isPaused = false;

    public GameObject TutorialUI;
    public GameObject Tutorial1UICanvas;
    public GameObject Tutorial2UICanvas;

    public Image buttonW;
    public Image buttonA;
    public Image buttonS;
    public Image buttonD;

    public TextMeshProUGUI tutorialQuest1;
    public TextMeshProUGUI tutorialQuest2;
    public TextMeshProUGUI tutorialQuest3;
    public TextMeshProUGUI tutorialQuest4;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TutorialUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {

        }

        if(TutorialManager.tutorial1)
        {
            Tutorial1UICanvas.SetActive(true);
            if(TutorialManager.tutorialQuest1)
            {
                buttonW.color = new Color(0f, 0f, 0f, 51 / 255f);
                tutorialQuest1.color = Color.red; // 임시
            }
            if (TutorialManager.tutorialQuest2)
            {
                buttonA.color = new Color(0f, 0f, 0f, 51 / 255f);
                tutorialQuest2.color = Color.red; // 임시
            }
            if (TutorialManager.tutorialQuest3)
            {
                buttonS.color = new Color(0f, 0f, 0f, 51 / 255f);
                tutorialQuest3.color = Color.red; // 임시
            }
            if (TutorialManager.tutorialQuest4)
            {
                buttonD.color = new Color(0f, 0f, 0f, 51 / 255f);
                tutorialQuest4.color = Color.red; // 임시
            }
        }
        else 
        {
            Tutorial1UICanvas.SetActive(false);
        }
    }


}
