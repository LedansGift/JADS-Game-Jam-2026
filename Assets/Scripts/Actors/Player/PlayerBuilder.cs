using System;
using UnityEngine;

// enum StructureType
// {
//     singleTarget,
//     aoe,
//     slow,
//     cannon,
//     barricade,
//     mine
// }

public class PlayerBuilder : MonoBehaviour
{
    private bool buildModeActive = false;

    private int activeStructureIndex = 0;

    [SerializeField]
    private GameObject[] structurePrefabs;

    private StructureStats[] prefabStats;

    public Action OnSuccessfulBuild;

    public static EventHandler<bool> OnToggleBuildUI;
    public static EventHandler<int> OnNewActiveStructure;

    private void Start()
    {
        InputManager.Instance.OnAttackEvent += TryBuildStructure;
        InputManager.Instance.OnCycleLeftEvent += CycleStructuresLeft;
        InputManager.Instance.OnCycleRightEvent += CycleStructuresRight;

        prefabStats = StructureAvailabilityManager.Instance.GetStructureStats();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= TryBuildStructure;
        InputManager.Instance.OnCycleLeftEvent -= CycleStructuresLeft;
        InputManager.Instance.OnCycleRightEvent -= CycleStructuresRight;
    }

    private void TryBuildStructure()
    {
        if (!IsBuilding())
        {
            return;
        }

        if (!GridMouseVisual.Instance.TryGetValidGridPosition(out GridPosition currentGridPosition))
        {
            return;
        }

        if (activeStructureIndex <= 0)
        {
            //Try remove foundational structure

            OnSuccessfulBuild?.Invoke();

            return;
        }

        int prefabIndex = activeStructureIndex - 1;

        if (!StructureAvailabilityManager.Instance.GetStructureAvailability()[prefabIndex])
        {
            return;
        }

        if (LevelGrid.Instance.HasAnyStructureOnGridPosition(currentGridPosition))
        {
            return;
        }

        //If not enough scrap, return

        PlaceStructure(structurePrefabs[prefabIndex], currentGridPosition);

        OnSuccessfulBuild?.Invoke();
    }

    private void PlaceStructure(GameObject structure, GridPosition gridPosition)
    {
        Vector3 spawnPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        Structure newStructure = Instantiate(structure, spawnPosition, Quaternion.identity)
            .GetComponent<Structure>();

        LevelGrid.Instance.AddStructureAtGridPosition(gridPosition, newStructure);
    }

    private void CycleStructuresLeft()
    {
        if (!IsBuilding())
        {
            return;
        }

        if (activeStructureIndex > 0)
        {
            activeStructureIndex--;
            OnNewActiveStructure?.Invoke(this, activeStructureIndex);
        }
    }

    private void CycleStructuresRight()
    {
        if (!IsBuilding())
        {
            return;
        }

        if (activeStructureIndex < structurePrefabs.Length)
        {
            activeStructureIndex++;
            OnNewActiveStructure?.Invoke(this, activeStructureIndex);
        }
    }

    public void ToggleBuildMode(bool toggle)
    {
        buildModeActive = toggle;
        GridMouseVisual.Instance.ToggleMouseVisibility(toggle);
        OnToggleBuildUI?.Invoke(this, toggle);
    }

    public bool IsBuilding()
    {
        return buildModeActive;
    }
}
