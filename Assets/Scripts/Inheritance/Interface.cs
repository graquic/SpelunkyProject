using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

interface IHoldable
{
    public abstract void SetHoldItem(Player player);
}