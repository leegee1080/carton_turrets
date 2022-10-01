using UnityEngine;
using System;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject, IUpgradeable
{
    
    //upgrade stats
    [field: SerializeField]public string UpgradeName {get; set;}
    [field: SerializeField]public string UpgradeDesc {get; set;}
    [field: SerializeField]public UpgradeType UpgradeType {get; set;}
    [field: SerializeField]public UpgradeTier[] Tiers {get; set;}
    [field: SerializeField]public bool IsUnlimited {get; set;}
    [field: SerializeField]public float Cooldown {get; set;}
    
    [Header("Turret Stats")]
    public float TReloadTime;
    [Space]
    [Header("***If TReloadTime set to -1, then the reload time will be the BulletLifeTime.***")]
    public int TAmmo;
    [Space]
    [Header("***If TAmmo set to negative number, then the ammo cannot be higher than the inverse.***")]
    public float TColliderSize;

    [Header("Bullet Stats")]
    public int BulletsShotPerReload;
    public int BulletSpreadAngle;
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;

    [Header("Explo Stats")]
    public float ELifeTime;
    public float EDamage;
    public float ESpeed;
    public float ESize;

    [Header("Turret Vars")]
    public int TurretAmountToPool;
    public int BulletAmountToPool;
    public int ExplosionAmountToPool;
    public GameObject TurretGameObject;
    public GameObject BulletGameObject;
    public GameObject ExplosionGameObject;

    [Header("Turret Art")]
    public Mesh Mesh;
    [field: SerializeField]public Sprite Icon {get; set;}



    public void ApplyUpgrade(int chosenTier)
    {
        if(chosenTier == 0)
        {
            PublicUpgradeClasses.PutUpgradeInFirstOpenSlot(0, this);
            return;
        }
        Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable, bool> chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[Tiers[chosenTier].EquipFunc];
        float upgradeAmount = Tiers[chosenTier].amt;

        chosenUpgradeFunc(upgradeAmount, StageController.singlton.Player.PlayerCurrentStatDict, this, false);
    }

    public void Activate(int chosenTier, int slot)
    {
        Action<int, IUpgradeable> chosenActivateFunc = PublicUpgradeClasses.PlayerEquipmentActivateFuncDict[Tiers[chosenTier].ActivateFunc];

        chosenActivateFunc(slot, this);
    }
}
