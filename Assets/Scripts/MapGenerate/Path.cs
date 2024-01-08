using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Path
{
    public Path(Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
    public Path(Vector2Int bottomLeft, Vector2Int topRight, Direction direction)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        this.direction = direction;
    }

    public Vector2Int bottomLeft;
    public Vector2Int topRight;

    public Direction direction;
}
