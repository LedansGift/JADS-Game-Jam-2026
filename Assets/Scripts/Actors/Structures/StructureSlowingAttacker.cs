using System.Collections.Generic;
using UnityEngine;

public class StructureSlowingAttacker : StructureAttacker
{
    protected override void AttackEnemies(List<EnemyHealth> enemies)
    {
        structureAnimator.SetTrigger("attack");

        foreach (EnemyHealth enemyHealth in enemies)
        {
            EnemyMovement movement = enemyHealth.GetComponent<EnemyMovement>();
            movement.SlowMovement(stats.damage);
        }

        if (attackFX)
        {
            attackFX.Play();
        }

        attackTimer = 0f;
        attackReady = false;
    }
}
