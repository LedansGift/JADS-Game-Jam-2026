using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    private bool movementActive = false;
    protected float movementSpeed;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected Rigidbody2D moveRb;

    private void FixedUpdate()
    {
        if (movementActive)
        {
            MoveEnemy();
        }
    }

    protected abstract void MoveEnemy();

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

    public void SetupMovement(float movementSpeed)
    {
        this.movementSpeed = movementSpeed;
    }
}
