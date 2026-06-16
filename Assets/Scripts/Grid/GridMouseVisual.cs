using UnityEngine;

public class GridMouseVisual : MonoBehaviour
{
    public static GridMouseVisual Instance;

    private bool mouseVisualActive = false;
    private bool laneStructureActive = false;
    private GridPosition currentGridPosition;
    private InputManager input;

    [SerializeField]
    private Transform mouseGridVisual;

    [SerializeField]
    private SpriteRenderer visualRenderer;

    [SerializeField]
    private Color availableVisual;

    [SerializeField]
    private Color unavailableVisual;

    //private GridSystemVisualSingle mouseGridVisualScript;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //mouseGridVisualScript = mouseGridVisual.GetComponent<GridSystemVisualSingle>();
        //mouseGridVisualScript.ToggleTransparencyOscillation(true);

        input = InputManager.Instance;
        currentGridPosition = new GridPosition(0, 0);

        ToggleMouseVisibility(false);
    }

    void Update()
    {
        if (!mouseVisualActive)
        {
            return;
        }

        SetMouseVisualPosition();
    }

    private void SetMouseVisualPosition()
    {
        GridPosition mouseGridPosition;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(input.MousePosition);

        mouseGridPosition = LevelGrid.Instance.GetGridPosition(mousePosition);

        if (currentGridPosition == mouseGridPosition)
        {
            return;
        }

        currentGridPosition = mouseGridPosition;

        //Debug.Log("Current Grid Position: " + mouseGridPosition.ToString());

        mouseGridVisual.position = LevelGrid.Instance.GetWorldPosition(mouseGridPosition);

        //Change visual to green or red based on validity / a structure being present
        EvaluateMouseVisual();
    }

    private void EvaluateMouseVisual()
    {
        bool validPosition =
            laneStructureActive == LevelGrid.Instance.IsLanePosition(currentGridPosition);

        if (validPosition && LevelGrid.Instance.IsValidGridPosition(currentGridPosition))
        {
            visualRenderer.color = availableVisual;
        }
        else
        {
            visualRenderer.color = unavailableVisual;
        }
    }

    public bool TryGetValidGridPosition(out GridPosition gridPosition)
    {
        gridPosition = currentGridPosition;

        return LevelGrid.Instance.IsValidGridPosition(gridPosition);
    }

    public bool IsLaneGridPosition(GridPosition gridPosition)
    {
        return LevelGrid.Instance.IsLanePosition(gridPosition);
    }

    public void SetLaneStructureActive(bool laneStructure)
    {
        laneStructureActive = laneStructure;

        if (mouseVisualActive)
        {
            EvaluateMouseVisual();
        }
    }

    public void ToggleMouseVisibility(bool toggle)
    {
        mouseVisualActive = toggle;
        mouseGridVisual.gameObject.SetActive(toggle);

        if (toggle)
        {
            SetMouseVisualPosition();
        }
    }
}
