using UnityEngine;

public class LaneRoute : MonoBehaviour
{
    [SerializeField]
    private Transform[] routeWaypoints;

    public Transform[] GetRouteWaypoints()
    {
        return routeWaypoints;
    }
}
