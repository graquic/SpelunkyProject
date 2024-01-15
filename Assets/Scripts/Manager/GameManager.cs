using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; private set; }

    Enemy attackerInfo;
    public Enemy AttackerInfo { get{ return attackerInfo; } }

    public Player player;

    [SerializeField] private int playerHp;
    public int PlayerHp { get { return playerHp; } }

    public UnityEvent hpChanged; 

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

        playerHp = 20;
        
    }


    public void SetPlayerDamaged(int dmg)
    {
        if(playerHp - dmg <= 0)
        {
            playerHp = 0;
        }
        else
        {
            playerHp -= dmg;
        }
        
    }

    public void SetAttackerInfo(Enemy enemy)    
    {
        attackerInfo = enemy;
    }

    public void OnGetChangedHp()
    {

    }
}
