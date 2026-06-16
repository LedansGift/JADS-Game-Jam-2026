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
    private PlayerStats playerStats;

    [SerializeField]
    private Transform attackTransform;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private LayerMask attackLayerMask;

    private Coroutine attackCoroutine;

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
        }
    }

    private void WeaponAttack()
    {
        if (!canAttack || isBusy)
        {
            return;
        }

        playerAnimator.SetTrigger("attack");

        attackHeld = true;

        attackCoroutine = StartCoroutine(WrenchSlash());
    }

    private void WeaponAttackRelease()
    {
        if (!canAttack)
        {
            return;
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

        attackCoroutine = StartCoroutine(WrenchRepair());
    }

    private IEnumerator WrenchSlash()
    {
        isBusy = true;

        yield return new WaitForSeconds(attackHitDelay);

        if (attackHeld)
        {
            playerAnimator.SetTrigger("chargeAttackReady");
            chargeTime = 0f;
            charging = true;
            yield break;
        }

        HitEnemies(playerStats.GetAttackDamage(), playerStats.GetAttackRange());

        yield return new WaitForSeconds(playerStats.GetAttackSpeed() - attackHitDelay);

        isBusy = false;
    }

    private IEnumerator WrenchChargeSlash()
    {
        chargeTime = Mathf.Min(chargeTime, maxChargeDuration);

        float attackDamage = Mathf.Lerp(
            playerStats.GetMinChargeAttackDamage(),
            playerStats.GetMaxChargeAttackDamage(),
            chargeTime / maxChargeDuration
        );

        if (chargeTime >= maxChargeDuration)
        {
            playerAnimator.SetTrigger("chargeAttackStrong");
        }
        else
        {
            playerAnimator.SetTrigger("chargeAttackWeak");
        }

        HitEnemies(attackDamage, playerStats.GetAttackRange());

        yield return new WaitForSeconds(playerStats.GetAttackSpeed() - attackHitDelay);

        isBusy = false;
    }

    private IEnumerator WrenchRepair()
    {
        isBusy = true;

        RepairStructures(playerStats.GetRepairDamage(), playerStats.GetRepairRange());

        yield return new WaitForSeconds(playerStats.GetAttackSpeed());

        isBusy = false;
    }

    private void HitEnemies(float attackDamage, float attackRange)
    {
        HealthSystem[] hitObjects = GetHitObjects(attackRange, false);

        foreach (HealthSystem health in hitObjects)
        {
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
                structureHealth.HealDamage(repairAmount);
                // Use up scrap
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
