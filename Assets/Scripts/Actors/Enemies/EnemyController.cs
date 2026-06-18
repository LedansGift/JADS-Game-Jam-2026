using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool enemyActive = false;

    private EnemyHealth health;
    private EnemyMovement movement;
    private EnemyAttacker attacker;

    [SerializeField]
    private EnemyStats stats;

    [SerializeField]
    private Animator animator;

    public static Action OnEnemyDead;

    private void Start()
    {
        health = GetComponent<EnemyHealth>();
        movement = GetComponent<EnemyMovement>();
        attacker = GetComponent<EnemyAttacker>();

        health.OnEnemyDead += DespawnEnemy;
        health.OnEnemyDamaged += DamageEnemy;

        health.SetMaxHealth(stats.maxHealth, stats.strongholdDamage);
        movement.SetupMovement(stats.movementSpeed);

        attacker.SetupAttacker(stats.damage, stats.attackFrequency);
        attacker.OnAttackableStructureNearby += ToggleAttackMode;
    }

    private void OnDisable()
    {
        health.OnEnemyDead -= DespawnEnemy;
        health.OnEnemyDamaged -= DamageEnemy;
        attacker.OnAttackableStructureNearby -= ToggleAttackMode;
    }

    private void DamageEnemy()
    {
        animator.SetTrigger("damage");
    }

    public void SpawnEnemy()
    {
        enemyActive = true;

        animator.SetBool("dead", false);

        attacker.ToggleAttacking(false);
        attacker.ToggleEnemyActive(true);
        health.ReviveEnemy();

        movement.StartMovement();

        StartCoroutine(DelayedAnimationReset());
    }

    private IEnumerator DelayedAnimationReset()
    {
        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("reset");
    }

    private void DespawnEnemy()
    {
        enemyActive = false;
        animator.SetTrigger("die");
        animator.SetBool("dead", true);
        movement.StopMovement();
        attacker.ToggleAttacking(false);
        attacker.ToggleEnemyActive(false);

        OnEnemyDead?.Invoke();
    }

    private void ToggleAttackMode(object sender, bool attacking)
    {
        if (!enemyActive)
        {
            return;
        }

        attacker.ToggleAttacking(attacking);

        if (attacking)
        {
            movement.StopMovement();
        }
        else
        {
            movement.StartMovement();
        }
    }

    public bool GetIsEnemyActive()
    {
        return enemyActive;
    }
}
