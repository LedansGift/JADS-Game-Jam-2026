using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermissionUI : MonoBehaviour
{
    private int structureChosen = -1;
    private GameManager gameManager;

    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private GameObject[] roundEndLetters;

    [SerializeField]
    private StructureButton[] structureButtons;

    private void Start()
    {
        GameManager.OnRoundEnd += RoundEndIntermission;

        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);
    }

    private void OnDisable()
    {
        GameManager.OnRoundEnd -= RoundEndIntermission;
    }

    private void DeselectAllStructures()
    {
        foreach (StructureButton structure in structureButtons)
        {
            structure.ToggleSelectStructured(false);
        }
    }

    private void DisableAllLeters()
    {
        foreach (GameObject letter in roundEndLetters)
        {
            letter.SetActive(false);
        }
    }

    private void RoundEndIntermission(object sender, int roundIndex)
    {
        if (!gameManager)
        {
            gameManager = sender as GameManager;
        }

        structureChosen = -1;

        if (roundIndex < roundEndLetters.Length)
        {
            roundEndLetters[roundIndex].SetActive(true);
        }

        DeselectAllStructures();

        List<bool> currentStructureAvailability =
            StructureAvailabilityManager.Instance.GetStructureAvailability();

        for (int i = 0; i < structureButtons.Length; i++)
        {
            structureButtons[i].SetStructureAvailability(currentStructureAvailability[i]);
        }

        StartCoroutine(DisplayIntermissionMenu());
    }

    private IEnumerator DisplayIntermissionMenu()
    {
        yield return new WaitForSeconds(1f);

        fader.ToggleFade(true);
        fader.ToggleBlockRaycasts(true);
    }

    public void SelectStructure(int structureIndex)
    {
        if (structureIndex >= structureButtons.Length)
        {
            return;
        }

        List<bool> currentStructureAvailability =
            StructureAvailabilityManager.Instance.GetStructureAvailability();

        if (!currentStructureAvailability[structureIndex])
        {
            return;
        }

        structureChosen = structureIndex;

        DeselectAllStructures();
        structureButtons[structureIndex].ToggleSelectStructured(true);
    }

    public void TryFinishIntermission()
    {
        if (structureChosen < 0)
        {
            //Display "You must pick a Blueprint" graphic
            return;
        }

        StructureAvailabilityManager.Instance.SetStructureAvailability(structureChosen, false);

        fader.ToggleFade(false);
        fader.ToggleBlockRaycasts(false);

        gameManager.StartNextRound();
    }
}
