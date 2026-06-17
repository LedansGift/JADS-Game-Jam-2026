using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;

    [SerializeField]
    private float maxHealth = 100f;

    [Header("Attack")]
    [SerializeField]
    private float attackDamage = 1f;

    [SerializeField]
    private float attackMinChargeDamage = 10f;

    [SerializeField]
    private float attackMaxChargeDamage = 50f;

    [SerializeField]
    private float repairDamage = 25f;

    [SerializeField]
    private int repairCost = 10;

    [SerializeField]
    private float attackRange = 2.5f;

    [SerializeField]
    private float maxChargeAttackRange = 3f;

    [SerializeField]
    private float repairRange = 4f;

    [SerializeField]
    private float attackSpeed = 1f;

    [Header("Dash")]
    [SerializeField]
    private float dashRechargeTime = 1f;

    [SerializeField]
    private float dashSpeedModifier = 3f;

    [SerializeField]
    private float dashingTime = 0.4f;

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetAttackDamage()
    {
        return attackDamage;
    }

    public float GetMinChargeAttackDamage()
    {
        return attackMinChargeDamage;
    }

    public float GetMaxChargeAttackDamage()
    {
        return attackMaxChargeDamage;
    }

    public float GetRepairDamage()
    {
        return repairDamage;
    }

    public int GetRepairCost()
    {
        return repairCost;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public float GetMaxChargeAttackRange()
    {
        return maxChargeAttackRange;
    }

    public float GetRepairRange()
    {
        return repairRange;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetDashRechargeTime()
    {
        return dashRechargeTime;
    }

    public float GetDashSpeedModifier()
    {
        return dashSpeedModifier;
    }

    public float GetDashTime()
    {
        return dashingTime;
    }
}
