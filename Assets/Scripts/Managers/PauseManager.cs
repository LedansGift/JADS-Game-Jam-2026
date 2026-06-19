using System;
using System.Collections;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool pauseCooldown = false;
    private bool pauseActive = false;

    [SerializeField]
    private CanvasGroupFader pauseMenuScreen;

    public static EventHandler<bool> OnPauseGame;

    private void Awake()
    {
        pauseMenuScreen.SetCanvasGroupAlpha(0f);
        pauseMenuScreen.ToggleBlockRaycasts(false);
        pauseActive = false;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        InputManager.Instance.OnMenuEvent += TogglePause;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnMenuEvent -= TogglePause;

        StopAllCoroutines();
    }

    private IEnumerator PauseCD()
    {
        pauseCooldown = true;
        yield return new WaitForSecondsRealtime(0.5f);
        pauseCooldown = false;
    }

    public void TogglePause()
    {
        if (pauseCooldown)
        {
            return;
        }

        StartCoroutine(PauseCD());

        pauseActive = !pauseActive;

        pauseMenuScreen.ToggleFade(pauseActive);

        pauseMenuScreen.ToggleBlockRaycasts(pauseActive);

        if (pauseActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        OnPauseGame?.Invoke(this, pauseActive);
    }

    // private void EndOfGamePause(object sender, bool lightsOut)
    // {
    //     pauseActive = true;
    //     Time.timeScale = 0f;
    // }
}
