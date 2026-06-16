using System.Collections;
using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    private bool isTrainPaused = false;
    private bool isTrainActive = false;
    private Vector2 trainVelocity;
    private float trainTime;

    private float playerCheckTimer = 0f;
    private float playerCheckFrequency = 1f;
    private float scrapCollectionDistance = 2.5f;

    private int scrapPerCollection = 10;

    private Transform playerTransform;
    private Transform movementGoal;

    [SerializeField]
    private bool stopAtEndPoint;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
    }

    private void Update()
    {
        if (!isTrainActive)
        {
            return;
        }

        MoveTrain();

        playerCheckTimer += Time.deltaTime;

        if (playerCheckTimer > playerCheckFrequency)
        {
            playerCheckTimer = 0f;
            CheckPlayerDistance();
        }
    }

    private void MoveTrain()
    {
        if (isTrainPaused)
        {
            return;
        }

        // transform.position += moveDirection * trainSpeed * Time.deltaTime;
        transform.position = Vector2.SmoothDamp(
            transform.position,
            movementGoal.position,
            ref trainVelocity,
            trainTime
        );

        float distanceToGoal = Vector2.Distance(transform.position, movementGoal.position);

        if (distanceToGoal < 0.1f)
        {
            if (stopAtEndPoint || (movementGoal == startPoint))
            {
                DeactivateTrain();
            }
            else
            {
                StartCoroutine(ChangeDirection());
            }
        }
    }

    private IEnumerator ChangeDirection()
    {
        isTrainPaused = true;
        yield return new WaitForSeconds(1f);

        movementGoal = startPoint;
        isTrainPaused = false;
    }

    private void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        Debug.Log(distanceToPlayer);

        if (distanceToPlayer <= scrapCollectionDistance)
        {
            //Play scrap collection effect

            ScrapManager.Instance.AddScrap(scrapPerCollection);
        }
    }

    private void DeactivateTrain()
    {
        isTrainActive = false;
    }

    public void ActivateTrain(float trainSpeed)
    {
        if (isTrainActive)
        {
            return;
        }

        this.trainTime = trainSpeed;

        movementGoal = endPoint;
        transform.position = startPoint.position;
        isTrainActive = true;
    }
}
