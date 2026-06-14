using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Stats", menuName = "Structure Stats", order = 0)]
public class StructureStats : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float attackRange;
    public float attackFrequency;
    public int scrapCost;
}
