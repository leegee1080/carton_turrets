using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Scriptable Objects/New Player Upgrade")]
public class PlayerUpgrade : ScriptableObject, IUpgradeable
{

    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}
    [field: SerializeField]public UpgradeTier[] Tiers {get; set;}
    [field: SerializeField]public bool IsUnlimited {get; set;}
    [field: SerializeField]public float Cooldown {get; set;}

    public void ApplyUpgrade(int chosenTier)
    {
        Action<float, IUpgradeable> chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[Tiers[chosenTier].EquipFunc];
        float upgradeAmount = Tiers[chosenTier].amt;

        chosenUpgradeFunc(upgradeAmount, this);

        // PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[
        //     Tiers[chosenTier].EquipFunc](
        //         Tiers[chosenTier].amt, this);
    }

    public void Activate(int chosenTier, int slot)
    {
        Action<int, IUpgradeable> chosenActivateFunc = PublicUpgradeClasses.PlayerUpgradeActivateFuncDict[Tiers[chosenTier].ActivateFunc];

        chosenActivateFunc(slot, this);

        // PublicUpgradeClasses.PlayerUpgradeActivateFuncDict[
        //     Tiers[chosenTier].ActivateFunc](
        //         slot, this);
    }
}

