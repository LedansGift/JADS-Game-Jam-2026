using System;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private bool structureActive = false;
    private float buildProgress = 0;
    private float buildFinished = 4;

    private StructureHealth health;

    [SerializeField]
    private GameObject structureTempVisual;

    [SerializeField]
    private StructureStats stats;

    private void Awake()
    {
        health = GetComponent<StructureHealth>();
        health.SetMaxHealth(stats.maxHealth);
        health.OnStructureDestroyed += DestroyStructure;
    }

    private void OnDisable()
    {
        health.OnStructureDestroyed -= DestroyStructure;
    }

    public void BuildStructure()
    {
        buildProgress++;
        Debug.Log("Structure hit");

        if (StructureBuilt())
        {
            Debug.Log("Structure Built");

            //Finalise building structure, activate it
            health.ActivateStructure();
            structureTempVisual.SetActive(true);
        }
    }

    private void DestroyStructure()
    {
        //Deactivate attacker, play destroy animation
        //Destroy object
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
