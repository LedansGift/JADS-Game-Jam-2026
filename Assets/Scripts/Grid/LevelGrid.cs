using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }

    //public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private float cellSize;

    [SerializeField]
    private Vector2 gridOffset;

    // [SerializeField]
    // private Transform gridDebugVisual;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one LevelGrid! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //Gridsystem is created
        gridSystem = new GridSystem<GridObject>(
            width,
            height,
            cellSize,
            gridOffset,
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition)
        );

        //gridSystem.CreateDebugObjects(gridDebugVisual);
    }

    //This script is basically for other classes to interact with the GridSystem and to see/manage which tiles units are on

    public bool HasGridObject(GridPosition gridPosition)
    {
        if (gridSystem.GetGridObject(gridPosition) != null)
        {
            return true;
        }
        return false;
    }

    public void AddStructureAtGridPosition(GridPosition gridPosition, Structure structure)
    {
        if (HasGridObject(gridPosition))
            gridSystem.GetGridObject(gridPosition).AddGridStructure(structure);
    }

    public void RemoveStructureAtGridPosition(GridPosition gridPosition, Structure structure)
    {
        gridSystem.GetGridObject(gridPosition).RemoveGridStructure(structure);
    }

    public Structure GetStructureAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetStructure();
    }

    public bool HasAnyStructureOnGridPosition(GridPosition gridPosition)
    {
        if (!HasGridObject(gridPosition))
            return false;
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyStructure();
    }

    public bool TryGetStructureAtGridPosition(GridPosition gridPosition, out Structure structure)
    {
        structure = null;
        if (!IsValidGridPosition(gridPosition))
        {
            return false;
        }

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        if (gridObject.HasAnyStructure())
        {
            structure = gridObject.GetStructure();
            return true;
        }
        else
        {
            return false;
        }
    }

    public GridPosition GetGridPosition(Vector2 worldPosition) =>
        gridSystem.GetGridPosition(worldPosition);

    public GridObject GetGridObject(GridPosition gridPosition) =>
        gridSystem.GetGridObject(gridPosition);

    public Vector2 GetWorldPosition(GridPosition gridPosition) =>
        gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) =>
        gridSystem.IsValidGridPosition(gridPosition);

    public bool IsLanePosition(GridPosition gridPosition) =>
        gridSystem.IsLanePosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();
}
