using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridData
{
    public GridData(int x, int y, GameObject gridObj, float actualX, float actualY)
    {
        X = x;
        Y = y;
        GridObj = gridObj;
        ActualX = actualX;
        ActualY = actualY;
    }

    public int X { get; }
    public int Y { get; }
    public GameObject GridObj { get; }
    public float ActualX { get; }
    public float ActualY { get; }

    public override string ToString() => $"({X}, {Y}) GameObject: {GridObj} ({ActualX}, {ActualY})";
}

public class StageController : MonoBehaviour
{
    public StagePackageScriptableObject CurrentStage;

    [field: SerializeField]public GridData[][] XGridArray{get; private set;}

    private void Start()
    {
        
    }

}
