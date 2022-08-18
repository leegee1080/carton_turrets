using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : PickUps
{
    public Sprite[] Sprites;
    public int ExpAmount;

    public override void Activate(IPassableObject obj)
    {
        base.Activate(obj);
        
    }
}
