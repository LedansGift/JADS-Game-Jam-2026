using UnityEngine;

public class TrainManager : MonoBehaviour
{
    private float trainSpeed = 5f;

    [SerializeField]
    private TrainMovement northTrain;

    [SerializeField]
    private TrainMovement southTrain;

    public void SpawnTrain(int laneIndex)
    {
        if (laneIndex == 0)
        {
            northTrain.ActivateTrain(trainSpeed);
        }
        else
        {
            southTrain.ActivateTrain(trainSpeed);
        }
    }
}
