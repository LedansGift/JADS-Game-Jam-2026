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

        //gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
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

    public void AddPlaceableAtGridPosition(GridPosition gridPosition, IPlaceable placeable)
    {
        if (HasGridObject(gridPosition))
            gridSystem.GetGridObject(gridPosition).AddGridPlaceable(placeable);
    }

    public void RemovePlaceableAtGridPosition(GridPosition gridPosition, IPlaceable placeable)
    {
        gridSystem.GetGridObject(gridPosition).RemoveGridPlaceable(placeable);
    }

    public IPlaceable GetPlaceableAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetPlaceable();
    }

    public bool HasAnyPlaceableOnGridPosition(GridPosition gridPosition)
    {
        if (!HasGridObject(gridPosition))
            return false;
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyPlaceable();
    }

    public bool TryGetPlaceableAtGridPosition(GridPosition gridPosition, out IPlaceable placeable)
    {
        placeable = null;
        if (!IsValidGridPosition(gridPosition))
        {
            return false;
        }

        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        if (gridObject.HasAnyPlaceable())
        {
            placeable = gridObject.GetPlaceable();
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

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public float GetCellSize() => gridSystem.GetCellSize();
}
