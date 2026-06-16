using System;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    private bool enemyActive = false;

    private float health;
    private float maxHealth;

    public Action OnEnemyDamaged;
    public Action OnEnemyDead;

    public override void TakeDamage(float damageAmount)
    {
        if (!enemyActive)
        {
            return;
        }

        health = Mathf.Max(0f, health - damageAmount);

        if (health <= 0f)
        {
            enemyActive = false;
            OnEnemyDead?.Invoke();
        }
        else
        {
            OnEnemyDamaged?.Invoke();
        }
    }

    public bool IsEnemyAlive()
    {
        return enemyActive;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void ReviveEnemy()
    {
        health = maxHealth;
        enemyActive = true;
    }
}
