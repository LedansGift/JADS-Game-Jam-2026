using System.Collections.Generic;
using UnityEngine;

public abstract class StructureAttacker : MonoBehaviour
{
    protected bool attackerActive = false;
    protected bool attackReady = false;
    protected bool attackDebuff = false;
    protected float attackTimer = 0f;
    protected float attackCheckTimer = 0f;
    protected float attackCheckFrequency = 0.3f;
    protected float attackDamageReduction = 0.75f;
    protected StructureStats stats;

    [SerializeField]
    protected ParticleSystem attackFX;

    [SerializeField]
    protected SFXObject attackSFX;

    [SerializeField]
    protected Animator structureAnimator;

    [SerializeField]
    protected LayerMask enemyLayerMask;

    protected virtual void Update()
    {
        if (!attackerActive)
        {
            return;
        }

        TargetCheck();
    }

    private void TargetCheck()
    {
        if (!attackReady)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= stats.attackFrequency)
            {
                attackReady = true;

                TryAttackEnemies();

                return;
            }
        }
        else
        {
            attackCheckTimer += Time.deltaTime;

            if (attackCheckTimer >= attackCheckFrequency)
            {
                TryAttackEnemies();
                attackCheckTimer = 0f;
            }
        }
    }

    private void TryAttackEnemies()
    {
        List<EnemyHealth> attackableEnemies = PerformEnemyCheck();
        if (attackableEnemies.Count > 0)
        {
            AttackEnemies(attackableEnemies);
        }
    }

    protected virtual void AttackEnemies(List<EnemyHealth> enemies)
    {
        structureAnimator.SetTrigger("attack");

        foreach (EnemyHealth enemyHealth in enemies)
        {
            enemyHealth.TakeDamage(stats.damage * attackDamageReduction);
        }

        if (attackFX)
        {
            attackFX.Play();
        }

        if (attackSFX)
        {
            AudioManager.PlaySFX(attackSFX, transform.position);
        }

        attackTimer = 0f;
        attackReady = false;
    }

    protected virtual List<EnemyHealth> PerformEnemyCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            stats.attackRange,
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

    public virtual void ToggleAttacker(bool toggle)
    {
        attackerActive = toggle;

        if (toggle)
        {
            structureAnimator.SetTrigger("idle");
        }
    }

    public void DebuffTower()
    {
        attackDebuff = true;
    }

    public void SetStats(StructureStats stats)
    {
        this.stats = stats;
    }
}
