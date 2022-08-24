using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Upgrade", menuName = "Scriptable Objects/New Player Upgrade")]
public class PlayerUpgrade : ScriptableObject, IUpgradeable
{
    private enum PlayerUpgradeTypes
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
    private static readonly Dictionary<PlayerUpgradeTypes, Action<float>> PlayerUpgradeFuncDict = new Dictionary<PlayerUpgradeTypes, Action<float>>
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

    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}

    [SerializeField]private PlayerUpgradeTypes _upgradeType;
    [SerializeField]private float _amount;

    public void ApplyUpgrade()
    {
        PlayerUpgradeFuncDict[_upgradeType](_amount);
    }


    //upgrade funcs
    private static void UpgradeIncreasePlayerSpeed(float value)
    {
        StageController.singlton.Player.CurrentSpeed += value;
    }
    private static void UpgradeIncreasePlayerHealth(float value)
    {
        StageController.singlton.Player.CurrentHealth += value;
    }
    private static void UpgradeIncreasePlayerExpGatherRange(float value)
    {
        StageController.singlton.Player.ExpPickupGameObject.GetComponent<SphereCollider>().radius += value;
    }
    private static void UpgradeIncreasePlayerExpMultiplier(float value)
    {
        StageController.singlton.Player.ExpMultiplier += value;
    }
    private static void UpgradeIncreasePlayerTurretRecharge(float value)
    {
        StageController.singlton.Player.CurrentReloadTimerMax -= value;
    }
    private static void UpgradeIncreasePlayerTurretShootSpeed(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusShootSpeed += value;
    }
    private static void UpgradeIncreasePlayerTurretLifetime(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusLifeTime += value;
    }
    private static void UpgradeIncreasePlayerTurretAmmo(float value)
    {
        StageController.singlton.Player.CurrentTurretBonusAmmo += value;
    }

}

