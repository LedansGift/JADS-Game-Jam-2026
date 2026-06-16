using System.Collections.Generic;
using UnityEngine;

public class StructureAvailabilityManager : MonoBehaviour
{
    public static StructureAvailabilityManager Instance { get; private set; }

    private List<bool> structureAvailability = new List<bool>();

    [SerializeField]
    private StructureStats[] structureStats;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        for (int i = 0; i < 6; i++)
        {
            structureAvailability.Add(true);
        }
    }

    public List<bool> GetStructureAvailability()
    {
        return structureAvailability;
    }

    public StructureStats[] GetStructureStats()
    {
        return structureStats;
    }

    public void SetStructureAvailability(int strucIndex, bool available)
    {
        structureAvailability[strucIndex] = available;
    }
}
