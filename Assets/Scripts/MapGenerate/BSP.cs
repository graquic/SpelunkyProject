using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor.Experimental.GraphView;
using System.Runtime.InteropServices;
using TreeEditor;
using Unity.VisualScripting.FullSerializer;

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

    public Vector2Int roomBottomLeft;
    public Vector2Int roomTopRight;

    private Direction direction;

    public bool isDivided;

    public bool isSideConnected;
    public bool isBottomConnected;

    public bool isEndPoint;

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
        if (direction == Direction.Horizontal)
        { return true; }

        else return false;
    }

    public bool DivideNode(int ratio, int minDividedXSize , int minDividedYSize, int distanceFromPoint)
    {
        float temp;
        Vector2Int dividePoint1;
        Vector2Int dividePoint2;

        if (direction == Direction.Vertical)
        {
            temp = ((topRight.x - distanceFromPoint) - (bottomLeft.x + distanceFromPoint));
            temp = temp * ratio / 100;
            int width = Mathf.RoundToInt(temp);
            if (width - (2 * distanceFromPoint) < minDividedXSize || topRight.x - bottomLeft.x - width - (2 * distanceFromPoint) < minDividedXSize) // 타일의 크기가 결정됨
                return false;
            dividePoint1 = new Vector2Int(bottomLeft.x + width, topRight.y);
            dividePoint2 = new Vector2Int(bottomLeft.x + width, bottomLeft.y);
            
        }
        else
        {
            temp = (topRight.y - bottomLeft.y);
            temp = temp * ratio / 100;
            int height = Mathf.RoundToInt(temp);
            if (height < minDividedYSize || topRight.y - bottomLeft.y - height < minDividedYSize)
                return false;
            dividePoint1 = new Vector2Int(topRight.x, bottomLeft.y + height);
            dividePoint2 = new Vector2Int(bottomLeft.x, bottomLeft.y + height);

        }
        
        leftNode = new BSPNode(bottomLeft, dividePoint1);
        rightNode = new BSPNode(dividePoint2, topRight);

        leftNode.parentNode = this;
        rightNode.parentNode = this;

        isDivided = true;
        return true;
    }

    public void CreateRoom(int distanceFromPoint)
    {
        if (!isDivided)
        {
            roomBottomLeft = new Vector2Int(bottomLeft.x + distanceFromPoint, bottomLeft.y + distanceFromPoint);
            roomTopRight = new Vector2Int(topRight.x - distanceFromPoint, topRight.y - distanceFromPoint);
        }
    }

}
