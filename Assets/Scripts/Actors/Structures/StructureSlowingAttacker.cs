using System.Collections.Generic;
using UnityEngine;

public class StructureSlowingAttacker : StructureAttacker
{
    protected override void AttackEnemies(List<EnemyHealth> enemies)
    {
        foreach (EnemyHealth enemyHealth in enemies)
        {
            EnemyMovement movement = enemyHealth.GetComponent<EnemyMovement>();
            movement.SlowMovement(stats.damage);
        }

        attackReady = false;
    }
}
