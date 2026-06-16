using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int roundIndex = 0;
    private float roundStartDelay = 3f;

    [SerializeField]
    private WaveManager waveManager;

    [SerializeField]
    private RoundWaves[] roundWaves;

    public static EventHandler<int> OnNewRoundStart;
    public static EventHandler<int> OnRoundEnd;

    void Start()
    {
        //Temp debug start
        StartGame();
    }

    private void StartGame()
    {
        InputManager.Instance.GameStart();
        OnNewRoundStart?.Invoke(this, roundIndex);
        StartCoroutine(DelayedRoundStart());
    }

    private IEnumerator DelayedRoundStart()
    {
        yield return new WaitForSeconds(roundStartDelay);
        waveManager.StartRound(roundWaves[roundIndex]);
    }

    public void StartNextRound()
    {
        roundIndex++;
        OnNewRoundStart?.Invoke(this, roundIndex);
        StartCoroutine(DelayedRoundStart());
    }

    public void EndRound()
    {
        InputManager.Instance.GamePause();
        OnRoundEnd?.Invoke(this, roundIndex);
    }
}
