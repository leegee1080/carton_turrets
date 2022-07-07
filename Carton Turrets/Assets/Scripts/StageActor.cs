using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour
{
    [SerializeField]protected BoxCollider _collider;
    [SerializeField]protected float _health;
    public ScriptableObject ActorData;

    public ActorStatesAbstractClass CurrentStateClass;

    public virtual void ChangeState(ActorStatesAbstractClass newState)
    {
        if(CurrentStateClass != null){CurrentStateClass.OnExitState(this);}
        CurrentStateClass = newState;
        CurrentStateClass.OnEnterState(this);
    }

    private void FixedUpdate()
    {
        if(CurrentStateClass != null){CurrentStateClass.OnUpdateState(this);}
    }

    public virtual void Setup()
    {

    }
    public virtual void Activate()
    {

    }
    public virtual void Die()
    {

    }
}


public abstract class ActorStatesAbstractClass
{
    public abstract void OnEnterState(StageActor _cont);
    public abstract void OnExitState(StageActor _cont);
    public abstract void OnUpdateState(StageActor _cont);
}
