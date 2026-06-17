using UnityEngine;

public class LaneRoute : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private Transform[] routeWaypoints;

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public Transform[] GetRouteWaypoints()
    {
        return routeWaypoints;
    }
}
