using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerUpgradeEquipTypes
{
    none,
    //upgrade types
    Speed,
    Health,
    ExpGatherRange,
    ExpMultiplier,
    TurretRecharge,
    TurretShootSpeed,
    TurretLifetime,
    TurretAmmo,
    //equip types
    EquipTurret,
    EquipUpgrade
}
public enum PlayerUpgradeActivateTypes
{
    none,
    PlayerSpeedBoost,
    PlaceTurret
}

[Serializable]
public struct UpgradeTier
{
    public PlayerUpgradeEquipTypes EquipFunc;
    public PlayerUpgradeActivateTypes ActivateFunc;
    public float amt;
}

public class PublicUpgradeClasses
{
    public static readonly Dictionary<PlayerUpgradeEquipTypes, Action<float, IUpgradeable>> PlayerUpgradeEquipFuncDict = new Dictionary<PlayerUpgradeEquipTypes, Action<float, IUpgradeable>>
    {
        {PlayerUpgradeEquipTypes.none, UpgradeNull},
        //upgrade func
        {PlayerUpgradeEquipTypes.Speed, UpgradeIncreasePlayerSpeed},
        {PlayerUpgradeEquipTypes.Health, UpgradeIncreasePlayerHealth},
        {PlayerUpgradeEquipTypes.ExpGatherRange, UpgradeIncreasePlayerExpGatherRange},
        {PlayerUpgradeEquipTypes.ExpMultiplier, UpgradeIncreasePlayerExpMultiplier},
        {PlayerUpgradeEquipTypes.TurretRecharge, UpgradeIncreasePlayerTurretRecharge},
        {PlayerUpgradeEquipTypes.TurretShootSpeed, UpgradeIncreasePlayerTurretShootSpeed},
        {PlayerUpgradeEquipTypes.TurretLifetime, UpgradeIncreasePlayerTurretLifetime},
        {PlayerUpgradeEquipTypes.TurretAmmo, UpgradeIncreasePlayerTurretAmmo},
        //equip func
        {PlayerUpgradeEquipTypes.EquipTurret, EquipTurretInFirstOpenSlot},
        {PlayerUpgradeEquipTypes.EquipUpgrade, EquipUpgradeInFirstOpenSlot},
    };

    public static readonly Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>> PlayerUpgradeActivateFuncDict = new Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>>
    {
        {PlayerUpgradeActivateTypes.none, ActivateNull},
        //activate func
        {PlayerUpgradeActivateTypes.PlayerSpeedBoost, ActivateSpeedBoost},
        {PlayerUpgradeActivateTypes.PlaceTurret, PlaceTurret}
    };
 
#region EqiupFuncs
    public static void UpgradeNull(float value, IUpgradeable turretSO)
    {
        return;
    }
    public static void UpgradeIncreasePlayerSpeed(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentSpeed += value;
    }
    public static void UpgradeIncreasePlayerHealth(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentHealth += value;
    }
    public static void UpgradeIncreasePlayerExpGatherRange(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    public static void UpgradeIncreasePlayerExpMultiplier(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.ExpMultiplier += value;
    }
    public static void UpgradeIncreasePlayerTurretRecharge(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentReloadTimerMax += value;
    }
    public static void UpgradeIncreasePlayerTurretShootSpeed(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentTurretBonusShootSpeed += value;
    }
    public static void UpgradeIncreasePlayerTurretLifetime(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentTurretBonusLifeTime += value;
    }
    public static void UpgradeIncreasePlayerTurretAmmo(float value, IUpgradeable turretSO)
    {
        StageController.singlton.Player.CurrentTurretBonusAmmo += value;
    }
    public static void EquipTurretInFirstOpenSlot(float value, IUpgradeable turretSO)
    {
        PlayerActor pd = StageController.singlton.Player;

        UpgradeSlot NewTurret = new UpgradeSlot();
        NewTurret.name = turretSO.UpgradeName;
        NewTurret.SO = turretSO;
        NewTurret.Tier = 0;
        NewTurret.MaxAllowedTier = NewTurret.SO.Tiers.Length -1;

        TurretScriptableObject TSO = NewTurret.SO as TurretScriptableObject;

        //apply to first open slot
        for (int i = 0; i < pd.CurrentUpgradesArray.Length; i++)
        {
            if(pd.CurrentUpgradesArray[i].name != ""){continue;}
            pd.CurrentUpgradesArray[i] = NewTurret;
            pd.TurretObjectPools[pd.CurrentUpgradesArray[i].name] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
            pd.BulletObjectPools[pd.CurrentUpgradesArray[i].name] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
            pd.ExplosionObjectPools[pd.CurrentUpgradesArray[i].name] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);
            return;
        }
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
        for (int i = 0; i < pd.CurrentUpgradesArray.Length; i++)
        {
            if(pd.CurrentUpgradesArray[i].name != ""){continue;}
            pd.CurrentUpgradesArray[i] = NewUpgrade;
            return;
        }
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
}