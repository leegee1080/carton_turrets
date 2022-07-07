using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player Character", menuName = "Scriptable Objects/New Player Character")]
public class PlayerScriptableObject : ScriptableObject
{
    [Header("Character Text")]
    public new string name;
    public string Desc;

    [Header("Character Vars")]
    public int MaxHealth;
    public float MaxSpeed;
    public float MaxTurretReloadTime;

    public TurretScriptableObject TurretTemplate;
    public TurretUpgradeScriptableObject[] StartingTurretUpgrades;

    // [Header("Character Art")]
    // public Sprite Icon;
}
