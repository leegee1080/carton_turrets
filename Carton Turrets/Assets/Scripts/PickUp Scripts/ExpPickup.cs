using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : PickUps
{
    [SerializeField]Sprite[] _sprites;
    [SerializeField]float _expAmount;

    public override void Activate(IPassableObject obj)
    {
        base.Activate(obj);
        
    }
}
