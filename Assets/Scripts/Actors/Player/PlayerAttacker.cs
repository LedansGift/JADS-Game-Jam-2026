using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private bool attackHeld = false;
    private bool charging = false;
    private bool isBusy = false;
    private bool canAttack = false;

    private float chargeTime = 0f;
    private float maxChargeDuration = 3f;

    private float attackHitDelay = 0.15f;
    private float movementBeginOffset = 0.25f;
    private PlayerStats playerStats;

    [SerializeField]
    private Transform attackTransform;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private LayerMask attackLayerMask;

    [SerializeField]
    private SFXObject wrenchSwingSFX;

    [SerializeField]
    private SFXObject wrenchCharge1SFX;

    [SerializeField]
    private SFXObject wrenchCharge2SFX;

    [SerializeField]
    private SFXObject wrenchChargeSlamSFX;

    [SerializeField]
    private SFXObject wrenchRepairSFX;

    [SerializeField]
    private SFXObject wrenchConstructSFX;

    private Coroutine chargeAttackCoroutine;

    public EventHandler<bool> OnPauseMovement;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        InputManager.Instance.OnAttackEvent += WeaponAttack;
        InputManager.Instance.OnRepairEvent += WeaponRepair;
        InputManager.Instance.OnAttackReleaseEvent += WeaponAttackRelease;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnAttackEvent -= WeaponAttack;
        InputManager.Instance.OnRepairEvent -= WeaponRepair;
        InputManager.Instance.OnAttackReleaseEvent -= WeaponAttackRelease;
    }

    private void Update()
    {
        if (charging)
        {
            chargeTime += Time.deltaTime;

            //play sfx and spawn particles at 2 charge levels
        }
    }

    private void WeaponAttack()
    {
        if (!canAttack || isBusy)
        {
            return;
        }

        playerAnimator.SetTrigger("attack");

        AudioManager.PlaySFX(wrenchSwingSFX, transform.position);

        attackHeld = true;

        StartCoroutine(WrenchSlash());
    }

    private void WeaponAttackRelease()
    {
        if (!canAttack)
        {
            return;
        }

        if (chargeAttackCoroutine != null)
        {
            StopCoroutine(chargeAttackCoroutine);
        }

        attackHeld = false;

        if (!charging)
        {
            return;
        }

        charging = false;

        StartCoroutine(WrenchChargeSlash());
    }

    private void WeaponRepair()
    {
        if (!canAttack || isBusy)
        {
            return;
        }

        playerAnimator.SetTrigger("build");

        StartCoroutine(WrenchRepair());
    }

    private IEnumerator WrenchSlash()
    {
        isBusy = true;

        OnPauseMovement?.Invoke(this, true);

        yield return new WaitForSeconds(attackHitDelay);

        if (attackHeld)
        {
            playerAnimator.SetTrigger("chargeAttackReady");
            chargeTime = 0f;
            charging = true;

            chargeAttackCoroutine = StartCoroutine(ChargingEffects());
            yield break;
        }

        HitEnemies(playerStats.GetAttackDamage(), playerStats.GetAttackRange());

        yield return new WaitForSeconds(
            playerStats.GetAttackSpeed() - attackHitDelay - movementBeginOffset
        );

        OnPauseMovement?.Invoke(this, false);

        yield return new WaitForSeconds(movementBeginOffset);

        isBusy = false;
    }

    private IEnumerator ChargingEffects()
    {
        yield return new WaitForSeconds(maxChargeDuration / 2f);
        AudioManager.PlaySFX(wrenchCharge1SFX, transform.position);

        yield return new WaitForSeconds(maxChargeDuration);
        AudioManager.PlaySFX(wrenchCharge2SFX, transform.position);
    }

    private IEnumerator WrenchChargeSlash()
    {
        chargeTime = Mathf.Min(chargeTime, maxChargeDuration);

        float attackDamage = Mathf.Lerp(
            playerStats.GetMinChargeAttackDamage(),
            playerStats.GetMaxChargeAttackDamage(),
            chargeTime / maxChargeDuration
        );

        float attackRange = Mathf.Lerp(
            playerStats.GetAttackRange(),
            playerStats.GetMaxChargeAttackRange(),
            chargeTime / maxChargeDuration
        );

        if (chargeTime >= maxChargeDuration)
        {
            playerAnimator.SetTrigger("chargeAttackStrong");
            AudioManager.PlaySFX(wrenchChargeSlamSFX, transform.position);
        }
        else
        {
            AudioManager.PlaySFX(wrenchSwingSFX, transform.position);
            playerAnimator.SetTrigger("chargeAttackWeak");
        }

        HitEnemies(attackDamage, attackRange);

        yield return new WaitForSeconds(
            playerStats.GetAttackSpeed() - attackHitDelay - movementBeginOffset
        );

        OnPauseMovement?.Invoke(this, false);

        yield return new WaitForSeconds(movementBeginOffset);

        isBusy = false;
    }

    private IEnumerator WrenchRepair()
    {
        isBusy = true;

        OnPauseMovement?.Invoke(this, true);

        RepairStructures(playerStats.GetRepairDamage(), playerStats.GetRepairRange());

        yield return new WaitForSeconds(playerStats.GetAttackSpeed() - movementBeginOffset);

        OnPauseMovement?.Invoke(this, false);

        yield return new WaitForSeconds(movementBeginOffset);

        isBusy = false;
    }

    private void HitEnemies(float attackDamage, float attackRange)
    {
        HealthSystem[] hitObjects = GetHitObjects(attackRange, false);

        foreach (HealthSystem health in hitObjects)
        {
            if (health.GetType() == typeof(ScrapEnemyHealth))
            {
                ScrapEnemyHealth scrapEnemyHealth = health as ScrapEnemyHealth;
                int scrapDropped = scrapEnemyHealth.DropScrap();
                ScrapManager.Instance.AddScrap(scrapDropped);
            }

            health.TakeDamage(attackDamage);
        }
    }

    private void RepairStructures(float repairAmount, float repairRange)
    {
        HealthSystem[] hitObjects = GetHitObjects(repairRange, true);

        // Build a structure if available
        bool structureBuild = false;

        foreach (HealthSystem hitObject in hitObjects)
        {
            if (hitObject.TryGetComponent<Structure>(out Structure hitStructure))
            {
                if (!hitStructure.StructureBuilt())
                {
                    hitStructure.BuildStructure();
                    structureBuild = true;

                    AudioManager.PlaySFX(wrenchConstructSFX, transform.position);
                    break;
                }
            }
        }

        if (structureBuild)
        {
            return;
        }

        foreach (HealthSystem hitObject in hitObjects)
        {
            if (hitObject.GetType() != typeof(StructureHealth))
            {
                continue;
            }

            StructureHealth structureHealth = hitObject as StructureHealth;

            if (!structureHealth.IsMaxHealth())
            {
                if (ScrapManager.Instance.IsEnoughAvailableScrap(playerStats.GetRepairCost()))
                {
                    ScrapManager.Instance.SpendScrap(playerStats.GetRepairCost());
                    structureHealth.HealDamage(repairAmount);
                    // Use up scrap

                    AudioManager.PlaySFX(wrenchRepairSFX, transform.position);
                }
                else
                {
                    //No scrap UI
                }

                break;
            }
        }
    }

    private HealthSystem[] GetHitObjects(float attackRange, bool playerObjects)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            attackTransform.position,
            attackRange,
            attackLayerMask
        );

        List<HealthSystem> healths = new List<HealthSystem>();

        foreach (Collider2D collider in colliders)
        {
            if (
                collider.TryGetComponent<HealthSystem>(out HealthSystem hitHealth)
                && (hitHealth.GetIsPlayer() == playerObjects)
            )
            {
                healths.Add(hitHealth);
            }
        }

        return healths.ToArray();
    }

    public void ToggleCanAttack(bool toggle)
    {
        canAttack = toggle;
    }
}
