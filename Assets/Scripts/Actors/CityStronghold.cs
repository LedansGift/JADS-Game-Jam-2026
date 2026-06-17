using System;
using UnityEngine;

public class CityStronghold : MonoBehaviour
{
    private bool gameOver = false;

    private float strongholdHealth = 250f;

    public static Action OnGameOver;

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

        Debug.Log("Stronghold damaged. Health remaining: " + strongholdHealth);

        //Play damage fx

        if (strongholdHealth <= 0f)
        {
            OnGameOver?.Invoke();
            gameOver = true;
        }
    }
}
