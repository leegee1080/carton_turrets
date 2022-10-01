using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// public enum PlayerUpgradeEquipTypes
// {
//     none,
//     //upgrade types
//     Speed,
//     Health,
//     MaxHealth,
//     ExpGatherRange,
//     ExpMultiplier,
//     AbilityCooldown,
//     TurretShootSpeed,
//     TurretAmmo,
//     BulletLifetime,
//     money,
// }
public enum PlayerUpgradeActivateTypes
{
    none,
    PlayerSpeedBoost,
    PlaceTurret
}

[Serializable]
public struct UpgradeTier
{
    public string TierDesc;
    public PlayerStatEnum EquipFunc;
    public PlayerUpgradeActivateTypes ActivateFunc;
    public float amt;
}

public class PublicUpgradeClasses
{
    public static readonly Dictionary<PlayerStatEnum, Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable, bool>> PlayerUpgradeEquipFuncDict = new Dictionary<PlayerStatEnum, Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable, bool>>
    {
        {PlayerStatEnum.none, UpgradeNull},
        //upgrade func
        {PlayerStatEnum.CurrentSpeed, UpgradeIncreasePlayerSpeed},
        {PlayerStatEnum.CurrentHealth, UpgradeIncreasePlayerHealth},
        {PlayerStatEnum.MaxHealth, UpgradeIncreasePlayerMaxHealth},
        {PlayerStatEnum.ExpGatherRange, UpgradeIncreasePlayerExpGatherRange},
        {PlayerStatEnum.ExpMultiplier, UpgradeIncreasePlayerExpMultiplier},
        {PlayerStatEnum.CurrentAbilityCooldown, UpgradeIncreasePlayerAbilityCooldown},
        {PlayerStatEnum.CurrentTurretBonusShootSpeed, UpgradeIncreasePlayerTurretShootSpeed},
        {PlayerStatEnum.CurrentBulletLifetimeBonus, UpgradeIncreasePlayerBulletLifetime},
        {PlayerStatEnum.CurrentTurretBonusAmmo, UpgradeIncreasePlayerTurretAmmo},
        {PlayerStatEnum.money, GiveMoney},
    };

    public static readonly Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>> PlayerEquipmentActivateFuncDict = new Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>>
    {
        {PlayerUpgradeActivateTypes.none, ActivateNull},
        //activate func
        {PlayerUpgradeActivateTypes.PlayerSpeedBoost, ActivateSpeedBoost},
        {PlayerUpgradeActivateTypes.PlaceTurret, PlaceTurret}
    };
 
#region EqiupFuncs
    public static void UpgradeNull(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        return;
    }
    public static void UpgradeIncreasePlayerSpeed(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.CurrentSpeed] += value;
    }
    public static void UpgradeIncreasePlayerHealth(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        if(statDictToEffect[PlayerStatEnum.CurrentHealth] + value >= StageController.singlton.Player.PlayerCurrentStatDict[PlayerStatEnum.MaxHealth])
        {
            statDictToEffect[PlayerStatEnum.CurrentHealth] = StageController.singlton.Player.PlayerCurrentStatDict[PlayerStatEnum.MaxHealth];
            return;
        }
        statDictToEffect[PlayerStatEnum.CurrentHealth] += value;
    }
    public static void UpgradeIncreasePlayerMaxHealth(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.MaxHealth] += value;
    }
    public static void UpgradeIncreasePlayerExpGatherRange(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.ExpGatherRange] += value;
        if(testApply){return;}
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    public static void UpgradeIncreasePlayerExpMultiplier(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.ExpMultiplier] += value;
    }
    public static void UpgradeIncreasePlayerAbilityCooldown(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.CurrentAbilityCooldown] += value;
    }
    public static void UpgradeIncreasePlayerTurretShootSpeed(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.CurrentTurretBonusShootSpeed] += value;
    }
    public static void UpgradeIncreasePlayerBulletLifetime(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.CurrentBulletLifetimeBonus] += value;
    }
    public static void UpgradeIncreasePlayerTurretAmmo(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.CurrentTurretBonusAmmo] += value;
    }
    public static void GiveMoney(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData, bool testApply)
    {
        statDictToEffect[PlayerStatEnum.money] += value;
        if(testApply){return;}
        StageMoneyEarnedIndicatorUI.singlton.UpdateMoneyAmountUI((int)value);
    }
#endregion


#region ActivateFuncs
    public static void ActivateNull(int slot, IUpgradeable turretSO)
    {
        return;
    }
    public static void ActivateSpeedBoost(int slot, IUpgradeable turretSO)
    {
        //start speedboost coroutine
    }
    public static void PlaceTurret(int slot, IUpgradeable turretSO)
    {
        StageController.singlton.Player.PlaceTurret(slot);
    }
#endregion

    public static void PutUpgradeInFirstOpenSlot(float value, IUpgradeable upgradeable)
    {
        PlayerActor pd = StageController.singlton.Player;

        UpgradeSlot newUpgrade = new UpgradeSlot();
        newUpgrade.name = upgradeable.UpgradeName;
        newUpgrade.SO = upgradeable;
        newUpgrade.Tier = 0;
        newUpgrade.MaxAllowedTier = newUpgrade.SO.Tiers.Length -1;

        int OpenSlotIndex = StageController.singlton.Player.ReturnPlayerFirstUpgradableSlot(upgradeable.UpgradeType);

        switch (upgradeable.UpgradeType)
        {
            case UpgradeType.PlayerUpgrade:
                //apply to first open slot
                if(OpenSlotIndex < 0){return;}
                pd.CurrentUpgradesArray[OpenSlotIndex] = newUpgrade;
                CurrentUpgradesUI.singlton.UpdateUpgradeUI(OpenSlotIndex, newUpgrade.SO.Icon, newUpgrade.name, newUpgrade.Tier.ToString());
                break;
            case UpgradeType.Equipment:
                TurretScriptableObject TSO = upgradeable as TurretScriptableObject;
                
                //apply to first open slot
                if(OpenSlotIndex < 0){return;}

                pd.CurrentEquipmentArray[OpenSlotIndex] = newUpgrade;
                pd.TurretObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
                pd.BulletObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
                pd.ExplosionObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);

                CurrentEquipmentUI.singlton.UpdateUpgradeUI(OpenSlotIndex, newUpgrade.SO.Icon, newUpgrade.name, newUpgrade.Tier.ToString());
                break;
            case UpgradeType.TurretMod:
                break;
            default:
                return;
        }
    }
}
