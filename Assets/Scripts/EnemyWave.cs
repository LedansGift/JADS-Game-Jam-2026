using System;
using UnityEngine;

[Serializable]
public class EnemyWave
{
    public EnemyType enemyType;
    public int enemyNumber;

    [Range(0, 2)]
    public int spawnRoute;
    public float timeBetweenSpawns = 0.5f;
    public float delayToNextWave;
}
