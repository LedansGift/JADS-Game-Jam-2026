using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroupFader fader;

    private void Awake()
    {
        fader.SetCanvasGroupAlpha(0f);
        fader.ToggleBlockRaycasts(false);
    }

    private void Start()
    {
        CityStronghold.OnGameOver += ToggleGameOverScreen;
    }

    private void OnDisable()
    {
        CityStronghold.OnGameOver -= ToggleGameOverScreen;
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
