using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RoundStartUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roundText;

    [SerializeField]
    private CanvasGroupFader fader;

    private void Start()
    {
        fader.SetCanvasGroupAlpha(0f);
        GameManager.OnNewRoundStart += DisplayRoundStartUI;
    }

    private void OnDisable()
    {
        GameManager.OnNewRoundStart -= DisplayRoundStartUI;
    }

    private IEnumerator DisplayCoroutine(int newRoundNumber)
    {
        newRoundNumber++;
        roundText.text = "Day " + newRoundNumber.ToString();

        fader.ToggleFade(true);
        yield return new WaitForSeconds(2.5f);
        fader.ToggleFade(false);
    }

    private void DisplayRoundStartUI(object sender, int newRoundNumber)
    {
        StartCoroutine(DisplayCoroutine(newRoundNumber));
    }
}
