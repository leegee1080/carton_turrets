using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ScriptableObject
{
    [Header("Turret Text")]
    public new string name;
    public string Desc;

    [Header("Turret Art")]
    public Sprite Icon;
    public GameObject TurretModel;
}
