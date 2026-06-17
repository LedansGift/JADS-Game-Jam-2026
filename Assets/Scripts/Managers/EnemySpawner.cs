using System;
using UnityEngine;

public enum EnemyType
{
    fastLane,
    slowLane,
    roamer,
    supplyTrain
}

public class EnemySpawner : MonoBehaviour
{
    private bool spawnerActive = false;
    private bool spawningStatus = false;

    private int fastLaneIndex = 0;
    private int slowLaneIndex = 0;
    private int crusherIndex = 0;

    private int aliveEnemies = 0;

    private RouteManager routeManager;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private TrainManager trainManager;

    [SerializeField]
    private EnemyController[] fastLaneEnemies;

    [SerializeField]
    private EnemyController[] slowLanesEnemies;

    [SerializeField]
    private EnemyController[] crusherEnemies;

    private void Start()
    {
        routeManager = RouteManager.Instance;
    }

    private void OnEnable()
    {
        EnemyController.OnEnemyDead += ReduceEnemyCount;
    }

    private void OnDisable()
    {
        EnemyController.OnEnemyDead -= ReduceEnemyCount;
    }

    public void SpawnEnemy(EnemyType enemyType, int laneIndex = 0)
    {
        aliveEnemies++;

        switch (enemyType)
        {
            case EnemyType.fastLane:
                EnemyController fastEnemy = fastLaneEnemies[fastLaneIndex];
                fastLaneIndex++;
                if (fastLaneIndex >= fastLaneEnemies.Length)
                {
                    fastLaneIndex = 0;
                }
                SpawnLaneEnemy(fastEnemy, laneIndex);
                break;
            case EnemyType.slowLane:
                EnemyController slowEnemy = slowLanesEnemies[slowLaneIndex];
                slowLaneIndex++;
                if (slowLaneIndex >= slowLanesEnemies.Length)
                {
                    slowLaneIndex = 0;
                }
                SpawnLaneEnemy(slowEnemy, laneIndex);
                break;
            case EnemyType.roamer:
                EnemyController crusherEnemy = crusherEnemies[crusherIndex];
                crusherIndex++;
                if (crusherIndex >= crusherEnemies.Length)
                {
                    crusherIndex = 0;
                }
                SpawnRoamingEnemy(crusherEnemy, laneIndex);
                break;
            case EnemyType.supplyTrain:
                aliveEnemies--;
                trainManager.SpawnTrain(laneIndex);
                break;
        }
    }

    private void SpawnLaneEnemy(EnemyController enemy, int laneIndex)
    {
        EnemyLaneMovement laneMovement = enemy.GetComponent<EnemyLaneMovement>();
        LaneRoute route = routeManager.GetLaneRoute(laneIndex);

        laneMovement.SetEnemyRoute(route.GetRouteWaypoints());
        laneMovement.SpawnEnemyAtLocation(route.GetSpawnPoint().position);
        enemy.SpawnEnemy();
    }

    private void SpawnRoamingEnemy(EnemyController enemy, int laneIndex)
    {
        EnemyFreeMovement freeMovement = enemy.GetComponent<EnemyFreeMovement>();
        Transform spawnPoint = routeManager.GetRoamerSpawn(laneIndex);

        freeMovement.SetEnemyRoute(routeManager.GetStrongholdPosition());
        freeMovement.SpawnEnemyAtLocation(spawnPoint.position);

        enemy.SpawnEnemy();
    }

    private void ReduceEnemyCount()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);

        Debug.Log("Alive Enemies: " + aliveEnemies);

        if (!spawningStatus)
        {
            TryEndRound();
        }
    }

    public void SetSpawnerActive(bool active)
    {
        spawnerActive = active;
    }

    public void SetSpawningStatus(bool status)
    {
        spawningStatus = status;
    }

    public void TryEndRound()
    {
        if (!spawnerActive)
        {
            return;
        }

        if (aliveEnemies <= 0)
        {
            SetSpawnerActive(false);
            gameManager.EndRound();
        }
    }
}
