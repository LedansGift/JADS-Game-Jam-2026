using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;

    [SerializeField]
    private LaneRoute[] westRoutes;

    [SerializeField]
    private LaneRoute[] northRoutes;

    [SerializeField]
    private LaneRoute[] eastRoutes;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public LaneRoute GetLaneRoute(int laneIndex)
    {
        LaneRoute[] routes;

        switch (laneIndex)
        {
            case 0:
                routes = westRoutes;
                break;
            case 1:
                routes = northRoutes;
                break;
            case 2:
                routes = eastRoutes;
                break;
            default:
                routes = westRoutes;
                Debug.Log("Invalid Route");
                break;
        }

        int randomPath = Random.Range(0, routes.Length);
        return routes[randomPath];
    }
}
