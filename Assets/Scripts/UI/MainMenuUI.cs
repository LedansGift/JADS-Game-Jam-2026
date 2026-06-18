using System.Collections;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuAssets;

    [SerializeField]
    private CanvasGroupFader mainMenuFader;

    [SerializeField]
    private CanvasGroupFader cutsceneFader;

    [SerializeField]
    private CanvasGroupFader cutsceneTextFader;

    [SerializeField]
    private CanvasGroupFader cutsceneContinueFader;

    [SerializeField]
    private SFXObject buttonClickSFX;

    private void Awake()
    {
        mainMenuFader.SetCanvasGroupAlpha(1f);
        mainMenuFader.ToggleBlockRaycasts(true);
    }

    public void StartGame()
    {
        AudioManager.PlaySFX(buttonClickSFX, transform.position);

        StartCoroutine(RevealCutscene());
    }

    private IEnumerator RevealCutscene()
    {
        cutsceneFader.ToggleFade(true);
        cutsceneFader.ToggleBlockRaycasts(true);

        yield return new WaitForSeconds(3.5f);

        cutsceneTextFader.ToggleFade(true);

        yield return new WaitForSeconds(2.5f);

        cutsceneContinueFader.ToggleFade(true);
        cutsceneContinueFader.ToggleBlockRaycasts(true);
    }

    public void FinishIntroCutscene()
    {
        AudioManager.PlaySFX(buttonClickSFX, transform.position);
        mainMenuAssets.SetActive(false);
        mainMenuFader.ToggleFade(false);
        mainMenuFader.ToggleBlockRaycasts(false);
    }
}
