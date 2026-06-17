using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Stats/Enemy Stats", order = 0)]
public class EnemyStats : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float strongholdDamage;
    public float attackFrequency;
    public float movementSpeed;
}
