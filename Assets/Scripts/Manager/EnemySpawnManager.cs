using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    private enum SpawnType
    {
        Snake = PoolType.Snake,
        Bat = PoolType.Bat,
        Yeti = PoolType.Yeti,
        CaveMan = PoolType.CaveMan,
    }

    Dictionary<SpawnType, Transform> enemyParents = new();

    private IEnumerator Start()
    {
        yield return null;

        for (int i = 0; i < Enum.GetValues(typeof(SpawnType)).Length; i++)
        {

        }

        foreach (BSPNode node in MapGenerator.Instance.RoomList)
        {
            GenerateBats(node);
            GenerateRandom(node);
        }
    }

    void GenerateBats(BSPNode node)
    {
        if (node.topRight.y != MapGenerator.Instance.TopRight.y)
        {
            int generateCount = UnityEngine.Random.Range(0, 3);

            for (int count = 0; count < generateCount; count++)
            {
                float posX = UnityEngine.Random.Range(node.roomBottomLeft.x + 1 + MapGenerator.Instance.ExtendedWidth,
                node.roomTopRight.x - 1 + MapGenerator.Instance.ExtendedWidth);

                float posY = node.roomBottomLeft.y + 2 + MapGenerator.Instance.ExtendedHeight;
                /*
                float roofY = node.roomTopRight.y - 0.5f + MapGenerator.Instance.ExtendedHeight;
                */
                
                float dist = node.roomTopRight.y - node.roomBottomLeft.y + 1 + MapGenerator.Instance.ExtendedHeight;
                int layer = LayerMask.GetMask("Ground");
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(posX, posY), Vector2.up, dist);
                
                posX = hit.point.x;
                posY = hit.point.y - 0.25f;

                int coordX = (int)Mathf.Ceil((int)posX);
                int coordY = (int)Mathf.Ceil((int)posY);

                if (MapGenerator.Instance.ExtendedMap[coordY + 1, coordX] != 1) { return; }

                GameObject bat = ObjectPoolManager.Instance.GetObject((PoolType)SpawnType.Bat);
                bat.transform.position = new Vector2(posX, posY);
            }
        }
    }

    void GenerateRandom(BSPNode node)
    {
        if(node.isStartPoint == true) { return; }

        List<Vector2> posList = new List<Vector2>();

        int genNum = UnityEngine.Random.Range(0, 4);

        if(genNum <= 0) { return; }

        for(int count = 0; count < genNum; count++)
        {
            float posX = UnityEngine.Random.Range(node.roomBottomLeft.x + 1 + MapGenerator.Instance.ExtendedWidth,
                node.roomTopRight.x - 1 + MapGenerator.Instance.ExtendedWidth);

            float posY = node.roomTopRight.y - 1 + MapGenerator.Instance.ExtendedHeight;

            float dist = node.roomTopRight.y - node.roomBottomLeft.y + 1;
            int layer = LayerMask.GetMask("Ground");
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(posX, posY), Vector2.down, dist, layer);

            posX = hit.point.x;
            posY = hit.point.y + 1;

            posList.Add(new Vector2(posX, posY));
        }

        PoolType genType = 0;

        foreach (Vector2 pos in posList)
        {
            int idx = UnityEngine.Random.Range(0, Enum.GetValues(typeof(SpawnType)).Length);

            switch (idx)
            {
                case 1:
                    genType = PoolType.Snake;
                    break;
                case 2:
                    genType = PoolType.Yeti;
                    break;
                case 3:
                    genType = PoolType.CaveMan;
                    break;
                default:
                    break;
            }

            if (genType == 0) { return; }

            GameObject enemy = ObjectPoolManager.Instance.GetObject(genType);
            enemy.transform.position = pos;
        }
    }

    
}
