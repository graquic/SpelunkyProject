using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;
using TreeEditor;

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

    public Vector2Int roomBL;
    public Vector2Int roomTR;

    private Direction direction;
    private Direction prevDirection;

    public bool isDivided;

    public int depth;



    public BSPNode() { }
    public BSPNode(Vector2Int bottomLeft, Vector2Int topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
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

    public bool GetDirection()
    {
        if (direction == Direction.Vertical)
        { return true; }

        else return false;
    }

    public bool DivideNode(int ratio, int minDividedXSize , int minDividedYSize)
    {
        float temp;
        Vector2Int dividePoint1, dividePoint2;

        if (direction == Direction.Vertical)
        {
            temp = (topRight.x - bottomLeft.x);
            temp = temp * ratio / 100;
            int width = Mathf.RoundToInt(temp);
            if (width < minDividedXSize || topRight.x - bottomLeft.x - width < minDividedXSize)
                return false;
            dividePoint1 = new Vector2Int(bottomLeft.x + width, topRight.y);
            dividePoint2 = new Vector2Int(bottomLeft.x + width, bottomLeft.y);
            /*
            leftNode = new BSPNode(bottomLeft, dividePoint1);
            rightNode = new BSPNode(dividePoint2, topRight);
            */
        }
        else
        {
            temp = (topRight.y - bottomLeft.y);
            temp = temp * ratio / 100;
            int height = Mathf.RoundToInt(temp);
            if (height < minDividedYSize || topRight.y - bottomLeft.y - height < minDividedYSize)
                return false;
            dividePoint1 = new Vector2Int(bottomLeft.x, bottomLeft.y + height);
            dividePoint2 = new Vector2Int(topRight.x, bottomLeft.y + height);
            /*
            leftNode = new BSPNode(bottomLeft, dividePoint2);
            rightNode = new BSPNode(dividePoint1, topRight);
            */
        }
        
        leftNode = new BSPNode(bottomLeft, dividePoint1);
        rightNode = new BSPNode(dividePoint2, topRight);
        

        leftNode.parentNode = rightNode.parentNode = this;
        isDivided = true;
        return true;
    }

    public void CreateRoom()
    {
        int distanceFrom = 2;
        if (!isDivided)
        {
            roomBL = new Vector2Int(bottomLeft.x + distanceFrom, bottomLeft.y + distanceFrom);
            roomTR = new Vector2Int(topRight.x - distanceFrom, topRight.y - distanceFrom);
        }
    }

}
