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
    private bool debuffedTowers = false;

    private int activeStructureIndex = 0;

    private float barkSpawnPosition = 1.5f;

    [SerializeField]
    private GameObject[] structurePrefabs;

    private StructureStats[] prefabStats;

    public Action OnSuccessfulBuild;

    [SerializeField]
    private SFXObject buildDenialSFX;

    [SerializeField]
    private SFXObject placeStructureSFX;

    [SerializeField]
    private SFXObject openBuildMenuSFX;

    [SerializeField]
    private SFXObject cycleBuildMenuSFX;

    [SerializeField]
    private SFXObject closeBuildMenuSFX;

    public static EventHandler<bool> OnToggleBuildUI;
    public static EventHandler<int> OnNewActiveStructure;

    private void Start()
    {
        InputManager.Instance.OnAttackEvent += TryBuildStructure;
        InputManager.Instance.OnCycleLeftEvent += CycleStructuresLeft;
        InputManager.Instance.OnCycleRightEvent += CycleStructuresRight;
        GameManager.OnDebuffTowers += SetDebuffStatus;

        prefabStats = StructureAvailabilityManager.Instance.GetStructureStats();
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= TryBuildStructure;
        InputManager.Instance.OnCycleLeftEvent -= CycleStructuresLeft;
        InputManager.Instance.OnCycleRightEvent -= CycleStructuresRight;
        GameManager.OnDebuffTowers -= SetDebuffStatus;
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

        bool hasStructure = LevelGrid.Instance.HasAnyStructureOnGridPosition(currentGridPosition);

        if (activeStructureIndex <= 0)
        {
            if (!hasStructure)
            {
                return;
            }

            Structure structure = LevelGrid.Instance.GetStructureAtGridPosition(
                currentGridPosition
            );

            structure.DestroyStructure();

            OnSuccessfulBuild?.Invoke();

            return;
        }

        int prefabIndex = activeStructureIndex - 1;

        StructureStats prefabStructureStats = prefabStats[prefabIndex];

        if (!StructureAvailabilityManager.Instance.GetStructureAvailability()[prefabIndex])
        {
            TextBarkManager.SpawnBark(
                transform.position + new Vector3(0, barkSpawnPosition, 0),
                "No Blueprint",
                Color.red
            );
            AudioManager.PlaySFX(buildDenialSFX, transform.position);
            return;
        }

        if (hasStructure)
        {
            TextBarkManager.SpawnBark(
                transform.position + new Vector3(0, barkSpawnPosition, 0),
                "Occupied by another tower",
                Color.red
            );
            AudioManager.PlaySFX(buildDenialSFX, transform.position);
            return;
        }

        if (
            LevelGrid.Instance.IsLanePosition(currentGridPosition)
            != prefabStructureStats.laneStructure
        )
        {
            if (prefabStructureStats.laneStructure)
            {
                TextBarkManager.SpawnBark(
                    transform.position + new Vector3(0, barkSpawnPosition, 0),
                    "Must be placed in a lane",
                    Color.red
                );
            }
            else
            {
                TextBarkManager.SpawnBark(
                    transform.position + new Vector3(0, barkSpawnPosition, 0),
                    "Cannot be placed in lane",
                    Color.red
                );
            }

            AudioManager.PlaySFX(buildDenialSFX, transform.position);
            return;
        }

        if (!ScrapManager.Instance.IsEnoughAvailableScrap(prefabStructureStats.scrapCost))
        {
            TextBarkManager.SpawnBark(
                transform.position + new Vector3(0, barkSpawnPosition, 0),
                "Not Enough Scrap",
                Color.red
            );

            AudioManager.PlaySFX(buildDenialSFX, transform.position);
            return;
        }

        PlaceStructure(structurePrefabs[prefabIndex], currentGridPosition);

        //ScrapManager.Instance.SpendScrap(prefabStructureStats.scrapCost);

        AudioManager.PlaySFX(placeStructureSFX, transform.position);

        OnSuccessfulBuild?.Invoke();
    }

    private void PlaceStructure(GameObject structure, GridPosition gridPosition)
    {
        Vector3 spawnPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        Structure newStructure = Instantiate(structure, spawnPosition, Quaternion.identity)
            .GetComponent<Structure>();
        newStructure.SetStructureGridPosition(gridPosition);

        if (debuffedTowers)
        {
            newStructure.SetStructureDebuffStatus();
        }

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
            AudioManager.PlaySFX(cycleBuildMenuSFX, transform.position);
            OnNewActiveStructure?.Invoke(this, activeStructureIndex);

            if (activeStructureIndex == 0) { }
            else
            {
                GridMouseVisual.Instance.SetLaneStructureActive(
                    prefabStats[activeStructureIndex - 1].laneStructure
                );
            }
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
            AudioManager.PlaySFX(cycleBuildMenuSFX, transform.position);
            OnNewActiveStructure?.Invoke(this, activeStructureIndex);
            GridMouseVisual.Instance.SetLaneStructureActive(
                prefabStats[activeStructureIndex - 1].laneStructure
            );
        }
    }

    public void ToggleBuildMode(bool toggle)
    {
        if (buildModeActive == toggle)
        {
            return;
        }

        buildModeActive = toggle;
        GridMouseVisual.Instance.ToggleMouseVisibility(toggle);
        OnToggleBuildUI?.Invoke(this, toggle);

        if (buildModeActive)
        {
            AudioManager.PlaySFX(openBuildMenuSFX, transform.position);
        }
        else
        {
            AudioManager.PlaySFX(closeBuildMenuSFX, transform.position);
        }
    }

    private void SetDebuffStatus()
    {
        debuffedTowers = true;
    }

    public bool IsBuilding()
    {
        return buildModeActive;
    }
}
