using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : StageActor
{
    [SerializeField] private TurretScriptableObject TurretData;

    [Header("Turret Stats")]
    public float TLifeTime;
    public float ReloadTime;
    public float ReloadCountdown;
    public int Ammo;
    public float Damage;
    public StageActor ControllingActor;


    [Header("Turret Art")]
    [SerializeField]GameObject _turretArtObject;

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
        
        if(tu.TLifeTime > 0){tu.TLifeTime -= Time.fixedDeltaTime;}else{tu.Die();}

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