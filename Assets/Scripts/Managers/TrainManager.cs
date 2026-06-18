using UnityEngine;

public class TrainManager : MonoBehaviour
{
    private float northTrainDampTime = 3.5f;
    private float southTrainDampTime = 10f;

    [SerializeField]
    private TrainMovement northTrain;

    [SerializeField]
    private TrainMovement southTrain;

    public void SpawnTrain(int laneIndex)
    {
        if (laneIndex == 0)
        {
            northTrain.ActivateTrain(northTrainDampTime);
        }
        else
        {
            southTrain.ActivateTrain(southTrainDampTime);
        }
    }
}
