using System;
using Unity.Cinemachine;
using UnityEngine;

public class StructureHealth : HealthSystem
{
    private bool structureActive = false;

    private float health;
    private float maxHealth;
    private float impulseStrength = 0.5f;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    public Action OnStructureDestroyed;

    public override void TakeDamage(float damageAmount)
    {
        if (!structureActive)
        {
            return;
        }

        impulseSource.GenerateImpulse(impulseStrength);

        health = Mathf.Max(0f, health - damageAmount);

        if (health <= 0f)
        {
            //Destroy structure
            structureActive = false;
            OnStructureDestroyed?.Invoke();
        }
        else
        {
            //Damage structure
        }
    }

    public void HealDamage(float healAmount)
    {
        if (!structureActive)
        {
            return;
        }

        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public bool IsMaxHealth()
    {
        return health >= maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }

    public void ActivateStructure()
    {
        structureActive = true;
    }
}
