using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyInfo: IPassableObject
{
    public EnemyScriptableObject info;
}

public enum EnemyRespawnType
{
    OffsetOnPlayerDir,
    RandomAroundPlayer
}

public enum EnemyMovementType
{
    TowardsPlayer,
    Line
}

public static class EnemyAIClass
{

    #region Respawn
    public static readonly Dictionary<EnemyRespawnType, Action<GameObject, GameObject, float>> RespawnDict = new Dictionary<EnemyRespawnType, Action<GameObject, GameObject, float>>
    {
        {EnemyRespawnType.OffsetOnPlayerDir, OffsetOnPlayerFacing},
        {EnemyRespawnType.RandomAroundPlayer, RandomAroundPlayer}
    };

    public static void OffsetOnPlayerFacing(GameObject target, GameObject enemy, float offset)
    {
        
        Vector3 nearPos = target.transform.position + (StageController.singlton.Player.LastViewInput * (offset-2));

        float ranOffset = UnityEngine.Random.Range(-100, 100);

        ranOffset = ranOffset / 100;
    
        enemy.transform.position = new Vector3(nearPos.x + ranOffset, enemy.transform.position.y,nearPos.z + ranOffset);
    }
    public static void RandomAroundPlayer(GameObject target, GameObject enemy, float offset)
    {

        float ranOffsetx = UnityEngine.Random.Range(-100, 100);
        float ranOffsetz = UnityEngine.Random.Range(-100, 100);

        ranOffsetx = (ranOffsetx / 100);
        ranOffsetz = (ranOffsetz / 100);

        Vector3 nearPos = target.transform.position + (new Vector3(ranOffsetx, 0, ranOffsetz));
        nearPos = Vector3.MoveTowards(nearPos, target.transform.position, -1 * (offset-2));


        enemy.transform.position = new Vector3(nearPos.x, enemy.transform.position.y, nearPos.z);

    }
    #endregion


    #region Movement
    public static readonly Dictionary<EnemyMovementType, Action<Vector3, EnemyActor, float>> MovementDict = new Dictionary<EnemyMovementType, Action<Vector3, EnemyActor, float>>
    {
        {EnemyMovementType.TowardsPlayer, TowardsPlayer},
        {EnemyMovementType.Line, Line}
    };

    public static void TowardsPlayer(Vector3 target, EnemyActor enemy, float speed)
    {
        Vector3 v = Vector3.Normalize((enemy.ActorArtContainer.transform.position - target)) * -speed;

        enemy.rb.velocity = new Vector3(v.x, 0, v.z);
        enemy.RotateSpriteWalkAnimation(speed: speed, size: enemy.EnemyData.SpriteSize);
        enemy.FlipSpriteCheck(v.x);
    }
    public static void Line(Vector3 target, EnemyActor enemy, float speed)
    {
        Vector3 v = Vector3.Normalize((enemy.ActorArtContainer.transform.position - target)) * -speed;

        enemy.rb.velocity = new Vector3(v.x, 0, v.z);
        enemy.RotateSpriteWalkAnimation(speed: speed, size: enemy.EnemyData.SpriteSize);
        enemy.FlipSpriteCheck(v.x);
    }
    #endregion
}

public class EnemyActor : StageActor, IColliderMessageable
{
    [Header("Enemy Stats")]
    public EnemyScriptableObject EnemyData;
    public float CurrentDamage;
    public EnemyRespawnType RespawnType;
    public EnemyMovementType MovementType;

    [Header("Target Vars")]
    public GameObject Target;
    public float ViewDistance;

    [Header("Art Vars")]
    [SerializeField]private SpriteRenderer _sR;

    [Header("Phys Vars")]
    public Rigidbody rb;
    public SphereCollider[] Colliders;

    [Header("Timers")]
    private IEnumerator _freezeTimer;

    public void Respawn()//if player gets too far
    {
        EnemyAIClass.RespawnDict[RespawnType](Target, ActorArtContainer, ViewDistance);
    }

    public void RecMessageEnter(GameObject obj)
    {

    }

    public void RecMessageStay(GameObject obj)
    {
        if(CurrentStateClass.name != "normal"){return;}
        obj.GetComponentInParent<PlayerActor>().TakeDamage(CurrentDamage);
    }
    

