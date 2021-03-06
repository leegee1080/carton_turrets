using UnityEngine;


[CreateAssetMenu(fileName = "New Player Character", menuName = "Scriptable Objects/New Player Character")]
public class PlayerScriptableObject : ActorDataScriptableObject
{

    [Header("Character Vars")]
    public float MaxTurretReloadTime;

    public TurretScriptableObject TurretTemplate;
    public TurretUpgradeScriptableObject[] StartingTurretUpgrades;

    // [Header("Character Art")]
    // public Sprite Icon;
}
