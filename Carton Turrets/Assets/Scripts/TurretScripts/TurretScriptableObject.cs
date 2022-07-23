using UnityEngine;


[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject
{

    [Header("Turret Vars")]

    public int TurretAmountToPool;
    public int BulletAmountToPool;
    public GameObject TurretGameObject;
    public GameObject BulletGameObject;

    // [Header("Turret Art")]
    // public Sprite Icon;
}
