using UnityEngine;


[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject, IUpgradeable
{
    [Header("Turret Stats")]
    public float TLifeTime;
    public float TReloadTime;
    public int TAmmo;
    public float TColliderSize;

    [Header("Bullet Stats")]
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;

    [Header("Explo Stats")]
    public float ELifeTime;
    public float EDamage;
    public float ESpeed;
    public float ESize;

    [Header("Turret Vars")]
    public int TurretAmountToPool;
    public int BulletAmountToPool;
    public int ExplosionAmountToPool;
    public GameObject TurretGameObject;
    public GameObject BulletGameObject;
    public GameObject ExplosionGameObject;

    [Header("Turret Art")]
    public Mesh Mesh;
    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}


    public void Upgrade()
    {

    }
}
