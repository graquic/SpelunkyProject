using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    private enum CheckDirection
    {
        Left,
        Right,
        Down,
    }
    public static bool edgeVerticalConnect;

    public BSPNode rootNode;

    [Header("��� ���� ���")]
    private List<BSPNode> treeList;
    private List<BSPNode> roomList;
    private List<Path> pathList;

    private List<BSPNode> tempList;

    [Header("���� ũ��")]
    [SerializeField] private Vector2Int bottomLeft; 
    [SerializeField] private Vector2Int topRight;

    [Header("���� �ٱ� �� �β�")]
    [Range(2, 20)] [SerializeField] private int extendedWidth;
    [Range(2, 20)] [SerializeField] private int extendedHeight;

    [Header("��� ���� ���� ����")]
    [SerializeField] private int roomMinXSize;
    [SerializeField] private int roomMinYSize;

    [Header("��� ���� �ּ� ����")]
    [Range(20, 80)][SerializeField] private int divideRate;

    [Header("��� ���� �ִ� Ƚ��")]
    [SerializeField] private int maxDepth;

    private bool isDrawable;

    [Header("��� �� ���� �Ÿ�")]
    [SerializeField] private int distanceFromPoint;

    
    private int depth = 0;

    private int[,] map;
    private int[,] extendedMap;

    [Header("��� ũ��")]
    [SerializeField] int pathSize;

    [Header("Ÿ�� ���")]
    [SerializeField] private RuleTile wall;
    [SerializeField] private RuleTile background;
    [SerializeField] private Tilemap platformTileMap;
    [SerializeField] private Tilemap backgroundTileMap;

    [Header("�Ա�,�ⱸ")]
    public GameObject startPoint;
    public GameObject endPoint;

    // public GameManager manager;

    private void Awake()
    {
        map = new int[topRight.y - bottomLeft.y, topRight.x - bottomLeft.x];

        treeList = new List<BSPNode>();
        roomList = new List<BSPNode>();
        pathList = new List<Path>();

        tempList = new List<BSPNode>();
    }

    private void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int y = 0; y < map.GetLength(0); y++)
            for (int x = 0; x < map.GetLength(1); x++)
                map[y, x] = 0;

        treeList.Clear();
        roomList.Clear();
        //lineList.Clear();

        BSPNode root  = new BSPNode(bottomLeft, topRight);
        
        treeList.Add(root);
        MakeTreeNodes(ref root, depth);

        
       
        MakeRoom();
        ConnectNodeVertical();
        ConnectRoomsPath();
        

        /*
        ConnectRoom();
        ExtendLine();
        isDrawable = true;
        BuildWall();
        */

        

        CreateTileMap();

        
    }

    void MakeTreeNodes(ref BSPNode node, int depth)
    {
        node.depth = depth;
        if (depth >= maxDepth)
            return;
        int dividedRatio = Random.Range(divideRate, 100 - divideRate + 1);
        depth++;
        
        node.SetDirection();

        if (node.DivideNode(dividedRatio, roomMinXSize, roomMinYSize, distanceFromPoint) == true) // ��        
        {
            MakeTreeNodes(ref node.leftNode, depth); // �������� - ���
            MakeTreeNodes(ref node.rightNode, depth);

           
            treeList.Add(node.leftNode); // ���ϴܿ� �����ϸ� �� ������ �� ���� ���� ������� treeList�� �߰��ǰ� �� ���� ������ ��尡 �߰��Ǹ� ���� �Լ��� ������ ����Լ��� ȣ��� �������� ���ư�
            treeList.Add(node.rightNode); // �ٽ� �ش� ����� ������ ��带 �����ϴ� ����Լ��� ����. ��������� �� �̻� ������ �� ����, ������ ������ �ּ� ũ���� ���ѿ� �ɸ� �� ����� �θ� ������ ���� ����Ʈ�� �ϼ�
           
        }
    }
    

    void MakeRoom()
    {   
        for (int x = 0; x < treeList.Count; x++)
        {
            
            treeList[x].CreateRoom(distanceFromPoint);


            if (treeList[x].isDivided == false) // �߷����� ���� ��� == ���ϴ� ���
            {
                for (int pointY = treeList[x].roomBottomLeft.y; pointY <= treeList[x].roomTopRight.y; pointY++)
                {
                    for (int pointX = treeList[x].roomBottomLeft.x; pointX <= treeList[x].roomTopRight.x; pointX++)
                    {
                        if (pointX == treeList[x].roomBottomLeft.x || pointX == treeList[x].roomTopRight.x || pointY == treeList[x].roomBottomLeft.y || pointY == treeList[x].roomTopRight.y)
                        {
                            map[pointY, pointX] = 1;
                            
                        }   // room�� �ֿܰ��� 1, �� ������ ���� 
                        else
                            map[pointY, pointX] = 2; // room�� �ֿܰ��� �ƴ� �κе��� �� �������� ����
                    }
                }
                roomList.Add(treeList[x]);
            }
        }
    }

    void ConnectRoomsPath()
    {
        for(int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].topRight.y == topRight.y) // �ֻ���� ��
            {
                if (roomList[i].bottomLeft.x == 0) // ���ʳ��� ��
                {
                    CheckBottomRoom(roomList[i]);
                    CheckRightRoom(roomList[i]);
                }

                else if(roomList[i].topRight.x == topRight.x) // ������ ���� ��
                {
                    CheckBottomRoom(roomList[i]);
                    CheckLeftRoom(roomList[i]);
                }

                else // �� �� �ƴ� ��
                {
                    CheckLeftRoom(roomList[i]);
                    if (roomList[i].isSideConnected == false)
                    { CheckRightRoom(roomList[i]); }
                    
                }
            }

            else if (roomList[i].bottomLeft.y == bottomLeft.y) // ���ϴ��� ��
            {
                CheckLeftRoom(roomList[i]);
                if (roomList[i].isSideConnected == false)
                { CheckRightRoom(roomList[i]); }
            }
            
            else
            {
                if(roomList[i].isSideConnected == false)
                {
                    int dir = Random.Range(0, 2);

                    switch((CheckDirection)dir)
                    {
                        case CheckDirection.Left:
                            if (roomList[i].bottomLeft.x == bottomLeft.x) { break; }
                            CheckLeftRoom(roomList[i]);
                            break;
                        case CheckDirection.Right:
                            if (roomList[i].topRight.x == topRight.x) { break; }
                            CheckRightRoom(roomList[i]);
                            break;
                    }
                }



                if (roomList[i].isBottomConnected == false)
                {
                    int dir = Random.Range(0, 6);

                    if (dir < 4)
                    {
                        CheckBottomRoom(roomList[i]);
                    }
                }
                
            }
            
        }

        MakePath();
    }
        
    void CheckLeftRoom(BSPNode node)
    {
        if (node.roomBottomLeft.x - (2 * distanceFromPoint) > 0  // map�� 2���� �迭�̹Ƿ� ������ ���� ������ map�� ũ�⸦ ����� index error - map�� �ּ�ġ�� ��
                && map[node.roomBottomLeft.y + 1, node.roomBottomLeft.x - (2 * distanceFromPoint) - 1] != 1)
        {
            Vector2Int BL = new Vector2Int(node.roomBottomLeft.x - (2 * distanceFromPoint), node.roomBottomLeft.y + 1);
            Vector2Int TR = new Vector2Int(node.roomBottomLeft.x, node.roomBottomLeft.y + pathSize);

            Path path = new Path(BL, TR, Direction.Horizontal);
            pathList.Add(path);

            node.isSideConnected = true;
        }
    }

    void CheckRightRoom(BSPNode node)
    {
        if (node.roomTopRight.x + (2 * distanceFromPoint) < map.GetLength(1) // map�� 2���� �迭�̹Ƿ� ������ ���� ������ map�� ũ�⸦ ����� index error - map�� �ִ�ġ�� ��
                && map[node.roomBottomLeft.y +1, node.roomTopRight.x + (2 * distanceFromPoint) - 1] != 1)
        {
            Vector2Int BL = new Vector2Int(node.roomTopRight.x, node.roomBottomLeft.y);
            Vector2Int TR = new Vector2Int(node.roomTopRight.x + (2 * distanceFromPoint), node.roomBottomLeft.y + pathSize + 1);

            Path path = new Path(BL, TR, Direction.Horizontal);
            pathList.Add(path);

            node.isSideConnected = true;
        }
    }

    void CheckBottomRoom(BSPNode node)
    {
        int centerX = Mathf.RoundToInt((node.roomTopRight.x + node.roomBottomLeft.x) / 2);

        if (map[node.roomBottomLeft.y - (2 * distanceFromPoint), centerX] == 1)
        {
            Vector2Int BL = new Vector2Int(centerX - pathSize, node.roomBottomLeft.y - (2 * distanceFromPoint));
            Vector2Int TR = new Vector2Int(centerX, node.roomBottomLeft.y);


            Path path = new Path(BL, TR, Direction.Vertical);
            pathList.Add(path);

            node.isBottomConnected = true;
        }
    }

    void ConnectNodeVertical()
    {
        int connect = Random.Range(0, 2);
        if (connect == 0) { return; }

        for (int i = 0; i < roomList.Count; i++)
        {
            for (int j = 0; j < roomList.Count; j++)
            {
                if (roomList[i] != roomList[j] && roomList[i].parentNode == roomList[j].parentNode
                    && roomList[i].isBottomConnected == false && roomList[j].isBottomConnected == false)
                {
                    if (roomList[i].parentNode.GetDirection() == true)
                    {
                        BSPNode parent = roomList[i].parentNode;

                        int centerX = Mathf.RoundToInt((parent.rightNode.roomBottomLeft.x + parent.rightNode.roomTopRight.x) / 2);
                        int centerY = parent.rightNode.roomBottomLeft.y;

                        Vector2Int BL = new Vector2Int(centerX - pathSize, centerY - (2 * distanceFromPoint));
                        Vector2Int TR = new Vector2Int(centerX, centerY);

                        Path path = new Path(BL, TR, Direction.Vertical);
                        pathList.Add(path);

                        parent.rightNode.isBottomConnected = true;
                    }
                }
            }
        }
    }

    void MakePath()
    {

        foreach (Path path in pathList)
        {
            if (path.direction == Direction.Horizontal)
            {
                for (int y = 0; y < pathSize + 1; y++)
                {
                    for (int x = 0; x < (2 * distanceFromPoint) + 1; x++)
                    {
                        if (y == 0 || y == pathSize)
                        {
                            map[path.bottomLeft.y + y, path.bottomLeft.x + x] = 1;
                        }

                        else
                        {
                            map[path.bottomLeft.y + y, path.bottomLeft.x + x] = 2;
                        }

                        

                    }
                }
            }

            else if (path.direction == Direction.Vertical)
            {
                for (int y = 0; y < (2 * distanceFromPoint) + 1; y++)
                {
                    for (int x = 0; x < pathSize + 1; x++)
                    {
                        if (x == 0 || x == pathSize)
                        {
                            map[path.bottomLeft.y + y, path.bottomLeft.x + x] = 1;
                        }

                        else
                        {
                            map[path.bottomLeft.y + y, path.bottomLeft.x + x] = 2;
                        }
                    }
                }
            }
        }
    }

    


    void CreateTileMap()
    {
        int mapWidth = map.GetLength(1);
        int mapHeight = map.GetLength(0);

        int newWidth = mapWidth + extendedWidth * 2; 
        int newHeight = mapHeight + extendedHeight * 2; 

        extendedMap = new int[newHeight, newWidth];

        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                extendedMap[y + extendedHeight, x + extendedWidth] = map[y, x];
            }
        }

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    extendedMap[y + extendedHeight, x + extendedWidth] = 1;
                }

                else if (map[y, x] == 2)
                {
                    extendedMap[y + extendedHeight, x + extendedWidth] = 2;
                }

                else if(map[y, x] == 3)
                {
                    extendedMap[y + extendedHeight, x + extendedWidth] = 3;
                }

                else
                {
                    extendedMap[y + extendedHeight, x + extendedWidth] = 1;
                }
            }
        }

        for (int y = 0; y < extendedMap.GetLength(0); y++)
        {
            for (int x = 0; x < extendedMap.GetLength(1); x++)
            {
                if (extendedMap[y, x] == 1)
                {
                    platformTileMap.SetTile(new Vector3Int(x, y, 0), wall);
                }

                else if (extendedMap[y, x] == 2 || extendedMap[y, x] == 3)
                {

                }

                else
                {
                    platformTileMap.SetTile(new Vector3Int(x, y, 0), wall);
                }
            }
        }
        

        /*
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    wallTilemap.SetTile(new Vector3Int(x, y, 0), wall);
                }

                else
                {
                    // roadTilemap.SetTile(new Vector3Int(x, y, 0), road);
                }
            }
        }
        */

    }
        
    
}
