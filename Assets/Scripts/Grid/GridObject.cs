using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    //Each tile on the Grid is a GridObject
    private List<IPlaceable> gridPlaceableList; //All units currently on the tile
    private GridSystem<GridObject> gridSystem;

    private GridPosition gridPosition; //Position of this tile

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition) //Constructor
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        gridPlaceableList = new List<IPlaceable>();
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public List<IPlaceable> GetGridPlaceableList()
    {
        return gridPlaceableList;
    }

    public void AddGridPlaceable(IPlaceable gridPlaceable)
    {
        gridPlaceableList.Add(gridPlaceable);
    }

    public void RemoveGridPlaceable(IPlaceable gridPlaceable)
    {
        gridPlaceableList.Remove(gridPlaceable);
    }

    public bool HasAnyPlaceable()
    {
        return gridPlaceableList.Count > 0;
    }

    public IPlaceable GetPlaceable()
    {
        if (HasAnyPlaceable())
        {
            return gridPlaceableList[0];
        }
        else
        {
            return null;
        }
    }

    // public override string ToString()
    // {
    //     string unitString = "";
    //     foreach (Unit unit in gridUnitList)
    //     {
    //         unitString += unit + "\n";
    //     }
    //     return "\n" + unitString;
    // }
}
