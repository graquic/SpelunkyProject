using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Experimental.GraphView;

public enum Direction
{
    Vertical,
    Horizontal,
}
public class BSPNode
{
    public BSPNode leftNode;
    public BSPNode rightNode;
    public BSPNode parentNode;

    public Vector2Int bottomLeft;
    public Vector2Int topRight;

    private Direction direction;

    public bool isDivided;
    private int divideRatio;
    


    public BSPNode() { }
    public BSPNode(Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
    public BSPNode(Vector2Int bottomLeft, Vector2Int topRight, int ratio)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
        this.divideRatio = ratio;
    }

    public void SetDirection()
    {
        if (topRight.x - bottomLeft.x > topRight.y - bottomLeft.y) { direction = Direction.Vertical; }

        else if (topRight.x - bottomLeft.x < topRight.y - bottomLeft.y) { direction = Direction.Horizontal; }

        else
        {
            int temp = Random.Range(0, 2);
            if (temp == 0) { direction = Direction.Vertical; }
            else { direction = Direction.Horizontal; } 
        }
    }

}
