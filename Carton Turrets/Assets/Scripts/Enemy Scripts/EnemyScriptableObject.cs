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
}
