using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public enum BombState
{
    BombStart,
    Boom,
}
public class Bomb : Item
{
    Animator animator;

    [SerializeField] int boomRange;
    [SerializeField] int boomDmg;

    bool isBoom;

    [SerializeField] SpriteRenderer sprite;
    [SerializeField] ParticleSystem boomParticle;
    [SerializeField] Animation boomAnimation;
    Tilemap platformTileMap;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        platformTileMap = MapGenerator.Instance.PlatformTileMap;
        StartCoroutine(CheckStateInStart());
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        ResetAnimation();
        ObjectPoolManager.Instance.ReturnObject(PoolType.Bomb, gameObject);
    }


    IEnumerator CheckStateInStart()
    {
        while(true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                sprite.enabled = false;
                boomParticle.gameObject.SetActive(true);
                boomParticle.Stop();
                boomParticle.Play();

                Boom();
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(boomParticle.main.duration);

        boomParticle.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void Boom()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, boomRange);

        if (colliders != null)
        {
            foreach (Collider2D collider in colliders)
            {
                if(collider.isTrigger == true) { continue; }

                if (collider.TryGetComponent<Player>(out Player player))
                {
                    player.TakeDamage(boomDmg);
                }

                else if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(boomDmg);
                }

                else if (collider.tag == "Ground")
                {
                    
                    for (int y = -boomRange - 3; y < boomRange + 3; y++)
                    {
                        for (int x = -boomRange - 3; x < boomRange + 3; x++)
                        {
                            int posX = (int)transform.position.x + x;
                            int posY = (int)transform.position.y + y;
                            float dist = Vector2.Distance(new Vector2(posX, posY), transform.position);

                            if (dist < boomRange)
                            {
                                platformTileMap.SetTile(new Vector3Int(posX, posY, 0), null);
                            }
                        }
                    }
                }
            }
        }

    }

    void ResetAnimation()
    {
        // animation["Start"].normalizedTime = 0f;
    }



}
