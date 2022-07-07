using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour
{
    [SerializeField]protected BoxCollider _collider;
    [SerializeField]protected float _health;
    public ScriptableObject ActorData;


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
