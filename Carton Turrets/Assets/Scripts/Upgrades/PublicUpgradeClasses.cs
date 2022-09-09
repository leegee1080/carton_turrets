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
    MaxHealth,
    ExpGatherRange,
    ExpMultiplier,
    AbilityCooldown,
    TurretShootSpeed,
    TurretLifetime,
    TurretAmmo,
    money,
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
        {PlayerUpgradeEquipTypes.MaxHealth, UpgradeIncreasePlayerMaxHealth},
        {PlayerUpgradeEquipTypes.ExpGatherRange, UpgradeIncreasePlayerExpGatherRange},
        {PlayerUpgradeEquipTypes.ExpMultiplier, UpgradeIncreasePlayerExpMultiplier},
        {PlayerUpgradeEquipTypes.AbilityCooldown, UpgradeIncreasePlayerAbilityCooldown},
        {PlayerUpgradeEquipTypes.TurretShootSpeed, UpgradeIncreasePlayerTurretShootSpeed},
        {PlayerUpgradeEquipTypes.TurretLifetime, UpgradeIncreasePlayerTurretLifetime},
        {PlayerUpgradeEquipTypes.TurretAmmo, UpgradeIncreasePlayerTurretAmmo},
        {PlayerUpgradeEquipTypes.money, GiveMoney},
    };

    public static readonly Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>> PlayerUpgradeActivateFuncDict = new Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>>
    {
        {PlayerUpgradeActivateTypes.none, ActivateNull},
        //activate func
        {PlayerUpgradeActivateTypes.PlayerSpeedBoost, ActivateSpeedBoost},
        {PlayerUpgradeActivateTypes.PlaceTurret, PlaceTurret}
    };
 
#region EqiupFuncs
    public static void UpgradeNull(float value, IUpgradeable passedUpgradeData)
    {
        return;
    }
    public static void UpgradeIncreasePlayerSpeed(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.CurrentSpeed += value;
    }
    public static void UpgradeIncreasePlayerHealth(float value, IUpgradeable passedUpgradeData)
    {
        if(StageController.singlton.Player.CurrentHealth + value >= StageController.singlton.Player.MaxHealth)
        {
            StageController.singlton.Player.CurrentHealth = StageController.singlton.Player.MaxHealth;
            return;
        }
        StageController.singlton.Player.CurrentHealth += value;
    }
    public static void UpgradeIncreasePlayerMaxHealth(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.MaxHealth += value;
    }
    public static void UpgradeIncreasePlayerExpGatherRange(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    public static void UpgradeIncreasePlayerExpMultiplier(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.ExpMultiplier += value;
    }
    public static void UpgradeIncreasePlayerAbilityCooldown(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.CurrentAbilityCooldown += value;
    }
    public static void UpgradeIncreasePlayerTurretShootSpeed(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.CurrentTurretBonusShootSpeed += value;
    }
    public static void UpgradeIncreasePlayerTurretLifetime(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.CurrentTurretBonusLifeTime += value;
    }
    public static void UpgradeIncreasePlayerTurretAmmo(float value, IUpgradeable passedUpgradeData)
    {
        StageController.singlton.Player.CurrentTurretBonusAmmo += value;
    }
    public static void GiveMoney(float value, IUpgradeable passedUpgradeData)
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
        for (int i = 0; i < pd.CurrentUpgradesArray.Length; i++)
        {
            if(pd.CurrentUpgradesArray[i].name != ""){continue;}
            pd.CurrentUpgradesArray[i] = NewTurret;
            pd.TurretObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
            pd.BulletObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
            pd.ExplosionObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);

            CurrentEquipmentUI.singlton.UpdateUpgradeUI(i, NewTurret.SO.Icon, NewTurret.name);
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

            CurrentEquipmentUI.singlton.UpdateUpgradeUI(i, NewUpgrade.SO.Icon, NewUpgrade.name);
            return;
        }
    }
}
