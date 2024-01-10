using System.Buffers;
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
    [SerializeField] float bouncePower;

    SpriteRenderer sprite;
    Tilemap platformTileMap;
    ParticleSystem boomParticle;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        boomParticle = GetComponentInChildren<ParticleSystem>();
        
    }

    private void Start()
    {
        platformTileMap = MapGenerator.Instance.PlatformTileMap;
        
    }

    private void OnEnable()
    {
        StartCoroutine(CheckStateInStart());
    }

    protected override void Update()
    {
        base.Update();
        boomParticle.transform.rotation = Quaternion.identity;
    }

    protected override void OnDisable()
    {
        
        sprite.enabled = true;
        transform.rotation = Quaternion.identity;
        GetComponent<Rigidbody2D>().gravityScale = 1;
        base.OnDisable();

    }


    IEnumerator CheckStateInStart()
    {
        
        while(true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                sprite.enabled = false;
                boomParticle.transform.rotation = Quaternion.identity;
                transform.parent = null;

                boomParticle.Stop();
                boomParticle.Play();

                Boom();
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(boomParticle.main.duration);

        ObjectPoolManager.Instance.ReturnObject(PoolType.Bomb, gameObject);
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
                    Vector2 dir = (player.transform.position - transform.position).normalized;

                    player.GetComponent<Rigidbody2D>().AddForce(dir * bouncePower, ForceMode2D.Impulse);

                    player.TakeDamage(boomDmg);
                }

                else if (collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    enemy.TakeDamage(boomDmg);
                }

                else if (collider.TryGetComponent<Bomb>(out Bomb bomb))
                {
                    SetEarlyBoom(bomb);
                }

                else if (collider.TryGetComponent<Item>(out Item item))
                {
                    Vector3 pos = (item.transform.position + new Vector3(0, 1.5f, 0) - transform.position).normalized;
                    item.rb.AddForce(pos * bouncePower, ForceMode2D.Impulse);
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

    public void SetEarlyBoom(Bomb bomb)
    {
        bomb.animator.Play("Start", -1, 1);
    }
   
}
