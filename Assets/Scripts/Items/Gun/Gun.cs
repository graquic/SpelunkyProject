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
    public int currentAmmoAmount;
    public float shotDelay;
}



public class Gun : Weapon
{
    [SerializeField] Transform shootPoint;
    [SerializeField] AmmoSlot[] ammoSlots;
    [SerializeField] float reloadDelay;
    [SerializeField] float gunRecoil;


    SpriteRenderer sprite;
    AmmoType curType;
    bool canShoot = true;

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponentInChildren<SpriteRenderer>();

        curType = AmmoType.Pistol;
    }

    private void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        SwitchCurrentAmmoSlot();
    }
        
    public void Shoot(Player player)
    {
        player.Rb.AddForce(Vector3.left * player.transform.localScale.x * gunRecoil, ForceMode2D.Impulse);

        GameObject shotParticle = ObjectPoolManager.Instance.GetObject(PoolType.BulletParticle);
        shotParticle.transform.position = shootPoint.transform.position + new Vector3(5f * player.transform.localScale.x, 0, 0);
        shotParticle.transform.localScale = player.transform.localScale;

        ParticleSystem particle = shotParticle.GetComponent<ParticleSystem>();
        // yield return WaitForSeconds(particle.duration);
    }

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

   
}
