using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove;

    private int footstepIndex = 0;
    private float footStepTimer = 0f;
    private const float TIME_BETWEEN_STEPS = 1f;
    private PlayerStats stats;

    private InputManager inputManager;

    [SerializeField]
    private Rigidbody2D playerRb;

    [SerializeField]
    private Transform visualTransform;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private SFXObject[] footstepSFX;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveRB();
        }
    }

    private Vector2 MoveRB()
    {
        Vector2 movementValue = inputManager.MovementValue.normalized;
        playerRb.MovePosition(
            playerRb.position
                + new Vector2(movementValue.x, movementValue.y)
                    * stats.GetMovementSpeed()
                    * Time.fixedDeltaTime
        );

        //Debug.Log("Movement: " + movementValue + " Delta Time: " + Time.fixedDeltaTime);

        if (movementValue.sqrMagnitude > 0f)
        {
            playerAnimator.SetBool("moving", true);

            footStepTimer += Time.deltaTime;

            if (footStepTimer >= (TIME_BETWEEN_STEPS / stats.GetMovementSpeed()))
            {
                AudioManager.PlaySFX(footstepSFX[footstepIndex], transform.position);
                footstepIndex++;
                footstepIndex = footstepIndex % footstepSFX.Length;
                footStepTimer = 0f;
            }
        }
        else
        {
            playerAnimator.SetBool("moving", false);
        }

        if (movementValue.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (movementValue.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        return movementValue;
    }

    public void ToggleCanMove(bool enable)
    {
        canMove = enable;
    }

    // private void OnPlayerDead(object sender, bool playerDead)
    // {
    //     ToggleCanMove(!playerDead);
    // }
}
