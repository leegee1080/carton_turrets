using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Objects/New Enemy")]
public class EnemyScriptableObject : ActorDataScriptableObject
{
    public Sprite Sprite;
    public float SpriteSize;
    public float MaxDamage;
    public float ColliderSize;
    public PickupTypes DeathDrop;
    public int DeathDropAmount = 1;
    public EnemyRespawnType RespawnType;
    public EnemyMovementType MovementType;
    public AnimationCurve WalkingCurve;
}
