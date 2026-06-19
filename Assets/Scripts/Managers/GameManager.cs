using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool gameOver = false;
    private int roundIndex = -1;
    private int winRoundIndex = 5;
    private float roundStartDelay = 3f;

    [SerializeField]
    private WaveManager waveManager;

    [SerializeField]
    private RoundWaves[] roundWaves;

    public static EventHandler<int> OnNewRoundStart;
    public static EventHandler<int> OnRoundEnd;
    public static EventHandler OnTutorialStart;
    public static Action OnDebuffTowers;
    public static Action OnGameWin;

    void Start()
    {
        CityStronghold.OnGameOver += ToggleGameOver;
    }

    private void OnDisable()
    {
        CityStronghold.OnGameOver -= ToggleGameOver;

        StopAllCoroutines();
    }

    private IEnumerator DelayedRoundStart()
    {
        yield return new WaitForSeconds(roundStartDelay);
        waveManager.StartRound(roundWaves[roundIndex]);
    }

    public void TutorialFinished()
    {
        waveManager.StartRound(roundWaves[roundIndex]);
    }

    public void StartNextRound()
    {
        roundIndex++;

        if (roundIndex == 0)
        {
            InputManager.Instance.GameStart();
            OnNewRoundStart?.Invoke(this, roundIndex);
            OnTutorialStart?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (roundIndex == 3)
        {
            OnDebuffTowers?.Invoke();
        }

        InputManager.Instance.GameStart();
        OnNewRoundStart?.Invoke(this, roundIndex);
        StartCoroutine(DelayedRoundStart());
    }

    public void EndRound()
    {
        if (gameOver)
        {
            return;
        }

        InputManager.Instance.GamePause();
        OnRoundEnd?.Invoke(this, roundIndex);

        if (roundIndex >= winRoundIndex)
        {
            OnGameWin?.Invoke();
            return;
        }
    }

    private void ToggleGameOver()
    {
        gameOver = true;

        InputManager.Instance.GamePause();
    }
}
