using UnityEngine;

public class ScrapEnemyHealth : EnemyHealth
{
    private int scrapDropPerHit = 10;

    // [SerializeField]
    // private ParticleSystem scrapDropEffect;

    public int DropScrap()
    {
        if (!enemyActive)
        {
            return 0;
        }

        //scrapDropEffect.Play();

        return scrapDropPerHit;
    }
}
