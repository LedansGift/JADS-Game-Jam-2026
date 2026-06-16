using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private WaveManager waveManager;

    [SerializeField]
    private RoundWaves firstWaves;

    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        InputManager.Instance.GameStart();
        waveManager.StartRound(firstWaves);
    }
}
