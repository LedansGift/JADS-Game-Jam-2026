using UnityEngine;

public class EnemyLaneMovement : EnemyMovement
{
    private int routeIndex = 0;
    private float movementRefreshTimer = 0f;
    private float movementRefreshFrequency = 0.15f;
    private float distanceForRouteUpdate = 0.1f;
    private float routeRandomVariance = 0.1f;
    private Vector2 currentMovementValue = Vector2.zero;

    [SerializeField]
    private MeshRenderer meshRenderer;

    private Transform targetTransform;
    private Vector3 targetPosition;
    private Transform[] route;

    protected override void MoveEnemy()
    {
        Vector2 movementValue = (targetPosition - transform.position).normalized;
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

        if (Vector2.Distance(transform.position, targetPosition) < distanceForRouteUpdate)
        {
            if (routeIndex >= (route.Length - 1))
            {
                return;
            }

            routeIndex++;
            targetTransform = route[routeIndex];
            targetPosition =
                targetTransform.position
                + new Vector3(
                    Random.Range(-routeRandomVariance, routeRandomVariance),
                    Random.Range(-routeRandomVariance, routeRandomVariance)
                );
        }
    }

    public void SetEnemyRoute(Transform[] route)
    {
        this.route = route;
        routeIndex = 0;
        targetTransform = route[routeIndex];
        targetPosition =
            targetTransform.position
            + new Vector3(
                Random.Range(-routeRandomVariance, routeRandomVariance),
                Random.Range(-routeRandomVariance, routeRandomVariance)
            );
    }
}
