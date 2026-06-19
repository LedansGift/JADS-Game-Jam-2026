using System;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private bool tutorialActive = false;
    private int tutorialStepIndex = 0;

    private PlayerBuilder playerBuilder;

    [SerializeField]
    private CanvasGroupFader[] tutorialFaders;

    private GameManager gameManager;

    private void Start()
    {
        GameManager.OnTutorialStart += StartTutorial;
        PlayerBuilder.OnToggleBuildUI += Step1Advance;

        playerBuilder = PlayerIdentifier.PlayerTransform.GetComponent<PlayerBuilder>();

        playerBuilder.OnSuccessfulBuild += Step2Advance;
        Structure.OnStructureBuilt += Step3Advance;

        foreach (CanvasGroupFader fader in tutorialFaders)
        {
            fader.SetCanvasGroupAlpha(0f);
        }
    }

    private void OnDisable()
    {
        GameManager.OnTutorialStart -= StartTutorial;
        PlayerBuilder.OnToggleBuildUI -= Step1Advance;
        if (playerBuilder)
        {
            playerBuilder.OnSuccessfulBuild -= Step2Advance;
        }
        Structure.OnStructureBuilt -= Step3Advance;
    }

    private void FinishTutorial()
    {
        tutorialActive = false;
        gameManager.TutorialFinished();
    }

    private void AdvanceTutorial()
    {
        tutorialFaders[tutorialStepIndex].ToggleFade(false);
        tutorialStepIndex++;

        if (tutorialStepIndex >= tutorialFaders.Length)
        {
            FinishTutorial();
            return;
        }

        tutorialFaders[tutorialStepIndex].ToggleFade(true);
    }

    private void StartTutorial(object sender, EventArgs e)
    {
        gameManager = sender as GameManager;
        tutorialActive = true;
        tutorialStepIndex = 0;

        tutorialFaders[tutorialStepIndex].ToggleFade(true);
    }

    private void Step1Advance(object sender, bool e)
    {
        if (tutorialActive && (tutorialStepIndex == 0))
        {
            AdvanceTutorial();
        }
    }

    private void Step2Advance()
    {
        if (tutorialActive && (tutorialStepIndex == 1))
        {
            AdvanceTutorial();
        }
    }

    private void Step3Advance()
    {
        if (tutorialActive && (tutorialStepIndex == 2))
        {
            AdvanceTutorial();
        }
    }
}
