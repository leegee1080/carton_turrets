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

[Serializable]
public class StagePOI
{
    public GameObject Object;
    public int LocationX;
    public int LocationY;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "Scriptable Objects/New Stage")]
public class StagePackageScriptableObject : ScriptableObject
{
    [Header("Stage Text")]
    public new string name;
    public string Desc;
    [Header("Map Vars")]
    public int MapMaxX;
    public int MapMaxY;

    public StagePOI StartingLocation;
    public StagePOI[] ImportantLocations;
    public int GridSpacing;
    public StagePackageGridObject[] GridObjects;
    [Header("Reward Vars")]
    public int ScoreMulti;
}
