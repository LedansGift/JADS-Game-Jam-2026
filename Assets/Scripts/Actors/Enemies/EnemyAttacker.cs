using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    private bool attacking = false;
    private bool enemyActive = false;

    private float attackDamage = 10f;

    private float attackTimer = 0f;
    private float attackFrequency = 1f;
    private float attackDelay = 0.5f;

    private float attackCheckRadius = 0.5f;

    private float attackCheckRefreshTimer = 0f;
    private float attackCheckRefreshFrequency = 0.2f;

    private StructureHealth attackingStructure;

    public EventHandler<bool> OnAttackableStructureNearby;

    [SerializeField]
    private bool laneAttacker = true;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask structureMask;

    private void Update()
    {
        if (!enemyActive)
        {
            return;
        }

        if (!attacking)
        {
            StructureCheck();
            return;
        }

        AttackStructure();
    }

    private void StructureCheck()
    {
        attackCheckRefreshTimer += Time.deltaTime;

        if (attackCheckRefreshTimer < attackCheckRefreshFrequency)
        {
            return;
        }

        attackCheckRefreshTimer = 0f;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            attackCheckRadius,
            structureMask
        );

        //List<StructureHealth> healths = new List<StructureHealth>();

        //Debug.Log("Colliders Found: " + colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            if (
                collider.TryGetComponent<StructureHealth>(out StructureHealth hitHealth)
                && (hitHealth.GetIsLaneStructure() == laneAttacker)
                && hitHealth.IsStructureActive()
            )
            {
                attackingStructure = hitHealth;
                attackTimer = attackFrequency;
                OnAttackableStructureNearby?.Invoke(this, true);
                break;
            }
        }
    }

    private void AttackStructure()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer < attackFrequency)
        {
            return;
        }

        if (!attackingStructure.IsStructureActive())
        {
            OnAttackableStructureNearby?.Invoke(this, false);
            return;
        }

        attackTimer = 0f;

        StartCoroutine(DealDamageToStructure());
    }

    private IEnumerator DealDamageToStructure()
    {
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(attackDelay);

        attackingStructure.TakeDamage(attackDamage);
    }

    public void SetupAttacker(float attackDamage, float attackFrequency)
    {
        this.attackDamage = attackDamage;
        this.attackFrequency = attackFrequency;
    }

    public void ToggleAttacking(bool toggle)
    {
        attacking = toggle;
    }

    public void ToggleEnemyActive(bool toggle)
    {
        enemyActive = toggle;
    }
}
