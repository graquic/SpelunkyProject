using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipHitParticle : FixedPosParticles
{
    protected override void Awake()
    {
        base.Awake();
        thisPoolType = PoolType.WhipHitParticle;
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
}
