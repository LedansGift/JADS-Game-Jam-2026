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
