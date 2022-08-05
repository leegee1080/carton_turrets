using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo: IPassableObject
{
    public EnemyScriptableObject info;
}

public class EnemyActor : StageActor
{
    [Header("Enemy Stats")]
    public EnemyScriptableObject EnemyData;
    public float CurrentDamage;

    [Header("Target Vars")]
    public GameObject Target;
    public float ViewDistance;

    [Header("Art Vars")]
    [SerializeField]private SpriteRenderer _sR;

    [Header("Phys Vars")]
    public Rigidbody rb;

    public void Respawn()//if player gets too far
    {
        Vector3 nearPos = Target.transform.position + (StageController.singlton.Player.LastViewInput * ViewDistance);

        ActorArtContainer.transform.position = new Vector3(nearPos.x, ActorArtContainer.transform.position.y,nearPos.z);
    }
    

    public void TakeDamage(float amt)
    {
        CurrentHealth -= amt;

        if(CurrentHealth >=0 )
        {
            ChangeState(new EnemyState_Dead());
        }
    }

    public void ActivateEnemy(IPassableObject info)
    {
        EnemyInfo ei = (EnemyInfo)info;
        EnemyData = ei.info;
        Activate();
    }


    public override void Setup()
    {
        base.Setup();
        //gather enemydata from stage controller
        Target = StageController.singlton.Player.gameObject;
        CurrentHealth = EnemyData.MaxHealth;
        CurrentSpeed = EnemyData.MaxSpeed;
        CurrentDamage = EnemyData.MaxDamage;
        _sR.sprite = EnemyData.Sprite;
    }
    public override void Activate()
    {
        base.Activate();
        Setup();
        ChangeState(new EnemyState_Normal());
    }
    public override void Die()
    {
        base.Die();
        ActorArtContainer.SetActive(false);
    }

}

public class EnemyState_Frozen: ActorStatesAbstractClass
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
public class EnemyState_Normal: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {

    }   
    public override void OnUpdateState(StageActor _cont)
    {
        EnemyActor ea = (EnemyActor)_cont;
        if(ea.Target == null){return;}

        if(Vector3.Distance(ea.ActorArtContainer.transform.position, ea.Target.transform.position) > ea.ViewDistance){ea.Respawn(); return;}

        Vector3 v = Vector3.Normalize((ea.ActorArtContainer.transform.position - ea.Target.transform.position)) * -ea.CurrentSpeed;

        ea.rb.velocity = new Vector3(v.x, 0, v.z);


    }   
}
public class EnemyState_Dead: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        _cont.Die();
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}
public class EnemyState_Pause: ActorStatesAbstractClass
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
