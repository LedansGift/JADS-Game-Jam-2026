using System;
using UnityEngine;

public class GridSystem<TGridObject>
{
    //**The Grid**
    private int width; //How wide it is
    private int height; //How high it is
    private float cellSize; //How large each tile is
    private Vector2 gridStartOffset;
    private TGridObject[,] gridObjectArray; //All of the tiles on this grid, stored in a 2D array based on what their gridposition is

    public GridSystem(
        int width,
        int height,
        float cellSize,
        Vector2 gridStartOffset,
        Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject
    )
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridStartOffset = gridStartOffset;

        gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector2(
            (gridPosition.x * cellSize) + gridStartOffset.x,
            (gridPosition.y * cellSize) + gridStartOffset.y
        );
    }

    public GridPosition GetGridPosition(Vector2 worldPosition)
    {
        // Debug.Log(worldPosition);
        // Debug.Log(
        //     new GridPosition(
        //         Mathf.RoundToInt(worldPosition.x / cellSize),
        //         Mathf.RoundToInt(worldPosition.z / cellSize)
        //     )
        // );
        return new GridPosition(
            Mathf.RoundToInt((worldPosition.x - gridStartOffset.x) / cellSize),
            Mathf.RoundToInt((worldPosition.y - gridStartOffset.y) / cellSize)
        );
    }

    //Makes all the GridDebugObjects to show the GridPosition + current units of all tiles
    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                Transform debugTransform = GameObject.Instantiate(
                    debugPrefab,
                    GetWorldPosition(gridPosition),
                    Quaternion.identity
                );
                // GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                // gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    //You can get a specific tile based on its GridPosition
    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.y];
    }

    private bool CheckIfTileAccessible(GridPosition gridPosition)
    {
        float tileCheckRadius = .1f;

        Collider2D[] blockingColliders = Physics2D.OverlapCircleAll(
            GetWorldPosition(gridPosition),
            tileCheckRadius
        );

        // Collider[] colliderArray = Physics.OverlapSphere(
        //     new Vector3(gridPosition.x * cellSize, 0, gridPosition.y * cellSize),
        //     tileCheckRadius
        // );

        foreach (Collider2D collider in blockingColliders)
        {
            if (collider.gameObject.layer == 7)
            {
                return false;
            }
        }

        return true;
    }

    //Tests if the tile is within the Grid
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0
            && gridPosition.y >= 0
            && gridPosition.x < width
            && gridPosition.y < height
            && CheckIfTileAccessible(gridPosition);
    }

    public int GetWidth() //of the Grid
    {
        return width;
    }

    public int GetHeight() //^
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }
}
