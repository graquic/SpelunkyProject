using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public BSPNode mainNode;

    [SerializeField] int divideRatio;

    private void Awake()
    {
        mainNode = new BSPNode(new Vector2Int(0, 0), new Vector2Int(30, 30));
    }



}
