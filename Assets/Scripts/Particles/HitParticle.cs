using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticle : FixedPosParticles
{
    protected override void Awake()
    {
        base.Awake();

        thisPoolType = PoolType.HitParticle;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Update()
    {
        base.Update();
        
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    void CheckParticleName()
    {
        PoolType[] types = (PoolType[])Enum.GetValues(typeof(PoolType));

        foreach(PoolType type in types)
        {
            print(type.ToString());

            if(type.ToString() == gameObject.name)
            {
                thisPoolType = type;                
            }
        }
    }
}
