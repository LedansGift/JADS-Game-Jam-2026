using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntermissionUI : MonoBehaviour
{
    private int currentRoundIndex = 0;
    private int structureChosen = -1;

    private int blueprintLossStartRound = 1;
    private int finalRoundIndex = 4;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private GameObject blueprintLossMenu;

    [SerializeField]
    private GameObject pickBluePrintAlert;

    [SerializeField]
    private GameObject initialLetter;

    [SerializeField]
    private GameObject[] roundEndLetters;

    [SerializeField]
    private StructureButton[] structureButtons;

    [SerializeField]
    private SFXObject buttonClickSFX;

    private void Start()
    {
        GameManager.OnRoundEnd += RoundEndIntermission;

        fader.SetCanvasGroupAlpha(1f);
        fader.ToggleBlockRaycasts(true);
        blueprintLossMenu.SetActive(false);
        pickBluePrintAlert.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.OnRoundEnd -= RoundEndIntermission;

        StopAllCoroutines();
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
        initialLetter.SetActive(false);

        foreach (GameObject letter in roundEndLetters)
        {
            letter.SetActive(false);
        }
    }

    private void RoundEndIntermission(object sender, int roundIndex)
    {
        structureChosen = -1;
        currentRoundIndex = roundIndex;

        DisableAllLeters();

        if (currentRoundIndex >= roundEndLetters.Length)
        {
            return;
        }

        roundEndLetters[roundIndex].SetActive(true);

        StartCoroutine(DisplayIntermissionMenu());

        if (roundIndex < blueprintLossStartRound)
        {
            return;
        }

        pickBluePrintAlert.SetActive(false);

        blueprintLossMenu.SetActive(true);

        DeselectAllStructures();

        if (roundIndex >= finalRoundIndex)
        {
            for (int i = 0; i < structureButtons.Length; i++)
            {
                StructureAvailabilityManager.Instance.SetStructureAvailability(i, false);
            }

            structureChosen = 0;
        }

        List<bool> currentStructureAvailability =
            StructureAvailabilityManager.Instance.GetStructureAvailability();

        for (int i = 0; i < structureButtons.Length; i++)
        {
            structureButtons[i].SetStructureAvailability(currentStructureAvailability[i]);
        }
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
        AudioManager.PlaySFX(buttonClickSFX, transform.position);
        if ((currentRoundIndex >= blueprintLossStartRound) && (structureChosen < 0))
        {
            pickBluePrintAlert.SetActive(true);

            return;
        }

        StructureAvailabilityManager.Instance.SetStructureAvailability(structureChosen, false);

        fader.ToggleFade(false);
        fader.ToggleBlockRaycasts(false);

        gameManager.StartNextRound();
    }
}
