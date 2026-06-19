using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    [SerializeField]
    private CanvasGroupFader endStoryFader;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private GameObject gameWinScreen;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);

        gameWinScreen.SetActive(false);

        endStoryFader.SetCanvasGroupAlpha(0f);
        endStoryFader.ToggleBlockRaycasts(false);
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

        StopAllCoroutines();
    }

    private void ToggleGameWinScreen()
    {
        gameWinScreen.SetActive(true);
        gameOverScreen.SetActive(false);

        StartCoroutine(FadeInStory());
    }

    private IEnumerator FadeInStory()
    {
        yield return new WaitForSeconds(3f);

        ToggleGameOverScreen();

        yield return new WaitForSeconds(3f);

        endStoryFader.ToggleFade(true);
        endStoryFader.ToggleBlockRaycasts(true);
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
