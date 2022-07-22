using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretSetupPackage
{
    public TurretScriptableObject TurretTemplate;
    public TurretUpgradeScriptableObject[] TurretUpgrades;

    public TurretSetupPackage(TurretScriptableObject tt, TurretUpgradeScriptableObject[] tuArray)
    {
        TurretTemplate = tt;
        TurretUpgrades = tuArray;
    }
}

public class Turret : StageActor
{

    [Header("Turret Stats")]
    public float LifeTime;
    public float ReloadTime;
    public float ReloadCountdown;
    public int Ammo;
    public float Damage;

    [Header("Turret Art")]
    [SerializeField]GameObject _turretArtObject;
    [SerializeField] TurretSetupPackage _turretPackage;
    [SerializeField] MeshRenderer _mR;
    [SerializeField] MeshFilter _mF;

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
        //shoot
        if(Ammo <= 0){ChangeState(new TurretState_Dead());}
    }

    public void BuildTurret(MonoBehaviour Player)
    {
        PlayerActor player = (PlayerActor)Player;
        _turretPackage = new TurretSetupPackage(player.StartingTurret, player.CurrentTurretUpgrades);
        Setup();
    }

    public override void Setup()
    {
        base.Setup();

        _turretArtObject.SetActive(true);
        _mF.mesh = _turretPackage.TurretTemplate.TurretModel;
        _mR.material = _turretPackage.TurretTemplate.TurretMaterial;

        LifeTime = _turretPackage.TurretTemplate.StartingLifeTime;
        ReloadTime = _turretPackage.TurretTemplate.StartingReloadTime;
        Ammo = _turretPackage.TurretTemplate.StartingAmmo;
        Damage = _turretPackage.TurretTemplate.StartingDamage;

        ChangeState(new TurretState_Normal());
    }


    public override void Die()
    {
        base.Die();
        _turretArtObject.SetActive(false);
    }
}

public class TurretState_Frozen: ActorStatesAbstractClass
{
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
        
        if(tu.LifeTime > 0){tu.LifeTime -= Time.fixedDeltaTime;}else{tu.Die();}

    }   
}
public class TurretState_Dead: ActorStatesAbstractClass
{
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