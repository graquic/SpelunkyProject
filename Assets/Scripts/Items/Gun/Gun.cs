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
    [SerializeField] AmmoSlot[] ammoSlots;
    [SerializeField] float reloadDelay;

    SpriteRenderer sprite;
    AmmoType curType;
    bool canShoot = true;
    Player player;

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponentInChildren<SpriteRenderer>();

        curType = AmmoType.Pistol;
    }

    private void Start()
    {
        player = GameManager.Instance.player;

        // StartCoroutine(FireCoroutine());
    }

    protected override void Update()
    {
        base.Update();

        SwitchCurrentAmmoSlot();

    }
    


    public void Shoot()
    {
        print(3);
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
