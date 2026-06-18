using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    private bool movementActive = false;
    protected float movementSpeed;
    protected float targetMovementSpeed;
    private float movementSpeedRestoration = 0.5f;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected Rigidbody2D moveRb;

    private void Update()
    {
        if (!movementActive)
        {
            return;
        }

        if (movementSpeed < targetMovementSpeed)
        {
            RestoreMovementSpeed();
        }
    }

    private void FixedUpdate()
    {
        if (movementActive)
        {
            MoveEnemy();
        }
    }

    protected abstract void MoveEnemy();

    private void RestoreMovementSpeed()
    {
        movementSpeed += movementSpeedRestoration * Time.deltaTime;
    }

    protected void SetVisualDirection(float xMovement)
    {
        if (xMovement < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (xMovement > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    public void SpawnEnemyAtLocation(Vector2 spawnPosition)
    {
        moveRb.position = spawnPosition;
    }

    public void StartMovement()
    {
        movementActive = true;
        animator.SetBool("move", true);
    }

    public void StopMovement()
    {
        movementActive = false;
        animator.SetBool("move", false);
    }

    public void SlowMovement(float movementSlowFactor)
    {
        movementSpeed = targetMovementSpeed * movementSlowFactor;
    }

    public void SetupMovement(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
        targetMovementSpeed = movementSpeed;
    }
}
