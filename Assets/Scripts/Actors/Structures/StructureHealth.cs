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
    private GameObject healthBar;

    [SerializeField]
    private Transform healthFill;

    [SerializeField]
    private ParticleSystem buildFX;

    [SerializeField]
    private ParticleSystem destroyFX;

    [SerializeField]
    private SFXObject structureBuiltSFX;

    [SerializeField]
    private SFXObject structureDamagedSFX;

    [SerializeField]
    private SFXObject structureDestroyedSFX;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    public Action OnStructureDestroyed;

    private void Start()
    {
        healthBar.SetActive(true);
        healthFill.localScale = new Vector3(1f, 0f, 1f);
    }

    public override void TakeDamage(float damageAmount)
    {
        if (!structureActive)
        {
            return;
        }

        impulseSource.GenerateImpulse(impulseStrength);

        health = Mathf.Max(0f, health - damageAmount);

        healthFill.localScale = new Vector3(1f, health / maxHealth, 1f);

        healthBar.SetActive(true);

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
        destroyFX.Play();
        structureActive = false;
        OnStructureDestroyed?.Invoke();
    }

    public void SetBuildHealthbar(float buildAmount)
    {
        healthFill.localScale = new Vector3(1f, buildAmount, 1f);

        buildFX.Play();

        if (buildAmount >= 1f)
        {
            healthBar.SetActive(false);
        }
    }

    public void HealDamage(float healAmount)
    {
        if (!structureActive)
        {
            return;
        }

        buildFX.Play();

        health = Mathf.Min(maxHealth, health + healAmount);

        if (health >= maxHealth)
        {
            healthBar.SetActive(false);
        }
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
