using System;
using Unity.Cinemachine;
using UnityEngine;

public class StructureHealth : HealthSystem
{
    private bool structureActive = false;
    private bool laneStructure = false;
    private float health;
    private float maxHealth;
    private float impulseStrength = 0.1f;

    [SerializeField]
    private SFXObject structureBuiltSFX;

    [SerializeField]
    private SFXObject structureDamagedSFX;

    [SerializeField]
    private SFXObject structureDestroyedSFX;

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
            DestroyStructure();
        }
        else
        {
            AudioManager.PlaySFX(structureDamagedSFX, transform.position);
        }
    }

    protected virtual void DestroyStructure()
    {
        AudioManager.PlaySFX(structureDestroyedSFX, transform.position);
        structureActive = false;
        OnStructureDestroyed?.Invoke();
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

    public void ActivateStructure(bool laneStructure)
    {
        structureActive = true;
        this.laneStructure = laneStructure;
        AudioManager.PlaySFX(structureBuiltSFX, transform.position);
    }

    public bool IsStructureActive()
    {
        return structureActive;
    }

    public bool GetIsLaneStructure()
    {
        return laneStructure;
    }
}
