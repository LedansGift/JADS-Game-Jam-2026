using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer = false;

    public abstract void TakeDamage(float damageAmount);

    public bool GetIsPlayer()
    {
        return isPlayer;
    }
}
