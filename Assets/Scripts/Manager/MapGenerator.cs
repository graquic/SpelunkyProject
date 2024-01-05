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
    public BSPNode rootNode;

    private List<BSPNode> treeList;
    private List<BSPNode> roomList;

    // private List<Line> lineList;

    [SerializeField] private Vector2Int bottomLeft;
    [SerializeField] private Vector2Int topRight;

    private bool isDrawable;

    [SerializeField]
    private int roomMinXSize;
    [SerializeField]
    private int roomMinYSize;


    [SerializeField]
    private int maxDepth;
    private int depth = 0;

    private int[,] map;

    [SerializeField]
    private Tile wall;
    [SerializeField]
    private Tile road;
    [SerializeField]
    private Tilemap roadTilemap;
    [SerializeField]
    private Tilemap wallTilemap;

    public GameObject startPoint;
    public GameObject endPoint;

    // public GameManager manager;

    private void Awake()
    {
        map = new int[topRight.y -bottomLeft.y, topRight.x - bottomLeft.x];
        treeList = new List<BSPNode>();
        roomList = new List<BSPNode>();
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

        int count = 0;
        foreach(BSPNode node in treeList)
        {
            if(node.isDivided == false)
            {
                count++;
            }
            
        }

        /*
        print(treeList.Count);
        print(count);
        */
        /////////////////////////////////////////////////////////


        MakeRoom();
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
        int dividedRatio = Random.Range(25, 76);
        depth++;
        
        node.SetDirection();

        if (node.DivideNode(dividedRatio, roomMinXSize, roomMinYSize)) // ★        
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
            treeList[x].CreateRoom();

            if (treeList[x].isDivided == false) // 잘려지지 않은 요소 == 최하단 요소
            {   
                for (int pointY = treeList[x].roomBL.y; pointY <= treeList[x].roomTR.y; pointY++)
                {
                    //print(pointY);
                    print($"왼쪽 아래 {treeList[x].bottomLeft.x}");
                    print($"오른쪽 위{treeList[x].roomTR.x}");
                    for (int pointX = treeList[x].roomBL.x; pointX <= treeList[x].roomTR.x; pointX++)
                    {
                        print(1);
                        if (pointX == treeList[x].roomBL.x || pointX == treeList[x].roomTR.x || pointY == treeList[x].roomBL.y || pointY == treeList[x].roomTR.y)
                        {
                            map[pointY, pointX] = 1;
                            
                        }   // room의 최외곽은 1, 즉 벽으로 설정 
                        else
                            map[pointY, pointX] = 3; // room의 최외곽이 아닌 부분들은 빈 공간으로 설정
                    }
                }
                roomList.Add(treeList[x]);
            }
        }

        int count = 0;

        for(int y = 0; y < map.GetLength(0); y++)
        {
            for(int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    count++;
                }
            }
        }
    }

    
    void CreateTileMap()
    {
        for(int y = 0; y < map.GetLength(0); y++)
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

    }
    
}
