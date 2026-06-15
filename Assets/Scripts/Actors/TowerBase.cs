using UnityEngine;
using Managers;

namespace Actors
{
    public class TowerBase : MonoBehaviour
    {
        // These are starting stat that applies to all Towers.
        [Header("BaseStats")]
        public int scrapCost;
        public float baseHealth;
        public float baseDamage;
        public float baseRange;
        public float baseFireRate;
        public DamageType damageType;
        public float baseDamageSubStat;
    
        // These are values to be adjusted in-game.
        [Header("VariableStats")]
        public float scrapMultiplier = 0.5f;
        public float healthMultiplier = 1.0f;
        public float repairMultiplier = 0.1f;
        public float damageMultiplier = 1.0f;
        public float vulnerabilityMultiplier = 1.0f;
        public float rangeMultiplier = 1.0f;
        public float fireRateDelta = 1.0f;
        public float subStatsDelta = 1.0f;
        public float currentHealth = 0f;
    
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        private void OnTriggerEnter(Collider other)
        {
            // First, Check whether it needs to process damage.
            if (other.CompareTag("Projectile"))
            {
            
            } else if (other.CompareTag("EnemyUnit"))
            {
            
            } else if (other.CompareTag("Player"))
            {
            
            }
        }

        private void OnDamageTaken()
        {
            // Handle enemy damage type effect.
        }
    }
}
