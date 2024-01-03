using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMoveable
{
    const int MOVESPEED = 3;
    public abstract void InputMove();
}