    public virtual void TakeDamage(float amt)
    {
        if(CurrentStateClass.name != "normal"){return;}
        if(amt < 1){return;}
        if(GlobalDataStorage.singleton.DamageNumbersOn)//shoot out damage numbers
        {   
            GameObject daGO = StageController.singlton.DamageNumberPooler.ActivateNextObject(new DamageAmountStorage(amt));
            daGO.transform.position =  ActorArtContainer.transform.position;
        }
        CurrentHealth -= amt;
        BlinkSprite();
        if(CurrentHealth <=0 )
        {
            ChangeState(new EnemyState_Dead());
        }
    }

    public virtual void Freeze(float time)
    {
        if(CurrentStateClass.name != "normal"){return;}
        ChangeState(new EnemyState_Frozen());
        if(_freezeTimer != null){StopCoroutine(_freezeTimer);}
        _freezeTimer = FreezeTimer(time);
        StartCoroutine(_freezeTimer);
        ChangeSpriteColor(false,FreezeColor);
    }

    private IEnumerator FreezeTimer(float time)
    {
        yield return new WaitForSeconds(time);
        ChangeState(new EnemyState_Normal());
        ChangeSpriteColor(true,FreezeColor);
    }

    public void ActivateEnemy(IPassableObject info)
    {
        if(CurrentStateClass != null && CurrentStateClass.name == "normal"){return;}
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
        RespawnType = EnemyData.RespawnType;
        MovementType = EnemyData.MovementType;

        float walkAniSpeed = 5f + EnemyData.MaxSpeed + UnityEngine.Random.Range(-0.1f, 0.1f);
        // _sR.material.SetFloat("_ShakeUvSpeed", walkAniSpeed);

        CurrentDamage = EnemyData.MaxDamage;
        _sR.sprite = EnemyData.Sprite;
        _sR.gameObject.transform.localScale = new Vector3(EnemyData.SpriteSize, EnemyData.SpriteSize, EnemyData.SpriteSize);
        WalkingCurve = EnemyData.WalkingCurve;
        ActorArtContainer.transform.position = new Vector3(ActorArtContainer.transform.position.x, .12f * EnemyData.SpriteSize, ActorArtContainer.transform.position.z);


        foreach (SphereCollider coll in Colliders)
        {
            coll.radius = EnemyData.ColliderSize;
        }
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

        GameObject part = StageController.singlton.DeathParticlePooler.ActivateNextObject(null);
        part.transform.position = new Vector3(ActorArtContainer.transform.position.x, 0.1f, ActorArtContainer.transform.position.z);

        for (int i = 0; i < EnemyData.DeathDropAmount; i++)
        {
            Vector3 randAdd = Vector3.zero;
            if(EnemyData.DeathDropAmount > 1)
            {
                randAdd = new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), 0, UnityEngine.Random.Range(-0.25f, 0.25f));
            }

            if(EnemyData.DeathDrop == PickupTypes.exp)//1% chance to drop money
            {
                if(UnityEngine.Random.Range(0, 100) == 0)
                {
                    StageController.singlton.DropMoney(ActorArtContainer.transform.position + randAdd);
                }
            }

            switch (EnemyData.DeathDrop)
            {
                case PickupTypes.money:
                    StageController.singlton.DropMoney(ActorArtContainer.transform.position + randAdd);
                    break;
                case PickupTypes.exp:
                    StageController.singlton.DropExp(ActorArtContainer.transform.position + randAdd);
                    break;
                default:
                    break;
            }
        }


        ActorArtContainer.transform.position = new Vector3(0, ActorArtContainer.transform.position.y, 0);

        ActorArtContainer.SetActive(false);
    }

}

public class EnemyState_Frozen: ActorStatesAbstractClass
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
public class EnemyState_Normal: ActorStatesAbstractClass
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
        EnemyActor ea = (EnemyActor)_cont;
        if(ea.Target == null){return;}
        if(Vector3.Distance(ea.ActorArtContainer.transform.position, ea.Target.transform.position) > ea.ViewDistance){ea.Respawn(); return;}

        EnemyAIClass.MovementDict[ea.MovementType](ea.Target.transform.position, ea, ea.CurrentSpeed);

        // Vector3 v = Vector3.Normalize((ea.ActorArtContainer.transform.position - ea.Target.transform.position)) * -ea.CurrentSpeed;

        // ea.rb.velocity = new Vector3(v.x, 0, v.z);

        // ea.FlipSpriteCheck(v.x);
    }   
}
public class EnemyState_Dead: ActorStatesAbstractClass
{
    public override string name {get {return "dead";}}
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
