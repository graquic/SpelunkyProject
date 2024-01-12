using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; private set; }

    Enemy attackerInfo;
    public Enemy AttackerInfo { get{ return attackerInfo; } }

    public Player player;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        
    }

    public void SetAttackerInfo(Enemy enemy)
    {
        attackerInfo = enemy;
    }
}
