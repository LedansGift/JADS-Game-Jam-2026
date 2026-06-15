using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    private float timeBetweenSpawns = 0.2f;

    [SerializeField]
    private EnemySpawner enemySpawner;

    public Action OnWavesFinishedSpawning;

    private IEnumerator EnemySpawnCoroutine(EnemyWave enemyWave)
    {
        for (int i = 0; i < enemyWave.enemyNumber; i++)
        {
            enemySpawner.SpawnEnemy(enemyWave.enemyType, enemyWave.spawnRoute);

            yield return new WaitForSeconds(timeBetweenSpawns);
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
            yield return null;
            OnWavesFinishedSpawning?.Invoke();
        }
    }

    public void StartRound(RoundWaves roundWaves)
    {
        StartCoroutine(WaveCoroutine(roundWaves, 0));
    }
}
