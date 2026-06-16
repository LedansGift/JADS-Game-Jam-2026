using System;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

public class BuildUI : MonoBehaviour
{
    private bool buildUIActive = false;
    private float structureGroupXGoal;
    private float groupScrollSpeed = 750f;

    private const float STRUCTURE_GROUP_START_X = 100f;
    private const float STRUCTURE_GROUP_ADVANCEMENT = 110f;

    private Vector3 playerOffset = new Vector2(0f, 2f);

    private RectTransform rectTransform;

    private Transform playerTransform;

    private StructureStats[] structureStats;

    [SerializeField]
    private BuildStructureElement[] structureUIs;

    [SerializeField]
    private RectTransform structuresGroup;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
        rectTransform = GetComponent<RectTransform>();

        CinemachineCore.CameraUpdatedEvent.AddListener(OnCameraUpdated);

        structureStats = StructureAvailabilityManager.Instance.GetStructureStats();
        fader.SetCanvasGroupAlpha(0f);
        structuresGroup.anchoredPosition = new Vector2(STRUCTURE_GROUP_START_X, 0f);
        structureGroupXGoal = STRUCTURE_GROUP_START_X;

        SetNewActiveStructure(this, 0);
    }

    private void Update()
    {
        if (!buildUIActive)
        {
            return;
        }

        structuresGroup.anchoredPosition = new Vector2(
            Mathf.MoveTowards(
                structuresGroup.anchoredPosition.x,
                structureGroupXGoal,
                groupScrollSpeed * Time.deltaTime
            ),
            0f
        );
    }

    private void OnEnable()
    {
        PlayerBuilder.OnToggleBuildUI += ToggleBuildUI;
        PlayerBuilder.OnNewActiveStructure += SetNewActiveStructure;
    }

    private void OnDisable()
    {
        PlayerBuilder.OnToggleBuildUI -= ToggleBuildUI;
        PlayerBuilder.OnNewActiveStructure -= SetNewActiveStructure;
        CinemachineCore.CameraUpdatedEvent.RemoveListener(OnCameraUpdated);
    }

    private void SetUIPosition()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(
            playerTransform.position + playerOffset
        );

        rectTransform.anchoredPosition = screenPosition;
    }

    private void ToggleBuildUI(object sender, bool toggle)
    {
        fader.ToggleFade(toggle);
        //Reset visuals to show default option
        // if (toggle)
        // {

        // }

        //structureGroupPreviousXGoal = STRUCTURE_GROUP_START_X;

        if (toggle)
        {
            List<bool> availabilities =
                StructureAvailabilityManager.Instance.GetStructureAvailability();

            for (int i = 0; i < availabilities.Count; i++)
            {
                structureUIs[i + 1].ToggleUnavailable(!availabilities[i]);
            }

            //Update affordability of UIs based on scrap
        }

        buildUIActive = toggle;
    }

    private void SetNewActiveStructure(object sender, int newIndex)
    {
        //structureGroupPreviousXGoal = structureGroupXGoal;
        structureGroupXGoal = STRUCTURE_GROUP_START_X - (STRUCTURE_GROUP_ADVANCEMENT * newIndex);

        foreach (BuildStructureElement buildStructure in structureUIs)
        {
            buildStructure.ToggleOpacity(false);
        }

        structureUIs[newIndex].ToggleOpacity(true);
    }

    private void OnCameraUpdated(CinemachineBrain arg0)
    {
        SetUIPosition();
    }
}
