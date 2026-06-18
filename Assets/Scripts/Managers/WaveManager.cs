using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner;

    //public Action OnWavesFinishedSpawning;

    private IEnumerator EnemySpawnCoroutine(EnemyWave enemyWave)
    {
        for (int i = 0; i < enemyWave.enemyNumber; i++)
        {
            enemySpawner.SpawnEnemy(enemyWave.enemyType, enemyWave.spawnRoute);

            yield return new WaitForSeconds(enemyWave.timeBetweenSpawns);
        }
    }

    private IEnumerator WaveCoroutine(RoundWaves roundWaves, int waveIndex)
    {
        EnemyWave currentWave = roundWaves.GetEnemyWaves()[waveIndex];

        StartCoroutine(EnemySpawnCoroutine(currentWave));

        yield return new WaitForSeconds(currentWave.delayToNextWave);

        waveIndex++;

        if (waveIndex < roundWaves.GetEnemyWaves().Count)
        {
            StartCoroutine(WaveCoroutine(roundWaves, waveIndex));
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            enemySpawner.SetSpawningStatus(false);
            enemySpawner.TryEndRound();
        }
    }

    public void StartRound(RoundWaves roundWaves)
    {
        enemySpawner.SetSpawningStatus(true);
        enemySpawner.SetSpawnerActive(true);

        StartCoroutine(WaveCoroutine(roundWaves, 0));
    }
}
