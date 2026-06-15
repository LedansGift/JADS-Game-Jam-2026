using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Round", menuName = "Round Waves", order = 0)]
public class RoundWaves : ScriptableObject
{
    [SerializeField]
    private List<EnemyWave> enemyWaves;

    public List<EnemyWave> GetEnemyWaves()
    {
        return enemyWaves;
    }
}
