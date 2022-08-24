using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerUpgradeTypes
{
    Speed,
    Health,
    ExpGatherRange,
    ExpMultiplier,
    TurretRecharge,
    TurretShootSpeed,
    TurretLifetime,
    TurretAmmo

}

public class PublicUpgradeClasses
{
    public static readonly Dictionary<PlayerUpgradeTypes, Action<float>> PlayerUpgradeFuncDict = new Dictionary<PlayerUpgradeTypes, Action<float>>
    {
        {PlayerUpgradeTypes.Speed, UpgradeIncreasePlayerSpeed},
        {PlayerUpgradeTypes.Health, UpgradeIncreasePlayerHealth},
        {PlayerUpgradeTypes.ExpGatherRange, UpgradeIncreasePlayerExpGatherRange},
        {PlayerUpgradeTypes.ExpMultiplier, UpgradeIncreasePlayerExpMultiplier},
        {PlayerUpgradeTypes.TurretRecharge, UpgradeIncreasePlayerTurretRecharge},
        {PlayerUpgradeTypes.TurretShootSpeed, UpgradeIncreasePlayerTurretShootSpeed},
        {PlayerUpgradeTypes.TurretLifetime, UpgradeIncreasePlayerTurretLifetime},
        {PlayerUpgradeTypes.TurretAmmo, UpgradeIncreasePlayerTurretAmmo}
    };

    public static void UpgradeIncreasePlayerSpeed(float value)
    {
        StageController.singlton.Player.CurrentSpeed += value;
    }
    public static void UpgradeIncreasePlayerHealth(float value)
    {
        StageController.singlton.Player.CurrentHealth += value;
    }
    public static void UpgradeIncreasePlayerExpGatherRange(float value)
    {
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    public static void UpgradeIncreasePlayerExpMultiplier(float value)
    {
        StageController.singlton.Player.ExpMultiplier += value;
    }
    public static void UpgradeIncreasePlayerTurretRecharge(float value)
    {
        StageController.singlton.Player.CurrentReloadTimerMax += value;
    }
    public static void UpgradeIncreasePlayerTurretShootSpeed(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusShootSpeed += value;
    }
    public static void UpgradeIncreasePlayerTurretLifetime(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusLifeTime += value;
    }
    public static void UpgradeIncreasePlayerTurretAmmo(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusAmmo += value;
    }
}
