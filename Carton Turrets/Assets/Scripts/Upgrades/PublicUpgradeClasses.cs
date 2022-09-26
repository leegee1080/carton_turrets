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
    public static readonly Dictionary<PlayerStatEnum, Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable>> PlayerUpgradeEquipFuncDict = new Dictionary<PlayerStatEnum, Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable>>
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

    public static readonly Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>> PlayerUpgradeActivateFuncDict = new Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>>
    {
        {PlayerUpgradeActivateTypes.none, ActivateNull},
        //activate func
        {PlayerUpgradeActivateTypes.PlayerSpeedBoost, ActivateSpeedBoost},
        {PlayerUpgradeActivateTypes.PlaceTurret, PlaceTurret}
    };
 
#region EqiupFuncs
    public static void UpgradeNull(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        return;
    }
    public static void UpgradeIncreasePlayerSpeed(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.CurrentSpeed] += value;
    }
    public static void UpgradeIncreasePlayerHealth(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        if(statDictToEffect[PlayerStatEnum.CurrentHealth] + value >= StageController.singlton.Player.PlayerCurrentStatDict[PlayerStatEnum.MaxHealth])
        {
            statDictToEffect[PlayerStatEnum.CurrentHealth] = StageController.singlton.Player.PlayerCurrentStatDict[PlayerStatEnum.MaxHealth];
            return;
        }
        statDictToEffect[PlayerStatEnum.CurrentHealth] += value;
    }
    public static void UpgradeIncreasePlayerMaxHealth(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.MaxHealth] += value;
    }
    public static void UpgradeIncreasePlayerExpGatherRange(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.ExpGatherRange] += value;
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    public static void UpgradeIncreasePlayerExpMultiplier(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.ExpMultiplier] += value;
    }
    public static void UpgradeIncreasePlayerAbilityCooldown(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.CurrentAbilityCooldown] += value;
    }
    public static void UpgradeIncreasePlayerTurretShootSpeed(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.CurrentTurretBonusShootSpeed] += value;
    }
    public static void UpgradeIncreasePlayerBulletLifetime(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.CurrentBulletLifetimeBonus] += value;
    }
    public static void UpgradeIncreasePlayerTurretAmmo(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        statDictToEffect[PlayerStatEnum.CurrentTurretBonusAmmo] += value;
    }
    public static void GiveMoney(float value, Dictionary<PlayerStatEnum, float> statDictToEffect, IUpgradeable passedUpgradeData)
    {
        Debug.Log("Gave Player: " + value + " doll hairs.");
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

    public static void EquipTurretInFirstOpenSlot(float value, IUpgradeable turretSO)
    {
        PlayerActor pd = StageController.singlton.Player;

        UpgradeSlot NewTurret = new UpgradeSlot();
        NewTurret.name = turretSO.UpgradeName;
        NewTurret.SO = turretSO;
        NewTurret.Tier = 0;
        NewTurret.MaxAllowedTier = NewTurret.SO.Tiers.Length -1;

        TurretScriptableObject TSO = turretSO as TurretScriptableObject;
        
        //apply to first open slot
        int OpenSlotIndex = StageController.singlton.Player.ReturnPlayerFirstUpgradableSlot();
        if(OpenSlotIndex < 0){return;}

        pd.CurrentUpgradesArray[OpenSlotIndex] = NewTurret;
        pd.TurretObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
        pd.BulletObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
        pd.ExplosionObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);

        CurrentEquipmentUI.singlton.UpdateUpgradeUI(OpenSlotIndex, NewTurret.SO.Icon, NewTurret.name, NewTurret.Tier.ToString());
    }
    public static void EquipUpgradeInFirstOpenSlot(float value, IUpgradeable upgradeSO)
    {
        PlayerActor pd = StageController.singlton.Player;

        UpgradeSlot NewUpgrade = new UpgradeSlot();
        NewUpgrade.name = upgradeSO.UpgradeName;
        NewUpgrade.SO = upgradeSO;
        NewUpgrade.Tier = 0;
        NewUpgrade.MaxAllowedTier = NewUpgrade.SO.Tiers.Length -1;

        //apply to first open slot
        int OpenSlotIndex = StageController.singlton.Player.ReturnPlayerFirstUpgradableSlot();
        if(OpenSlotIndex < 0){return;}
        // for (int i = 0; i < pd.CurrentUpgradesArray.Length; i++)
        // {
        //     if(pd.CurrentUpgradesArray[i].name != ""){continue;}
        //     pd.CurrentUpgradesArray[i] = NewUpgrade;

        //     CurrentEquipmentUI.singlton.UpdateUpgradeUI(i, NewUpgrade.SO.Icon, NewUpgrade.name, NewUpgrade.Tier.ToString());
        //     return;
        // }
        pd.CurrentUpgradesArray[OpenSlotIndex] = NewUpgrade;
        CurrentEquipmentUI.singlton.UpdateUpgradeUI(OpenSlotIndex, NewUpgrade.SO.Icon, NewUpgrade.name, NewUpgrade.Tier.ToString());
    }
}
