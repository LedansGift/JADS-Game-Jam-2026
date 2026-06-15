using UnityEngine;

public class EnemyLaneMovement : EnemyMovement
{
    private int routeIndex = 0;
    private float movementRefreshTimer = 0f;
    private float movementRefreshFrequency = 0.2f;
    private float distanceForRouteUpdate = 0.075f;

    private Transform targetTransform;
    private Transform[] route;

    protected override void MoveEnemy()
    {
        Vector2 movementValue = (targetTransform.position - transform.position).normalized;
        moveRb.MovePosition(
            moveRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * movementSpeed
                    * Time.fixedDeltaTime
        );

        //Debug.Log("Movement: " + movementValue + " Delta Time: " + Time.fixedDeltaTime);

        movementRefreshTimer += Time.fixedDeltaTime;

        if (movementRefreshTimer < movementRefreshFrequency)
        {
            return;
        }

        RefreshMovement(movementValue);
    }

    private void RefreshMovement(Vector2 movementValue)
    {
        movementRefreshTimer = 0f;

        if (movementValue.sqrMagnitude > 0f)
        {
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }

        SetVisualDirection(movementValue.x);

        if (Vector2.Distance(transform.position, targetTransform.position) < distanceForRouteUpdate)
        {
            routeIndex++;
            targetTransform = route[routeIndex];
        }
    }

    public void SetEnemyRoute(Transform[] route)
    {
        this.route = route;
        routeIndex = 0;
        targetTransform = route[routeIndex];
    }
}
