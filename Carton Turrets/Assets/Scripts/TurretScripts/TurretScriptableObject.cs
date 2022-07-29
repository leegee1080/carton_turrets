using UnityEngine;


[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject
{
    [Header("Turret Stats")]
    public float TLifeTime;
    public float TReloadTime;
    public int TAmmo;
    public float ExploSize;

    [Header("Bullet Stats")]
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;


    [Header("Turret Vars")]

    public int TurretAmountToPool;
    public int BulletAmountToPool;
    public GameObject TurretGameObject;
    public GameObject BulletGameObject;

    // [Header("Turret Art")]
    // public Sprite Icon;
}
