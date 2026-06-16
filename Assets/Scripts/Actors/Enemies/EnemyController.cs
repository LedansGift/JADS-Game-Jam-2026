using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool enemyActive = false;

    private EnemyHealth health;
    private EnemyMovement movement;
    private EnemyAttacker attacker;

    [SerializeField]
    private GameObject enemyVisual;

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

        health.SetMaxHealth(stats.maxHealth);
        movement.SetupMovement(stats.movementSpeed);

        attacker.SetupAttacker(stats.damage, stats.attackFrequency);
        attacker.OnAttackableStructureNearby += ToggleAttackMode;

        //enemyVisual.SetActive(false);
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
        animator.SetTrigger("reset");
        attacker.ToggleAttacking(false);
        attacker.ToggleEnemyActive(true);
        health.ReviveEnemy();
        //enemyVisual.SetActive(true);

        movement.StartMovement();
    }

    private void DespawnEnemy()
    {
        enemyActive = false;
        animator.SetTrigger("die");
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
}
