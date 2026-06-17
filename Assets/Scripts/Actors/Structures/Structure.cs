using System;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private float buildProgress = 0;
    private float buildFinished = 4;

    private GridPosition gridPosition;

    private StructureHealth health;

    [SerializeField]
    private SpriteRenderer structureUnfinishedVisual;

    [SerializeField]
    private SpriteRenderer structureFinishedVisual;

    [SerializeField]
    private StructureAttacker attacker;

    [SerializeField]
    private StructureStats stats;

    private void Awake()
    {
        health = GetComponent<StructureHealth>();
        health.SetMaxHealth(stats.maxHealth);
        health.OnStructureDestroyed += DestroyStructure;

        structureUnfinishedVisual.sortingOrder = Mathf.RoundToInt((-transform.position.y) * 100f);
        structureFinishedVisual.sortingOrder = Mathf.RoundToInt((-transform.position.y) * 100f);

        if (attacker)
        {
            attacker.SetStats(stats);
        }
    }

    private void OnDisable()
    {
        health.OnStructureDestroyed -= DestroyStructure;
    }

    public void BuildStructure()
    {
        buildProgress++;
        //Debug.Log("Structure hit");
        health.SetBuildHealthbar(0.25f * buildProgress);

        if (StructureBuilt())
        {
            //Debug.Log("Structure Built");

            //Finalise building structure, activate it
            health.ActivateStructure(stats.laneStructure);
            structureUnfinishedVisual.gameObject.SetActive(false);
            structureFinishedVisual.gameObject.SetActive(true);

            if (attacker)
            {
                attacker.ToggleAttacker(true);
            }
        }
    }

    public void DestroyStructure()
    {
        //Deactivate attacker, play destroy animation
        //Destroy object

        if (attacker)
        {
            attacker.ToggleAttacker(false);
        }

        structureUnfinishedVisual.gameObject.SetActive(true);
        structureFinishedVisual.gameObject.SetActive(false);

        LevelGrid.Instance.RemoveStructureAtGridPosition(gridPosition, this);

        Destroy(gameObject, 1f);
    }

    public void SetStructureGridPosition(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public bool StructureBuilt()
    {
        return buildProgress >= buildFinished;
    }

    public StructureStats GetStats()
    {
        return stats;
    }
}
