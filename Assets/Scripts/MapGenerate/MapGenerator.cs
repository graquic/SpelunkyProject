using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
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
    public static MapGenerator Instance { get; private set; }
    
    public static bool edgeVerticalConnect;

    public BSPNode rootNode;

    [Header("노드 보관 목록")]
    private List<BSPNode> treeList;
    private List<BSPNode> roomList;
    public List<BSPNode> RoomList { get { return roomList; } }
    private List<Path> pathList;

    [Header("맵의 크기")]
    [SerializeField] private Vector2Int bottomLeft; 
    public Vector2Int BottomLeft { get { return bottomLeft; } }
    [SerializeField] private Vector2Int topRight;
    public Vector2Int TopRight { get {  return topRight; } }

    [Header("맵의 바깥 벽 두께")]
    [SerializeField] private int extendedWidth;
    [SerializeField] private int extendedHeight;

    public int ExtendedWidth { get { return extendedWidth; } }
    public int ExtendedHeight { get {  return extendedHeight; } }

    [Header("노드 분할 기준 길이")]
    [SerializeField] private int roomMinXSize;
    [SerializeField] private int roomMinYSize;

    [Header("노드 분할 최소 비율")]
    [Range(20, 80)][SerializeField] private int divideRate;

    [Header("노드 분할 최대 횟수")]
    [SerializeField] private int maxDepth;

    private bool isDrawable;

    [Header("노드 벽 간의 거리")]
    [SerializeField] private int distanceFromPoint;
    public int DistanceFromPoint { get { return distanceFromPoint; } }

    
    private int depth = 0;

    private int[,] map;
    private int[,] extendedMap;
    public int[,] ExtendedMap { get { return extendedMap; } }

    [Header("통로 크기")]
    [SerializeField] int pathSize;

    [Header("타일 목록")]
    [SerializeField] private RuleTile wall;
    [SerializeField] private RuleTile background;

    [SerializeField] private Tilemap backgroundTileMap;
    [SerializeField] private Tilemap platformTileMap;
    public Tilemap PlatformTileMap { get { return platformTileMap; } }

    


    // public GameManager manager;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
        //DontDestroyOnLoad(gameObject);

        map = new int[topRight.y - bottomLeft.y, topRight.x - bottomLeft.x];

        treeList = new List<BSPNode>();
        roomList = new List<BSPNode>();
        pathList = new List<Path>();

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

        CreateGates();

        CreatePlatformTiles(); // 구조물

        /*
        foreach(BSPNode node in roomList)
        {
            print($"{node.roomBottomLeft} , {node.roomTopRight}");
        }
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

        if (node.DivideNode(dividedRatio, roomMinXSize, roomMinYSize, distanceFromPoint) == true) // ★        
        {
            MakeTreeNodes(ref node.leftNode, depth); // 분할정복 - 재귀
            MakeTreeNodes(ref node.rightNode, depth);

           
            treeList.Add(node.leftNode); // 최하단에 도달하면 더 분할할 수 없는 왼쪽 노드들부터 treeList에 추가되고 그 다음 오른쪽 노드가 추가되며 이후 함수가 끝나면 재귀함수가 호출된 지점으로 돌아가
            treeList.Add(node.rightNode); // 다시 해당 노드의 오른쪽 노드를 분할하는 재귀함수에 진입. 결과적으로 더 이상 분할할 수 없는, 방으로 설정될 최소 크기의 제한에 걸린 방 노드들과 부모 노드들이 섞인 리스트가 완성
           
        }
    }
    

    void MakeRoom()
    {   
        for (int x = 0; x < treeList.Count; x++)
        {
            
            treeList[x].CreateRoom(distanceFromPoint);


            if (treeList[x].isDivided == false) // 잘려지지 않은 요소 == 최하단 요소
            {
                for (int pointY = treeList[x].roomBottomLeft.y; pointY <= treeList[x].roomTopRight.y; pointY++)
                {
                    for (int pointX = treeList[x].roomBottomLeft.x; pointX <= treeList[x].roomTopRight.x; pointX++)
                    {
                        if (pointX == treeList[x].roomBottomLeft.x || pointX == treeList[x].roomTopRight.x || pointY == treeList[x].roomBottomLeft.y || pointY == treeList[x].roomTopRight.y)
                        {
                            map[pointY, pointX] = 1;
                            
                        }   // room의 최외곽은 1, 즉 벽으로 설정 
                        else
                            map[pointY, pointX] = 2; // room의 최외곽이 아닌 부분들은 빈 공간으로 설정
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
            if (roomList[i].topRight.y == topRight.y) // 최상단일 때
            {
                if (roomList[i].bottomLeft.x == 0) // 왼쪽끝일 때
                {
                    CheckBottomRoom(roomList[i]);
                    CheckRightRoom(roomList[i]);
                }

                else if(roomList[i].topRight.x == topRight.x) // 오른쪽 끝일 때
                {
                    CheckBottomRoom(roomList[i]);
                    CheckLeftRoom(roomList[i]);
                }

                else // 둘 다 아닐 때
                {
                    CheckLeftRoom(roomList[i]);
                    if (roomList[i].isSideConnected == false)
                    { CheckRightRoom(roomList[i]); }
                    
                }
            }

            else if (roomList[i].bottomLeft.y == bottomLeft.y) // 최하단일 때
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
                    int dir = Random.Range(0, 3);

                    if (dir < 2)
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
        if (node.roomBottomLeft.x - (2 * distanceFromPoint) > 0  // map이 2차원 배열이므로 설정된 조건 범위가 map의 크기를 벗어나면 index error - map의 최소치와 비교
                && map[node.roomBottomLeft.y + distanceFromPoint, node.roomBottomLeft.x - (2 * distanceFromPoint) - 1] != 1)
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
        if (node.roomTopRight.x + (2 * distanceFromPoint) < map.GetLength(1) // map이 2차원 배열이므로 설정된 조건 범위가 map의 크기를 벗어나면 index error - map의 최대치와 비교
                && map[node.roomBottomLeft.y + distanceFromPoint, node.roomTopRight.x + (2 * distanceFromPoint) - 1] != 1)
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
                        
                        if(y == 0)
                        {
                            if(map[path.bottomLeft.y + 1, path.bottomLeft.x + x] == 1 
                                || map[path.bottomLeft.y + 1, path.bottomLeft.x + x] == 0)
                            {
                                map[path.bottomLeft.y, path.bottomLeft.x + x] = 2;

                                continue;
                            }

                        }
                        else if(y == pathSize)
                        {
                            if (map[path.bottomLeft.y - 1, path.bottomLeft.x + x] == 1
                                || map[path.bottomLeft.y - 1, path.bottomLeft.x + x] == 0)
                            {
                                map[path.bottomLeft.y + y, path.bottomLeft.x + x] = 2;

                                continue;
                            }
                        }
                        
                        
                        if (y == 0 || y == pathSize)
                        {
                            if (map[path.bottomLeft.y + y, path.bottomLeft.x + x] != 1) { continue; }
                            
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
    void CreateGates()
    {
        BSPNode closestStartNode = null;
        float candidStartPosY = 0;

        List<BSPNode> endPointNodes = new();

        foreach (BSPNode node in roomList)
        {
            if (node.topRight.y == topRight.y) // CreateStartPoints
            {
                if (node.roomBottomLeft.y > candidStartPosY)
                {
                    candidStartPosY = node.roomBottomLeft.y;
                    closestStartNode = node;
                }
            }

            if (node.bottomLeft.y == bottomLeft.y) // CreateEndPoints
            {
                endPointNodes.Add(node);
            }
        }

        if (closestStartNode != null)
        {
            SetStartPoint(closestStartNode);
        }

        SetEndPoint(endPointNodes);
    }

    void SetStartPoint(BSPNode closestStartNode)
    {
        float startPosX = (closestStartNode.bottomLeft.x + closestStartNode.topRight.x) / 2 + extendedWidth + 3;
        //float startPosY = (closestStartNode.bottomLeft.y + closestStartNode.topRight.y) / 2 + extendedHeight - 3;
        float startPosY = closestStartNode.roomBottomLeft.y + extendedHeight + 1 + distanceFromPoint;

        Vector2 startPoint = new Vector2(startPosX, startPosY);

        closestStartNode.isStartPoint = true;
        GameManager.Instance.SetStartPoint(startPoint);
    }

    void SetEndPoint(List<BSPNode> endPointNodes)
    {
        if (endPointNodes.Count != 0) // endPoint 생성
        {
            int targetIndex = Random.Range(0, endPointNodes.Count);
            float endPosX = (endPointNodes[targetIndex].bottomLeft.x + endPointNodes[targetIndex].topRight.x) / 2 + extendedWidth;
            float endPosY = endPointNodes[targetIndex].roomBottomLeft.y + extendedHeight + 1 + distanceFromPoint;
            endPointNodes[targetIndex].isEndPoint = true;

            Vector2 endPoint = new Vector2(endPosX, endPosY);
            /*
            print(endPoint);
            print($"{endPointNodes[targetIndex].roomBottomLeft} , {endPointNodes[targetIndex].roomTopRight}");
            */

            GameManager.Instance.SetEndPoint(endPoint);
        }
    }


    void CreatePlatformTiles() // 노드 안의 천장 플랫폼 배치
    {
        foreach(BSPNode node in roomList)
        {
            CreateCenterBlocks(node);

            int isPlay = Random.Range(0, 3);

            if (isPlay < 2)
            {
                int bottomX = Mathf.RoundToInt((node.roomBottomLeft.x + node.roomTopRight.x) / 2);
                int bottomY = node.roomBottomLeft.y - 2 * distanceFromPoint;

                if (node.isBottomConnected == true)
                {
                    for (int x = bottomX - 4; x < bottomX + 3; x++)
                    {
                        map[bottomY - 4, x] = 1;
                    }
                }
            }
        }
    }

    void CreateCenterBlocks(BSPNode node)
    {
        if (node.isEndPoint == true) { return; }

        if(node.topRight.x - node.bottomLeft.x > 15 && node.topRight.y - node.bottomLeft.y > 15)
        {
            int centerY = (node.topRight.y + node.bottomLeft.y) / 2;
            int centerX = (node.topRight.x + node.bottomLeft.x) / 2;

            int lengthX = (node.roomTopRight.x - node.roomBottomLeft.x) / 4;
            int lengthY = (node.roomTopRight.y - node.roomBottomLeft.y) / 4;

            int radius = 5;

            int selectForm = Random.Range(1, 5);
            
            if(selectForm == 1) // 사각형
            {

                for (int y = centerY - lengthY; y < centerY + lengthY; y++)
                {
                    for (int x = centerX - lengthX; x < centerX + lengthX; x++)
                    {
                        map[y, x] = 1;
                    }
                }
            }

            else if (selectForm == 2) // 사다리꼴?
            {
                if (lengthY < 10 || lengthX < 10) { return; }
                if(lengthY > lengthX)
                {
                    for (int y = centerY - lengthY; y < centerY + lengthY; y++)
                    {
                        for (int x = centerX - lengthX; x < centerX + lengthX; x++)
                        {
                            if (y > x)
                            {
                                map[y, x] = 1;
                            }

                        }
                    }
                }
                else
                {
                    for (int y = centerY - lengthY; y < centerY + lengthY; y++)
                    {
                        for (int x = centerX - lengthX; x < centerX + lengthX; x++)
                        {
                            if (y <= x)
                            {
                                map[y, x] = 1;
                            }

                        }
                    }
                }
                
            }

            
            else if (selectForm == 3)
            for (int y = centerY - lengthY; y <= centerY + lengthY; y++) // 마름모
            {
                for (int x = centerX - lengthX; x <= centerX + lengthX; x++)
                {
                    int distanceX = Mathf.Abs(x - centerX);
                    int distanceY = Mathf.Abs(y - centerY);

                    if (distanceX + distanceY <= radius)
                    {
                        map[y, x] = 1;
                    }
                }
            }
            

            else if(selectForm == 4)
            {
                int diameterY = 2 * (lengthY - 1);
                int diameterX = 2 * (lengthX - 1);

                for (int y = 0; y < diameterY; y++)
                {
                    for (int x = centerX - diameterX + 1 + y; x <= centerX + diameterX - 1 - y; x++)
                    {
                        map[y + centerY - 3, x] = 1;
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
    }
    /*
    public int[,] WorldPositionToMap(Vector3Int pos)
    {

    }   
    */
}
