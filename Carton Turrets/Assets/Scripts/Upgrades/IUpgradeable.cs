using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUpgradeable
{
    //dont forget '[field: SerializeField]' when a class inherits from this interface. ie: [field: SerializeField]public Sprite Icon {get; set;}
    public Sprite Icon {get; set;}
    public string UpgradeName {get; set;}
    public void Upgrade();
}
