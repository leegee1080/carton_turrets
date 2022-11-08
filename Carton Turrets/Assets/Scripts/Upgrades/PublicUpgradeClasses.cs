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
    split
}

[Serializable]
public struct UpgradeTier
{
    public string TierDesc;
    [TextArea]
    public string InGameDesc;
    public PlayerStatEnum EquipFunc;
    public float amt;
    public PlayerUpgradeActivateTypes ActivateFunc;
    public TurretBuildTypes TurretBuildFunc;
    public TurretBonusClass[] TurretBuildMods;
    public TurretFireTypes TurretFireFunc;
    public TurretDeathTypes TurretDeathFunc;

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
    public static readonly Dictionary<TurretBuildTypes, Action<Turret, PlayerActor, Hashtable>> TurretBuildFuncDict = new Dictionary<TurretBuildTypes, Action<Turret, PlayerActor, Hashtable>>
    {
        {TurretBuildTypes.none, null},
        {TurretBuildTypes.normal, NormalBuild}
    };
    public static void NormalBuild(Turret t, PlayerActor p, Hashtable hashtable)
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
        t._barrel.transform.rotation = t.gameObject.transform.rotation;
        t._barrel.transform.rotation *= Quaternion.AngleAxis((-t.BulletSpreadAngle * (t.BulletsShotPerReload-1))/2, Vector3.up);


        t.BulletsShotPerReload = (int)(hashtable.Contains(PlayerStatEnum.BulletsShotPerReload) 
        ? (float)hashtable[PlayerStatEnum.BulletsShotPerReload] + t.TurretData.BulletsShotPerReload 
        : t.TurretData.BulletsShotPerReload);

        t.BulletSpreadAngle =(int)(hashtable.Contains(PlayerStatEnum.BulletSpreadAngle) 
        ? (float)hashtable[PlayerStatEnum.BulletSpreadAngle] + t.TurretData.BulletSpreadAngle 
        : t.TurretData.BulletSpreadAngle);

        t.BLifeTime =(float)(hashtable.Contains(PlayerStatEnum.CurrentBulletLifetimeBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentBulletLifetimeBonus] + t.TurretData.BLifeTime * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletLifetimeBonus] 
        : t.TurretData.BLifeTime * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletLifetimeBonus]); 
        
        t.BDamage =(float)(hashtable.Contains(PlayerStatEnum.CurrentBulletDamageBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentBulletDamageBonus] + t.TurretData.BDamage * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletDamageBonus] 
        : t.TurretData.BDamage * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletDamageBonus]); 
        
        t.BSpeed =(float)(hashtable.Contains(PlayerStatEnum.CurrentBulletSpeedBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentBulletSpeedBonus] + t.TurretData.BSpeed * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletSpeedBonus] 
        : t.TurretData.BSpeed * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletSpeedBonus]); 


        t.ELifeTime = t.TurretData.ELifeTime; 

        t.EDamage =(float)(hashtable.Contains(PlayerStatEnum.CurrentExploDamageBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentExploDamageBonus] + t.TurretData.EDamage * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploDamageBonus] 
        : t.TurretData.EDamage * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploDamageBonus]); 

        t.ESpeed =(float)(hashtable.Contains(PlayerStatEnum.CurrentExploSpeedBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentExploSpeedBonus] + t.TurretData.ESpeed * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSpeedBonus] 
        : t.TurretData.ESpeed * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSpeedBonus]); 

        t.ESize =(float)(hashtable.Contains(PlayerStatEnum.CurrentExploSizeBonus) 
        ? (float)hashtable[PlayerStatEnum.CurrentExploSizeBonus] + t.TurretData.ESize * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSizeBonus] 
        : t.TurretData.ESize * t.ControllingActor.PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSizeBonus]); 


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
        float angle = -((t.BulletSpreadAngle*(t.BulletsShotPerReload-1))/2);
        for (int i = 0; i < t.BulletsShotPerReload; i++)
        {
            GameObject bullet = t.ControllingActor.BulletObjectPools[t.TurretData.UpgradeName].ActivateNextObject(t);
            bullet.transform.position = t._barrel.transform.position;
            //bullet.transform.rotation = t._barrel.transform.rotation;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * t._barrel.transform.rotation;
            angle += t.BulletSpreadAngle;
        }
    }
#endregion

#region TurretDeathFuncs
    public static readonly Dictionary<TurretDeathTypes, Action<Turret>> TurretDeathFuncDict = new Dictionary<TurretDeathTypes, Action<Turret>>
    {
        {TurretDeathTypes.none, null},
        {TurretDeathTypes.normal, NormalDeath},
        {TurretDeathTypes.split, SplitDeath}
    };
    public static void NormalDeath(Turret t)
    {
        GameObject explo = t.ControllingActor.ExplosionObjectPools[t.TurretData.UpgradeName].ActivateNextObject(t);
        explo.transform.position = t._barrel.transform.position;
        explo.transform.rotation = t.transform.rotation;
    }
    public static void SplitDeath(Turret t)
    {
        GameObject explo = t.ControllingActor.ExplosionObjectPools[t.TurretData.UpgradeName].ActivateNextObject(t);
        explo.transform.position = t._barrel.transform.position;
        explo.transform.rotation = t.transform.rotation;

        PlayerActor p = StageController.singlton.Player;

        Debug.Log(t.currentTier );
        if(t.currentTier <= 0){return;}
        int tierDown = t.currentTier -=1;

        float angle = -((t.BulletSpreadAngle*(t.BulletsShotPerReload-1))/2);
        for (int i = 0; i < t.BulletsShotPerReload; i++)
        {
            GameObject tTurret =  p.TurretObjectPools[t.TurretData.UpgradeName].ActivateNextObject(p);
            Turret subTurretScript = tTurret.GetComponent<Turret>();

            subTurretScript.currentTier = tierDown;
            Debug.Log(tierDown);

            tTurret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * t._barrel.transform.rotation;
            tTurret.transform.position = t.transform.position + Vector3.forward;
            angle += t.BulletSpreadAngle;
        }


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
                CurrentUpgradesUI.singlton.UpdateUpgradeUI(OpenSlotIndex, newUpgrade.SO.Icon, newUpgrade.SO.UpgradeName, newUpgrade.Tier.ToString());
                break;
            case UpgradeType.Equipment:
                TurretScriptableObject TSO = upgradeable as TurretScriptableObject;
                
                //apply to first open slot
                if(OpenSlotIndex < 0){return;}

                pd.CurrentEquipmentArray[OpenSlotIndex] = newUpgrade;
                pd.TurretObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
                pd.BulletObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
                pd.ExplosionObjectPools[TSO.UpgradeName] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);

                CurrentEquipmentUI.singlton.UpdateUpgradeUI(OpenSlotIndex, newUpgrade.SO.Icon, newUpgrade.SO.UpgradeName, newUpgrade.Tier.ToString());
                break;
            case UpgradeType.TurretMod:
                break;
            default:
                return;
        }
    }
}
