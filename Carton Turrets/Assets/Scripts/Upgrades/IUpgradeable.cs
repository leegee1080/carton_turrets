using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Equipment,
    PlayerUpgrade,
    TurretMod
}

public interface IUpgradeable
{
    //dont forget '[field: SerializeField]' when a class inherits from this interface. ie: [field: SerializeField]public Sprite Icon {get; set;}
    public Sprite Icon {get; set;}
    public string UpgradeName {get; set;}
    public UpgradeType UpgradeType{get; set;}
    public UpgradeTier[] Tiers {get; set;}
    public bool IsUnlimited {get; set;}
    public void ApplyUpgrade(int chosenTier);
    public void Activate(int chosenTier, int slot);
    public float Cooldown {get; set;}
}
