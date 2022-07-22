using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EUpgradeTypes
{
    ReloadTime
}

public interface IUpgrade
{
    void ApplyUpgrade(Turret target);
}

[CreateAssetMenu(fileName = "New Turret Upgrade", menuName = "Scriptable Objects/New Turret Upgrade")]
public class TurretUpgradeScriptableObject : ScriptableObject, IUpgrade
{
    [Header("Upgrade Text")]
    public new string name;
    public string Desc;

    [Header("Upgrade Vars")]
    public EUpgradeTypes UpgradeType;
    public float UpgradeAmount;
    public Action<float,Turret> StatChangeFunc;

    // [Header("Upgrade Art")]
    // public Sprite Icon;

    public void ApplyUpgrade(Turret target)
    {
        StatChangeFunc = TurretUpgradeFunctions.TurretUpgradeFuncDict[UpgradeType];
        StatChangeFunc(UpgradeAmount, target);
    }
}

public static class TurretUpgradeFunctions
{
    public static readonly Dictionary<EUpgradeTypes, Action<float,Turret>> TurretUpgradeFuncDict = new Dictionary<EUpgradeTypes, Action<float,Turret>>
    {
        {EUpgradeTypes.ReloadTime, ReloadDown}
    };

    public static void ReloadDown(float amount,Turret target)
    {
        // target.player_score += amount;
    }
}

