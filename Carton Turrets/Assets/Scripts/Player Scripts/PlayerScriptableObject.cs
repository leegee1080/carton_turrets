using UnityEngine;


[CreateAssetMenu(fileName = "New Player Character", menuName = "Scriptable Objects/New Player Character")]
public class PlayerScriptableObject : ActorDataScriptableObject
{

    [Header("Character Vars")]
    public float MaxTurretReloadTime;

    public TurretScriptableObject StartingTurret;

    public float LevelUpThresholdMultiplier;

    // [Header("Character Art")]
    // public Sprite Icon;
}
