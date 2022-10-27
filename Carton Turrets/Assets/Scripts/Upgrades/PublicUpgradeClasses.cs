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

public enum TurretBuildTypes
{
    none,
    normal,
}
public enum TurretFireTypes
{
    none,
    normal,
}
public enum TurretDeathTypes
{
    none,
    normal,
}

[Serializable]
public struct UpgradeTier
{
    public string TierDesc;
    public PlayerStatEnum EquipFunc;
    public PlayerUpgradeActivateTypes ActivateFunc;
    public TurretBuildTypes TurretBuildFunc;
    public TurretFireTypes TurretFireFunc;
    public TurretDeathTypes TurretDeathFunc;
    public float amt;
}

public class PublicUpgradeClasses
{
 
#region EqiupFuncs

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
    public static readonly Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>> PlayerEquipmentActivateFuncDict = new Dictionary<PlayerUpgradeActivateTypes, Action<int, IUpgradeable>>
    {
        {PlayerUpgradeActivateTypes.none, ActivateNull},
        //activate func
        {PlayerUpgradeActivateTypes.PlayerSpeedBoost, ActivateSpeedBoost},
        {PlayerUpgradeActivateTypes.PlaceTurret, PlaceTurret}
    };
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

#region TurretBuildFuncs
    public static readonly Dictionary<TurretBuildTypes, Action<Turret, PlayerActor>> TurretBuildFuncDict = new Dictionary<TurretBuildTypes, Action<Turret, PlayerActor>>
    {
        {TurretBuildTypes.none, null},
        {TurretBuildTypes.normal, NormalBuild}
    };
    public static void NormalBuild(Turret t, PlayerActor p)
    {
        t.ControllingActor = p;

        if(t.TurretData.TReloadTime == -1)
        {
            t.ReloadTime = t.TurretData.BLifeTime * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletLifetimeBonus]; 
        }
        else
        {
            t.ReloadTime = t.TurretData.TReloadTime / t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentTurretBonusShootSpeed];  
        }
   
        t.ReloadCountdown = 0;  
        if(t.TurretData.TAmmo < 0)
        {
            t.Ammo = t.TurretData.TAmmo * -1;
        }
        else
        {
            t.Ammo = (int)(t.TurretData.TAmmo * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentTurretBonusAmmo]);
        }   

        t.AdjustCollider(t.TurretData.TColliderSize);

        t.BulletsShotPerReload = t.TurretData.BulletsShotPerReload;
        t.BulletSpreadAngle = t.TurretData.BulletSpreadAngle;
        t._barrel.transform.rotation = t.gameObject.transform.rotation;
        t._barrel.transform.rotation *= Quaternion.AngleAxis((-t.BulletSpreadAngle * (t.BulletsShotPerReload-1))/2, Vector3.up);
        t.BLifeTime = t.TurretData.BLifeTime * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletLifetimeBonus]; 
        t.BDamage = t.TurretData.BDamage; 
        t.BSpeed = t.TurretData.BSpeed; 

        t.ELifeTime = t.TurretData.ELifeTime; 
        t.EDamage = t.TurretData.EDamage; 
        t.ESpeed = t.TurretData.ESpeed; 
        t.ESize = t.TurretData.ESize; 


        // // // // CurrentBulletDamageBonus = PlayerData.StartingBulletDamageBonus;
        // // // // CurrentBulletRangeBonus= PlayerData.StartingBulletRangeBonus;
        // // // // CurrentBulletSpeedBonus= PlayerData.StartingBulletSpeedBonus;

        // // // // CurrentExploDamageBonus= PlayerData.StartingExploDamageBonus;
        // // // // CurrentExploSpeedBonus= PlayerData.StartingExploSpeedBonus;
        // // // // CurrentExploSizeBonus= PlayerData.StartingExploSizeBonus;
        // // // // CurrentExploDamageRangeBonus= PlayerData.StartingExploDamageRangeBonus;


        // _turretMesh.mesh = tStats.Mesh;
        t.Setup();
    }
#endregion

#region TurretFireFuncs
    public static readonly Dictionary<TurretFireTypes, Action<Turret>> TurretFireFuncDict = new Dictionary<TurretFireTypes, Action<Turret>>
    {
        {TurretFireTypes.none, null},
        {TurretFireTypes.normal, NormalFire}
    };

    public static void NormalFire(Turret t)
    {
        if(t.Ammo <= 0){t.ChangeState(new TurretState_Dead()); return;}
        t.Ammo -= 1;
        for (int i = 0; i < t.BulletsShotPerReload; i++)
        {
            GameObject bullet = t.ControllingActor.BulletObjectPools[t.TurretData.name].ActivateNextObject(t);
            bullet.transform.position = t._barrel.transform.position;
            // bullet.transform.rotation = this.gameObject.transform.rotation;
            float angle = i * t.BulletSpreadAngle;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * t._barrel.transform.rotation;
        }
    }
#endregion

#region TurretDeathFuncs
    public static readonly Dictionary<TurretDeathTypes, Action<Turret>> TurretDeathFuncDict = new Dictionary<TurretDeathTypes, Action<Turret>>
    {
        {TurretDeathTypes.none, null},
        {TurretDeathTypes.normal, NormalDeath}
    };
    public static void NormalDeath(Turret t)
    {
        GameObject explo = t.ControllingActor.ExplosionObjectPools[t.TurretData.name].ActivateNextObject(t);
        explo.transform.position = t._barrel.transform.position;
        explo.transform.rotation = t.transform.rotation;
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
