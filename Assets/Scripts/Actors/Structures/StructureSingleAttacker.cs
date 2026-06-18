using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSingleAttacker : StructureAttacker
{
    [SerializeField]
    private ParticleSystem ambientParticles;

    [SerializeField]
    private LineRenderer lightningLine;

    private void Start()
    {
        lightningLine.SetPosition(0, lightningLine.transform.position);
        lightningLine.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        lightningLine.widthMultiplier = Mathf.Clamp01(1f - (attackTimer / stats.attackFrequency));
    }

    protected override List<EnemyHealth> PerformEnemyCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            stats.attackRange,
            enemyLayerMask
        );

        List<EnemyHealth> healths = new List<EnemyHealth>();

        //Debug.Log("Colliders Found: " + colliders.Length);

        foreach (Collider2D collider in colliders)
        {
            if (
                collider.TryGetComponent<EnemyHealth>(out EnemyHealth hitHealth)
                && hitHealth.IsEnemyAlive()
            )
            {
                healths.Add(hitHealth);
            }
        }

        if (healths.Count <= 0)
        {
            return healths;
        }

        EnemyHealth mostLeftEnemy = null;

        foreach (EnemyHealth health in healths)
        {
            if (!mostLeftEnemy)
            {
                mostLeftEnemy = health;
                continue;
            }

            float xValue = health.transform.position.x;

            if (xValue < mostLeftEnemy.transform.position.x)
            {
                mostLeftEnemy = health;
            }
        }

        attackFX.transform.position = mostLeftEnemy.transform.position;

        lightningLine.SetPosition(1, mostLeftEnemy.transform.position);

        lightningLine.gameObject.SetActive(true);
        //StartCoroutine(TurnOffLightning());

        List<EnemyHealth> returnHealth = new List<EnemyHealth> { mostLeftEnemy };
        return returnHealth;
    }

    // private IEnumerator TurnOffLightning()
    // {
    //     yield return new WaitForSeconds(0.1f);

    //     lightningLine.widthMultiplier

    //     lightningLine.gameObject.SetActive(false);
    // }

    public override void ToggleAttacker(bool toggle)
    {
        base.ToggleAttacker(toggle);

        ambientParticles.Play();
    }
}
