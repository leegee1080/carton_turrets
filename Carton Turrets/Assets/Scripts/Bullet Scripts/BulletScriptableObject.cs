using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Scriptable Objects/New Bullet")]
public class BulletScriptableObject : ScriptableObject
{
    public GameObject BulletPrefab;
}
