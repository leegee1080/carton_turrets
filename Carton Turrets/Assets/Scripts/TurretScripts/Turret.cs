using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Turret : StageActor, IPassableObject
{
    public TurretScriptableObject TurretData;
    public GameObject _barrel;
    public PlayerActor ControllingActor;

    [Header("Turret Stats")]
    // public float LifeTime;
    public float ReloadTime;
    public float ReloadCountdown;
    public int Ammo;

    [Header("Bullet Stats")]
    public int BulletsShotPerReload;
    public int BulletSpreadAngle;
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;

    [Header("Explosion Stats")]
    public float ELifeTime;
    public float EDamage;
    public float ESpeed;
    public float ESize;

    [Header("Tier Vars")]
    public int currentTier;



    [Header("Turret Art")]
    [SerializeField]GameObject _turretArtObject;
    [SerializeField]MeshFilter _turretMesh;

    public void Awake()
    {
        _turretArtObject.SetActive(false);                                                                                                                     
    }

    public override void OnEnable()
    {
        //null
    }

    public void ApplyUpgrades()
    {

    }

    public void Fire()
    {
        int RequestedTier = 0;

        TurretFireTypes chosenFireType = TurretData.Tiers[RequestedTier].TurretFireFunc;
        if(chosenFireType == TurretFireTypes.none){return;}

        currentTier = RequestedTier;
        Action<Turret> chosenFireFunc = PublicUpgradeClasses.TurretFireFuncDict[chosenFireType];
        chosenFireFunc(this);
        
    }

    public void AdjustCollider(float newSize)
    {
        _collider.radius = newSize;
    }

    public void BuildTurret(IPassableObject Player)
    {
        PlayerActor p = (PlayerActor)Player;
        int RequestedTier =0;
        
        foreach (UpgradeSlot item in p.CurrentEquipmentArray)
        {
            if(item.name == this.TurretData.name)
            {
                RequestedTier = item.Tier;
                break;
            }
        }

        TurretBuildTypes chosenBuildType = TurretData.Tiers[RequestedTier].TurretBuildFunc;
        if(chosenBuildType == TurretBuildTypes.none){return;}

        currentTier = RequestedTier;
        Action<Turret, PlayerActor> chosenBuildFunc = PublicUpgradeClasses.TurretBuildFuncDict[chosenBuildType];
        chosenBuildFunc(this, p);
    }

    public override void Setup()
    {
        base.Setup();

        _turretArtObject.SetActive(true);

        ChangeState(new TurretState_Normal());
    }


    public override void Die()
    {
        base.Die();

        int RequestedTier = 0;

        TurretDeathTypes chosenDeathType = TurretData.Tiers[RequestedTier].TurretDeathFunc;
        if(chosenDeathType == TurretDeathTypes.none){return;}

        currentTier = RequestedTier;
        Action<Turret> chosenDeathFunc = PublicUpgradeClasses.TurretDeathFuncDict[chosenDeathType];
        chosenDeathFunc(this);

        //hide the art. this should be done no matter what the custom death is
        _turretArtObject.SetActive(false);
    }
}

public class TurretState_Frozen: ActorStatesAbstractClass
{
    public override string name {get {return "frozen";}}
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}
public class TurretState_Normal: ActorStatesAbstractClass
{
    public override string name {get {return "normal";}}
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {

    }   
    public override void OnUpdateState(StageActor _cont)
    {
        Turret tu = (Turret)_cont;

        if(tu.ReloadCountdown >0){tu.ReloadCountdown -= Time.fixedDeltaTime;}else{tu.ReloadCountdown = tu.ReloadTime; tu.Fire();}

    }   
}
public class TurretState_Dead: ActorStatesAbstractClass
{
    public override string name {get {return "dead";}}
    public override void OnEnterState(StageActor _cont)
    {
        Turret tu = (Turret)_cont;
        tu.Die();
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}
public class TurretState_Pause: ActorStatesAbstractClass
{
    public override string name {get {return "pause";}}
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}