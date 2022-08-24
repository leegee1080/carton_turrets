using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UpgradeTier
{
    public PlayerUpgradeTypes func;
    public float amt;

}

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Scriptable Objects/New Player Upgrade")]
public class PlayerUpgrade : ScriptableObject, IUpgradeable
{

    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}
    [SerializeField]private UpgradeTier[] _tiers;

    public void ApplyUpgrade(int chosenTier)
    {
        PublicUpgradeClasses.PlayerUpgradeFuncDict[_tiers[chosenTier].func](_tiers[chosenTier].amt);
    }


}

