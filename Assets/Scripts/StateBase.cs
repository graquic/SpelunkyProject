using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<TStateOwner> where TStateOwner : MonoBehaviour
{
    protected TStateOwner owner;

    public StateBase(TStateOwner owner)
    {
        this.owner = owner;
    }

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}


