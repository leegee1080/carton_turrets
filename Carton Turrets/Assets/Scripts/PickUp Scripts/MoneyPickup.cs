using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : PickUps
{
    public Sprite[] Sprites;
    [SerializeField]private int BaseMoneyAmount;
    [SerializeField]private int MoneyMulti;
    public int MoneyAmount;
    [SerializeField]private ParticleSystem _moneyPS;

    public override void Activate(IPassableObject obj)
    {
        base.Activate(obj);
        
    }
}
