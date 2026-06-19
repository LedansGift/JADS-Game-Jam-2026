using UnityEngine;

public class RouteManager : MonoBehaviour
{
    public static RouteManager Instance;

    [SerializeField]
    private Transform strongholdPosition;

    [SerializeField]
    private Transform bossSpawn;

    [SerializeField]
    private Transform[] roamerSpawns;

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

    public Transform GetStrongholdPosition()
    {
        return strongholdPosition;
    }

    public Transform GetBossSpawn()
    {
        return bossSpawn;
    }

    public Transform GetRoamerSpawn(int laneIndex)
    {
        if ((laneIndex < 0) || (laneIndex >= roamerSpawns.Length))
        {
            return transform;
        }

        return roamerSpawns[laneIndex];
    }
}
