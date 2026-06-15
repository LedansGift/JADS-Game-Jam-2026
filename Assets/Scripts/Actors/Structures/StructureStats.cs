using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Stats", menuName = "Stats/Structure Stats", order = 1)]
public class StructureStats : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float attackRange;
    public float attackFrequency;
    public int scrapCost;
}
