using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private GameObject gameWinScreen;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);

        gameWinScreen.SetActive(false);
    }

    private void Start()
    {
        CityStronghold.OnGameOver += ToggleGameOverScreen;
        GameManager.OnGameWin += ToggleGameWinScreen;
    }

    private void OnDisable()
    {
        CityStronghold.OnGameOver -= ToggleGameOverScreen;
        GameManager.OnGameWin -= ToggleGameWinScreen;
    }

    private void ToggleGameWinScreen()
    {
        gameWinScreen.SetActive(true);
        ToggleGameOverScreen();
    }

    private void ToggleGameOverScreen()
    {
        fader.ToggleFade(true);
        fader.ToggleBlockRaycasts(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
