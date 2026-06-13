using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        InputManager.Instance.GameStart();
    }
}
