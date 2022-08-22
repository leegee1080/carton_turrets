using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : StageActor, IPassableObject
{
    [SerializeField] private TurretScriptableObject TurretData;
    [SerializeField] private GameObject _barrel;
    public PlayerActor ControllingActor;

    [Header("Turret Stats")]
    public float LifeTime;
    public float ReloadTime;
    public float ReloadCountdown;
    public int Ammo;

    [Header("Bullet Stats")]
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;

    [Header("Explosion Stats")]
    public float ELifeTime;
    public float EDamage;
    public float ESpeed;
    public float ESize;


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
        Ammo -= 1;

        GameObject bullet = ControllingActor.BulletObjectPools[TurretData.name].ActivateNextObject(this);
        bullet.transform.position = _barrel.transform.position;
        bullet.transform.rotation = this.gameObject.transform.rotation;
        if(Ammo <= 0){ChangeState(new TurretState_Dead());}
    }

    public void BuildTurret(IPassableObject Player)
    {
        ControllingActor = (PlayerActor)Player;

        LifeTime = ControllingActor.TurretsEquipped[TurretData.name].TLifeTime * ControllingActor.CurrentTurretBonusLifeTime;     
        ReloadTime = ControllingActor.TurretsEquipped[TurretData.name].TReloadTime * ControllingActor.CurrentTurretBonusShootSpeed;     
        ReloadCountdown = ControllingActor.TurretsEquipped[TurretData.name].TReloadTime;     
        Ammo = (int)(ControllingActor.TurretsEquipped[TurretData.name].TAmmo * ControllingActor.CurrentTurretBonusAmmo);
        _collider.radius = ControllingActor.TurretsEquipped[TurretData.name].TColliderSize;


        BLifeTime = ControllingActor.TurretsEquipped[TurretData.name].BLifeTime; 
        BDamage = ControllingActor.TurretsEquipped[TurretData.name].BDamage; 
        BSpeed = ControllingActor.TurretsEquipped[TurretData.name].BSpeed; 


        ELifeTime = ControllingActor.TurretsEquipped[TurretData.name].ELifeTime; 
        EDamage = ControllingActor.TurretsEquipped[TurretData.name].EDamage; 
        ESpeed = ControllingActor.TurretsEquipped[TurretData.name].ESpeed; 
        ESize = ControllingActor.TurretsEquipped[TurretData.name].ESize; 

        _turretMesh.mesh = ControllingActor.TurretsEquipped[TurretData.name].Mesh;
        Setup();
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
        GameObject explo = ControllingActor.ExplosionObjectPools[TurretData.name].ActivateNextObject(this);
        explo.transform.position = _barrel.transform.position;
        explo.transform.rotation = this.gameObject.transform.rotation;
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
        
        if(tu.LifeTime > 0){tu.LifeTime -= Time.fixedDeltaTime;}else{tu.ChangeState(new TurretState_Dead());}

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