using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerStats playerStats;

    [SerializeField]
    private Animator playerAnimator;

    public static EventHandler<bool> OnPlayerDead;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable() { }

    private void OnDisable() { }

    public void KillPlayer()
    {
        OnPlayerDead?.Invoke(this, true);
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}
