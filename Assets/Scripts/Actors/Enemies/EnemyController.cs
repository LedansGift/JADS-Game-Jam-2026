using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool enemyActive = false;

    private EnemyHealth health;
    private EnemyMovement movement;

    [SerializeField]
    private EnemyStats stats;

    [SerializeField]
    private Animator animator;

    public static Action OnEnemyDead;

    private void Start()
    {
        health = GetComponent<EnemyHealth>();
        movement = GetComponent<EnemyMovement>();

        health.SetMaxHealth(stats.maxHealth);
        movement.SetupMovement(stats.movementSpeed);
    }

    private void OnEnable()
    {
        health.OnEnemyDead += DespawnEnemy;
        health.OnEnemyDamaged += DamageEnemy;
    }

    private void OnDisable()
    {
        health.OnEnemyDead -= DespawnEnemy;
        health.OnEnemyDamaged -= DamageEnemy;
    }

    private void DamageEnemy()
    {
        animator.SetTrigger("damage");
    }

    public void SpawnEnemy()
    {
        enemyActive = true;
        animator.SetTrigger("reset");
        movement.StartMovement();
    }

    private void DespawnEnemy()
    {
        enemyActive = false;
        animator.SetTrigger("die");
        movement.StopMovement();

        OnEnemyDead?.Invoke();
    }
}
