using System;
using Unity.Cinemachine;
using UnityEngine;

public class CityStronghold : MonoBehaviour
{
    private bool gameOver = false;

    private float strongholdHealth;
    private float strongholdMaxHealth = 250f;

    [SerializeField]
    private Transform healthBar;

    [SerializeField]
    private ParticleSystem damageFX;

    [SerializeField]
    private CinemachineImpulseSource damageImpulse;

    [SerializeField]
    private SFXObject damageSFX;

    public static Action OnGameOver;

    private void Start()
    {
        strongholdHealth = strongholdMaxHealth;

        healthBar.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOver)
        {
            return;
        }

        if (other.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(9999f);
            TakeStrongholdDamage(enemyHealth.GetStrongholdDamage());
        }
    }

    private void TakeStrongholdDamage(float damage)
    {
        strongholdHealth -= damage;

        healthBar.localScale = new Vector3(1f, strongholdHealth / strongholdMaxHealth, 1f);

        //Debug.Log("Stronghold damaged. Health remaining: " + strongholdHealth);

        //Play damage fx
        damageFX.Play();

        AudioManager.PlaySFX(damageSFX, transform.position);

        damageImpulse.GenerateImpulse();

        if (strongholdHealth <= 0f)
        {
            OnGameOver?.Invoke();
            gameOver = true;
        }
    }
}
