using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject
{

    [Header("Turret Art")]
    public Sprite Icon;
    public Mesh TurretModel;
    public Material TurretMaterial;
}
