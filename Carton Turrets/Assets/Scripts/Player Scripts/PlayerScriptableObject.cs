using UnityEngine;


[CreateAssetMenu(fileName = "New Player Character", menuName = "Scriptable Objects/New Player Character")]
public class PlayerScriptableObject : ActorDataScriptableObject
{

    [Header("Character Stat Vars")]
    public float MaxAbilityCooldownTime;
    public float StartingPlayerExpBonus;
    public float StartingPlayerExpGatherRange;
    public float LevelUpThresholdMultiplier;


    [Header("Character Turret Vars")]
    public TurretScriptableObject StartingTurret;
    public float StartingTurretBonusShootSpeed;
    public float StartingTurretBonusLifeTime;
    public float StartingTurretBonusAmmo;

    public float StartingBulletDamageBonus;
    public float StartingBulletLifetimeBonus;
    public float StartingBulletRangeBonus;
    public float StartingBulletSpeedBonus;
    
    public float StartingExploDamageBonus;
    public float StartingExploSpeedBonus;
    public float StartingExploSizeBonus;
    public float StartingExploDamageRangeBonus;


    [Header("Character Art")]
    public Sprite InGameSprite;
}
