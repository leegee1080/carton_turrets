using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Scriptable Objects/New Player Upgrade")]
public class PlayerUpgrade : ScriptableObject, IUpgradeable
{

    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}
    [field: SerializeField]public UpgradeTier[] Tiers {get; set;}
    [field: SerializeField]public float Cooldown {get; set;}

    public void ApplyUpgrade(int chosenTier)
    {
        PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[Tiers[chosenTier].EquipFunc](Tiers[chosenTier].amt, this);
    }

    public void Activate(int chosenTier, int slot)
    {
        PublicUpgradeClasses.PlayerUpgradeActivateFuncDict[Tiers[chosenTier].ActivateFunc](slot, this);
    }
}

