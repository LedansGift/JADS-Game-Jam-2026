using System;
using UnityEngine;

public enum EnemyType
{
    fastLane,
    slowLane,
    roamer
}

public class EnemySpawner : MonoBehaviour
{
    private int fastLaneIndex = 0;
    private int slowLaneIndex = 0;

    private int aliveEnemies = 0;

    private RouteManager routeManager;

    [SerializeField]
    private EnemyController[] fastLanes;

    [SerializeField]
    private EnemyController[] slowLanes;

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

    public void SpawnEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.fastLane:
                EnemyController fastEnemy = fastLanes[fastLaneIndex];
                fastLaneIndex++;
                if (fastLaneIndex >= fastLanes.Length)
                {
                    fastLaneIndex = 0;
                }
                SpawnLaneEnemy(fastEnemy);
                break;
            case EnemyType.slowLane:
                EnemyController slowEnemy = slowLanes[slowLaneIndex];
                slowLaneIndex++;
                if (slowLaneIndex >= slowLanes.Length)
                {
                    slowLaneIndex = 0;
                }
                SpawnLaneEnemy(slowEnemy);
                break;
            case EnemyType.roamer:

                break;
        }
    }

    private void SpawnLaneEnemy(EnemyController enemy) { }

    private void SpawnRoamingEnemy(EnemyController enemy) { }

    private void ReduceEnemyCount()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
    }
}
