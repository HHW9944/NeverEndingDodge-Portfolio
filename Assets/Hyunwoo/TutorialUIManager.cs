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
    public GameObject PlayerUI;
    public GameObject SubTitle;

    public GameObject step1UICanvas;
    public GameObject step3UICanvas;
    public GameObject step4UICanvas;
    public GameObject step5UICanvas;
    public GameObject step6UICanvas;
    public GameObject PauseUICanvas;
    public GameObject DarkOverlay;

    public GameObject joystickUI;

    public GameObject slowSkillButton;
    public GameObject barrierSkillButton;
    public GameObject dashSkillButton;

    public Image tutorial1ButtonW;
    public Image tutorial1ButtonA;
    public Image tutorial1ButtonS;
    public Image tutorial1ButtonD;

    public Image tutorial5ButtonW;
    public Image tutorial5ButtonA;
    public Image tutorial5ButtonS;
    public Image tutorial5ButtonD;

    public Image tutorial6ButtonQ;

    public Image buttonSpace;
    public Image buttonShift;
    public Image darkOverLayImage;

    public Slider quest5Slider;

    public TextMeshProUGUI tutorialQuest1;
    public TextMeshProUGUI tutorialQuest2;
    public TextMeshProUGUI tutorialQuest3;
    public TextMeshProUGUI tutorialQuest4;

    public float pulseScale = 1.2f;  // 최대 크기 배율
    public float pulseSpeed = 2f;    // 애니메이션 속도

    private Vector3 originalScaleTutorial1W;
    private Vector3 originalScaleTutorial1A;
    private Vector3 originalScaleTutorial1S;
    private Vector3 originalScaleTutorial1D;

    private Vector3 originalScaleTutorial5W;
    private Vector3 originalScaleTutorial5A;
    private Vector3 originalScaleTutorial5S;
    private Vector3 originalScaleTutorial5D;

    private Vector3 originalScaleTutorial6Q;

    private Vector3 originalScaleButtonSpace;
    private Vector3 originalScaleButtonShift;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TutorialUI.SetActive(true);
        PlayerUI.SetActive(false);
        joystickUI.SetActive(false);
        slowSkillButton.SetActive(false);
        barrierSkillButton.SetActive(false);
        dashSkillButton.SetActive(false);

        if (tutorial1ButtonW != null) originalScaleTutorial1W = tutorial1ButtonW.rectTransform.localScale;
        if (tutorial1ButtonA != null) originalScaleTutorial1A = tutorial1ButtonA.rectTransform.localScale;
        if (tutorial1ButtonS != null) originalScaleTutorial1S = tutorial1ButtonS.rectTransform.localScale;
        if (tutorial1ButtonD != null) originalScaleTutorial1D = tutorial1ButtonD.rectTransform.localScale;

        if (tutorial5ButtonW != null) originalScaleTutorial5W = tutorial5ButtonW.rectTransform.localScale;
        if (tutorial5ButtonA != null) originalScaleTutorial5A = tutorial5ButtonA.rectTransform.localScale;
        if (tutorial5ButtonS != null) originalScaleTutorial5S = tutorial5ButtonS.rectTransform.localScale;
        if (tutorial5ButtonD != null) originalScaleTutorial5D = tutorial5ButtonD.rectTransform.localScale;

        if (tutorial6ButtonQ != null) originalScaleTutorial6Q = tutorial6ButtonQ.rectTransform.localScale;

        if (buttonSpace != null) originalScaleButtonSpace = buttonSpace.rectTransform.localScale;
        if (buttonShift != null) originalScaleButtonShift = buttonShift.rectTransform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.isPaused)
        {
            PauseUICanvas.SetActive(true);
            TutorialUI.SetActive(false);
            PlayerUI.SetActive(false);
            SubTitle.SetActive(false);
            joystickUI.SetActive(false);
            slowSkillButton.SetActive(false);
            barrierSkillButton.SetActive(false);
            dashSkillButton.SetActive(false);
        }
        else
        {
            PauseUICanvas.SetActive(false);
            TutorialUI.SetActive(true);
            SubTitle.SetActive(true);
            if (TutorialManager.step1Started) joystickUI.SetActive(true);
            if (TutorialManager.step2) PlayerUI.SetActive(true);
            if (TutorialManager.step3Started && !TutorialManager.step5 && !TutorialManager.step6) barrierSkillButton.SetActive(true);
            if (TutorialManager.step5) dashSkillButton.SetActive(true);
            if (TutorialManager.step6) slowSkillButton.SetActive(true);
        }

        if(TutorialManager.step1)
        {
            step1UICanvas.SetActive(true);

            PulseButton(tutorial1ButtonW, originalScaleTutorial1W);
            PulseButton(tutorial1ButtonA, originalScaleTutorial1A);
            PulseButton(tutorial1ButtonS, originalScaleTutorial1S);
            PulseButton(tutorial1ButtonD, originalScaleTutorial1D);

            tutorial1ButtonW.color = TutorialManager.keyboardWPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial1ButtonA.color = TutorialManager.keyboardAPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial1ButtonS.color = TutorialManager.keyboardSPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial1ButtonD.color = TutorialManager.keyboardDPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);

            tutorialQuest1.color = TutorialManager.tutorialQuest1 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest2.color = TutorialManager.tutorialQuest2 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest3.color = TutorialManager.tutorialQuest3 ? Color.green : new Color(1f, 1f, 1f, 1f);
            tutorialQuest4.color = TutorialManager.tutorialQuest4 ? Color.green : new Color(1f, 1f, 1f, 1f);

            joystickUI.SetActive(true);
        }
        else 
        {
            step1UICanvas.SetActive(false);
        }

        if(TutorialManager.step2)
        {
            /*DarkOverlay.gameObject.SetActive(true);*/
        }
        else
        {
            DarkOverlay.gameObject.SetActive(false);
        }

        if(TutorialManager.step3)
        {
            step3UICanvas.SetActive(true);
            barrierSkillButton.SetActive(true);
            PulseButton(buttonSpace, originalScaleButtonSpace);
            buttonSpace.color = TutorialManager.keyboardSpacePressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            quest5Slider.value = TutorialManager.spacePressProgress;
            Debug.Log(TutorialManager.spacePressProgress);
        }
        else
        {
            step3UICanvas.SetActive(false);
        }

        if (TutorialManager.step4)
        {
            step4UICanvas.SetActive(true);
            
        }
        else
        {
            step4UICanvas.SetActive(false);
        }

        if (TutorialManager.step5)
        {
            step5UICanvas.SetActive(true);
            dashSkillButton.SetActive(true);

            PulseButton(tutorial5ButtonW, originalScaleTutorial5W);
            PulseButton(tutorial5ButtonA, originalScaleTutorial5A);
            PulseButton(tutorial5ButtonS, originalScaleTutorial5S);
            PulseButton(tutorial5ButtonD, originalScaleTutorial5D);
            PulseButton(buttonShift, originalScaleButtonShift);

            tutorial5ButtonW.color = TutorialManager.keyboardWPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial5ButtonA.color = TutorialManager.keyboardAPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial5ButtonS.color = TutorialManager.keyboardSPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            tutorial5ButtonD.color = TutorialManager.keyboardDPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            buttonShift.color = TutorialManager.keyboardShiftPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }
        else
        {
            step5UICanvas.SetActive(false);
            dashSkillButton.SetActive(false);
        }

        if(TutorialManager.step6)
        {
            step6UICanvas.SetActive(true);
            slowSkillButton.SetActive(true);
            PulseButton(tutorial6ButtonQ, originalScaleTutorial6Q);
            tutorial6ButtonQ.color = TutorialManager.keyboardQPressed ? new Color(255 / 255f, 135 / 255f, 135 / 255f, 255 / 255f) : new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }
        else
        {
            step6UICanvas.SetActive(false);
            slowSkillButton.SetActive(false);
        }
    }

    private void PulseButton(Image button, Vector3 originalScale)
    {
        if (button != null)
        {
            float scale = Mathf.Lerp(1f, pulseScale, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            button.rectTransform.localScale = originalScale * scale;
        }
    }
}
