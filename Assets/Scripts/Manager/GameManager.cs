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

    public Vector2 startPoint;
    public Vector2 StartPoint { get { return startPoint; } }

    public Vector2 endPoint;
    public Vector2 EndPoint { get { return endPoint; } }



    private int worldStageLevel;
    public int WorldStageLevel { get { return worldStageLevel; } }

    private int subStageLevel;
    public int SubStageLevel { get { return subStageLevel; } }

    [SerializeField] private int curScore;
    public int CurScore { get { return curScore; } }

    public UnityEvent hpChanged;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        worldStageLevel = 1;
        subStageLevel = 1;
        
    }

    private IEnumerator Start()
    {
        curScore = 0;

        yield return null;

        SetPlayer();

        player.transform.position = startPoint;
    }

    public void SetPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        }
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

    public void AddCurScore(int score)
    {
        curScore += score;
    }

    public void SetAttackerInfo(Enemy enemy)    
    {
        attackerInfo = enemy;
    }

    public void SetStartPoint(Vector2 startPoint)
    {
        this.startPoint = startPoint;
    }

    public void SetEndPoint(Vector2 endPoint)
    {
        this.endPoint = endPoint;
    }
}
