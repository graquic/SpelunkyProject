using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    Animator animator;
    Player player;

    [SerializeField] int checkHeight;
    int createdHeight;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        player = GameManager.Instance.player;

        CheckRoof();
    }

    void CheckRoof()
    {
        Vector2 extents = new Vector2(0.1f, checkHeight);

        Collider2D[] checkCols = Physics2D.OverlapBoxAll(player.transform.position, extents, LayerMask.NameToLayer("Ground"));

        if(checkCols == null)
        {
            createdHeight = (int) transform.position.y + checkHeight;
        }

        else
        {
            int minHeight = checkHeight + 1; // 그냥 높은 숫자

            foreach(Collider2D col in checkCols)
            {
                if(minHeight > col.transform.position.y)
                {
                    minHeight = (int) col.transform.position.y;
                }

                createdHeight = (int) transform.position.y + minHeight;
            }
        }
    }

    void CheckMaxHeight()
    {
        Vector2 extents = new Vector2(0.1f, checkHeight);

        Collider2D[] checkCols = Physics2D.OverlapBoxAll(player.transform.position, extents, LayerMask.NameToLayer("Ground"));

        if(checkCols == null)
        {

        }
    }
}
