using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerActive = false;

    private PlayerStats playerStats;
    private PlayerMovement playerMovement;
    private PlayerAttacker playerAttacker;
    private PlayerBuilder playerBuilder;

    [SerializeField]
    private Animator playerAnimator;

    public static EventHandler<bool> OnPlayerDead;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttacker = GetComponent<PlayerAttacker>();
        playerBuilder = GetComponent<PlayerBuilder>();

        playerBuilder.OnSuccessfulBuild += CloseBuildMenu;
    }

    private void Start()
    {
        //Temp for testing
        TogglePlayer(true);

        InputManager.Instance.OnBuildEvent += ToggleBuilding;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnBuildEvent -= ToggleBuilding;
        playerBuilder.OnSuccessfulBuild -= CloseBuildMenu;
    }

    private void ToggleBuilding()
    {
        if (!playerActive)
        {
            return;
        }

        if (playerBuilder.IsBuilding())
        {
            playerBuilder.ToggleBuildMode(false);
            playerAttacker.ToggleCanAttack(true);
        }
        else
        {
            playerBuilder.ToggleBuildMode(true);
            playerAttacker.ToggleCanAttack(false);
        }
    }

    private void CloseBuildMenu()
    {
        playerBuilder.ToggleBuildMode(false);
        playerAttacker.ToggleCanAttack(true);
    }

    private void TogglePlayer(bool toggle)
    {
        playerMovement.ToggleCanMove(toggle);
        playerAttacker.ToggleCanAttack(toggle);
        playerBuilder.ToggleBuildMode(false);

        playerActive = toggle;
    }

    public void KillPlayer()
    {
        TogglePlayer(false);

        playerAnimator.SetTrigger("die");
        OnPlayerDead?.Invoke(this, true);
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}
