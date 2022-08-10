using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour
{
    [SerializeField]protected SphereCollider _collider;
    // public ActorDataScriptableObject ActorData;
    public GameObject ActorArtContainer;

    public ActorStatesAbstractClass CurrentStateClass;

    [Header("Local Vars")]
    public float CurrentSpeed;
    public float CurrentHealth;

    public virtual void ChangeState(ActorStatesAbstractClass newState)
    {
        if(CurrentStateClass != null){CurrentStateClass.OnExitState(this);}
        CurrentStateClass = newState;
        CurrentStateClass.OnEnterState(this);
    }

    public virtual void OnEnable()
    {
        // Setup();
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
        ActorArtContainer.SetActive(true);
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
