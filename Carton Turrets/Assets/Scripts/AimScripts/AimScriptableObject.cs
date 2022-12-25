using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aim", menuName = "Scriptable Objects/New Aim")]
public class AimScriptableObject : ScriptableObject
{
    public string Name;
    public string InspectorDescription;
    public Sprite Icon;
    public string InGameDescription;
}
