using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    //Each tile on the Grid is a GridObject
    private List<Structure> gridStructureList; //All units currently on the tile
    private GridSystem<GridObject> gridSystem;

    private GridPosition gridPosition; //Position of this tile

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition) //Constructor
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        gridStructureList = new List<Structure>();
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public List<Structure> GetGridStructureList()
    {
        return gridStructureList;
    }

    public void AddGridStructure(Structure gridStructure)
    {
        gridStructureList.Add(gridStructure);
    }

    public void RemoveGridStructure(Structure gridStructure)
    {
        gridStructureList.Remove(gridStructure);
    }

    public bool HasAnyStructure()
    {
        return gridStructureList.Count > 0;
    }

    public Structure GetStructure()
    {
        if (HasAnyStructure())
        {
            return gridStructureList[0];
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
