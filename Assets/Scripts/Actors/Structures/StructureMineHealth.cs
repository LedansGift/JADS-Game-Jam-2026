using System.Collections.Generic;
using UnityEngine;

public class StructureMineHealth : StructureHealth
{
    [SerializeField]
    private LayerMask enemyLayerMask;

    protected override void DestroyStructure()
    {
        Structure structure = GetComponent<Structure>();
        StructureStats structureStats = structure.GetStats();

        DamageNearbyEnemies(structureStats);

        base.DestroyStructure();
    }

    private void DamageNearbyEnemies(StructureStats stats)
    {
        //Play explosionFX

        HealthSystem[] hitObjects = GetHitObjects(stats.attackRange, false);

        foreach (HealthSystem health in hitObjects)
        {
            health.TakeDamage(stats.damage);
        }
    }

    private HealthSystem[] GetHitObjects(float attackRange, bool playerObjects)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayerMask
        );

        List<HealthSystem> healths = new List<HealthSystem>();

        foreach (Collider2D collider in colliders)
        {
            if (
                collider.TryGetComponent<HealthSystem>(out HealthSystem hitHealth)
                && (hitHealth.GetIsPlayer() == playerObjects)
            )
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
    }
}
