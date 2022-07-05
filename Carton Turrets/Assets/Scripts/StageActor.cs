using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActor : MonoBehaviour
{
    [SerializeField]protected BoxCollider _collider;
    [SerializeField]protected float _health;
    public ScriptableObject Prescription;


    protected virtual void Setup()
    {

    }
    protected virtual void Activate()
    {

    }
    protected virtual void Die()
    {

    }
}
