using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public enum AmmoType
{
    Pistol,
    ShotGun,
    Rifle,
}

[Serializable]
public class AmmoSlot
{
    public AmmoType ammoType;
    public int ammoAmount;
    public float shotDelay;
}



public class Gun : Weapon
{
    [SerializeField] Transform shotPoint;
    [SerializeField] AmmoSlot[] ammoSlots;
    [SerializeField] float reloadDelay;
    [SerializeField] float shotDelay;
    [SerializeField] float gunRecoil;
    /*
    SpriteRenderer sprite;
    AmmoType curType;
    */

    bool canShoot = true;


    protected override void Awake()
    {
        base.Awake();

        /*
        sprite = GetComponentInChildren<SpriteRenderer>();

        curType = AmmoType.Pistol;
        */
    }

    private void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();


    }

    public void Shoot(Player player)
    {
        StartCoroutine(Fire(player));
    }

    IEnumerator Fire(Player player)
    {
        if(canShoot == true)
        {
            canShoot = false;
            int dir = player.Dir;

            player.Rb.AddForce(Vector3.left * dir * gunRecoil, ForceMode2D.Impulse); // ÃÑ±â ¹Ýµ¿

            GameObject shotParticle = ObjectPoolManager.Instance.GetObject(PoolType.ShotParticle);
            shotParticle.transform.position = shotPoint.transform.position + new Vector3(1.5f * player.Dir, 0, 0);
            shotParticle.transform.localScale = player.transform.localScale * 0.4f;

            GameObject bullet = ObjectPoolManager.Instance.GetObject(PoolType.Bullet);
            bullet.transform.position = shotPoint.transform.position;


            yield return new WaitForSeconds(shotDelay);

            canShoot = true;
        }
        
    }
    
    /*
    public void SwitchCurrentAmmoSlot()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curType = AmmoType.Pistol;
        }

        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            curType = AmmoType.ShotGun;
        }

        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            curType = AmmoType.Rifle;
        }
    }
    */

   
}
