using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    private bool invincible = false;
    private float health;
    private float invincibilityDuration = 2f;
    private float impulseStrength = 1f;
    private PlayerManager playerManager;

    [SerializeField]
    private GameObject playerVisual;

    [SerializeField]
    private SFXObject playerDamageSFX;

    [SerializeField]
    private SFXObject playerDeathSFX;

    [SerializeField]
    private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        SetMaxHealth();
    }

    private IEnumerator DamageInvincibility()
    {
        invincible = true;

        float invincibilityFlash = invincibilityDuration / 4f;

        for (int i = 0; i < 8; i++)
        {
            if ((i % 2) == 0)
            {
                //playerVisual.SetActive(false);
                yield return new WaitForSeconds(invincibilityFlash * 0.25f);
            }
            else
            {
                //playerVisual.SetActive(true);
                yield return new WaitForSeconds(invincibilityFlash * 0.75f);
            }
        }

        invincible = false;
    }

    private void SetMaxHealth()
    {
        health = playerManager.GetPlayerStats().GetMaxHealth();
    }

    public override void TakeDamage(float damageAmount)
    {
        if (invincible)
        {
            return;
        }

        impulseSource.GenerateImpulse(impulseStrength);

        health = Mathf.Max(0f, health - damageAmount);

        if (health <= 0f)
        {
            AudioManager.PlaySFX(playerDeathSFX, transform.position);
            playerManager.KillPlayer();
        }
        else
        {
            AudioManager.PlaySFX(playerDamageSFX, transform.position);
            StartCoroutine(DamageInvincibility());
        }
    }
}
