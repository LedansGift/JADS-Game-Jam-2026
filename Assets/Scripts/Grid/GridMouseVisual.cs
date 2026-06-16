using UnityEngine;

public class GridMouseVisual : MonoBehaviour
{
    public static GridMouseVisual Instance;

    private bool mouseVisualActive = false;
    private GridPosition currentGridPosition;
    private InputManager input;

    [SerializeField]
    private Transform mouseGridVisual;

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
    }

    public bool TryGetValidGridPosition(out GridPosition gridPosition)
    {
        gridPosition = currentGridPosition;

        //Must be within level bounds
        //Must not be an ilegal space

        return LevelGrid.Instance.IsValidGridPosition(gridPosition);
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
