using System;
using UnityEngine;

public struct GridPosition : IEquatable<GridPosition>
{
    //The struct a GridObject uses for its position

    public int x;
    public int y;

    public GridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //All the equality nonsense it needs because it's a custom struct and comparisons only work if you have the below aiosudfhuiasohfuio
    public override bool Equals(object obj)
    {
        return obj is GridPosition position && x == position.x && y == position.y;
    }

    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    public static int AbsDistance(GridPosition a, GridPosition b)
    {
        GridPosition offset = b - a;
        return Mathf.Abs(offset.x) + Mathf.Abs(offset.y);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y);
    }

    public override string ToString()
    {
        return "x: " + x + "; y:" + y;
    }

    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.y + b.y);
    }

    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.y - b.y);
    }

    public static GridPosition operator *(GridPosition a, int b)
    {
        return new GridPosition(a.x * b, a.y * b);
    }
}
