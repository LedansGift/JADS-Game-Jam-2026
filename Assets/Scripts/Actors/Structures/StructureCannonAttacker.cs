using System.Collections.Generic;
using UnityEngine;

public class StructureCannonAttacker : StructureAttacker
{
    private float cannonFireWidth = 1f;

    protected override List<EnemyHealth> PerformEnemyCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(
            transform.position + new Vector3(stats.attackRange / 2f, 0f, 0f),
            new Vector2(stats.attackRange, cannonFireWidth),
            0f,
            enemyLayerMask
        );

        List<EnemyHealth> healths = new List<EnemyHealth>();

        //Debug.Log("Colliders Found: " + colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            if (
                collider.TryGetComponent<EnemyHealth>(out EnemyHealth hitHealth)
                && hitHealth.IsEnemyAlive()
            )
            {
                healths.Add(hitHealth);
            }
        }

        return healths;
    }
}
