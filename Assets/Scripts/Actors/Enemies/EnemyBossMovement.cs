using UnityEngine;

public class EnemyBossMovement : EnemyMovement
{
    private float movementRefreshTimer = 0f;
    private float movementRefreshFrequency = 0.15f;

    private Vector2 currentMovementValue = Vector2.zero;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private Transform strongholdTarget;

    private Transform targetTransform;

    protected override void MoveEnemy()
    {
        if (!targetTransform)
        {
            targetTransform = strongholdTarget;
        }

        Vector2 movementValue = (targetTransform.position - transform.position).normalized;
        moveRb.MovePosition(
            moveRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * movementSpeed
                    * Time.fixedDeltaTime
        );

        //Debug.Log("Movement: " + movementValue + " Delta Time: " + Time.fixedDeltaTime);

        RefreshMovement(currentMovementValue);

        currentMovementValue = movementValue;
    }

    private void RefreshMovement(Vector2 movementValue)
    {
        movementRefreshTimer += Time.fixedDeltaTime;

        if (movementRefreshTimer < movementRefreshFrequency)
        {
            return;
        }

        movementRefreshTimer = 0f;

        meshRenderer.sortingOrder = Mathf.RoundToInt((-transform.position.y) * 100f);

        SetVisualDirection(movementValue.x);
    }

    public void SetEnemyRoute(Transform destination)
    {
        strongholdTarget = destination;
        targetTransform = destination;
    }
}
