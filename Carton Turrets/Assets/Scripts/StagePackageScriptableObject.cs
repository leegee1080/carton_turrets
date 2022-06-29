using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StagePackageGridObject //these are meant to be 5x5 tile sections
{
    public GameObject SpawnableGO;
    [Range(0,100)]
    public int AmountToPool;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "Scriptable Objects/New Stage")]
public class StagePackageScriptableObject : ScriptableObject
{
    [Header("Stage Text")]
    public new string name;
    public string Desc;
    [Header("Map Vars")]
    public int GridSpacing;
    public StagePackageGridObject[] GridObjects;
    [Header("Reward Vars")]
    public int ScoreMulti;
}
