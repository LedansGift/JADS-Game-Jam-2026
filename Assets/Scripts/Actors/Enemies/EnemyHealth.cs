using System;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    protected bool enemyActive = false;

    private float health;
    private float maxHealth;

    [SerializeField]
    private SFXObject enemyDamageSFX;

    [SerializeField]
    private SFXObject enemyDeathSFX;

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
            AudioManager.PlaySFX(enemyDeathSFX, transform.position);
            OnEnemyDead?.Invoke();
        }
        else
        {
            AudioManager.PlaySFX(enemyDamageSFX, transform.position);
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
