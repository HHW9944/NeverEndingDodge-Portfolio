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
    public GameObject Tutorial3UICanvas;

    public Image buttonW;
    public Image buttonA;
    public Image buttonS;
    public Image buttonD;
    public Image buttonSpace;

    public Slider quest5Slider;

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
            
            buttonW.color = TutorialManager.keyboardWPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            buttonA.color = TutorialManager.keyboardAPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            buttonS.color = TutorialManager.keyboardSPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            buttonD.color = TutorialManager.keyboardDPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

            tutorialQuest1.color = TutorialManager.tutorialQuest1 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest2.color = TutorialManager.tutorialQuest2 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest3.color = TutorialManager.tutorialQuest3 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest4.color = TutorialManager.tutorialQuest4 ? Color.green : new Color(1f, 1f, 1f, 1f);
        }
        else 
        {
            Tutorial1UICanvas.SetActive(false);
        }

        if(TutorialManager.tutorial2)
        {
            Tutorial2UICanvas.SetActive(true);

            buttonSpace.color = TutorialManager.keyboardSpacePressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            quest5Slider.value = TutorialManager.spacePressProgress;
            Debug.Log(TutorialManager.spacePressProgress);
        }
        else
        {
            Tutorial2UICanvas.SetActive(false);
        }

        if (TutorialManager.tutorial3)
        {
            Tutorial3UICanvas.SetActive(true);
        }
        else
        {
            Tutorial3UICanvas.SetActive(false);
            
        }
    }


}
