using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridData
{
    public GridData(int x, int y, GameObject gridObj, float actualX, float actualY)
    {
        X = x;
        Y = y;
        GridObj = gridObj;
        ActualX = actualX;
        ActualY = actualY;
    }

    public int X;
    public int Y;
    public GameObject GridObj;
    public string TileType;
    public bool Locked;
    public float ActualX;
    public float ActualY;

    public override string ToString() => $"({X}, {Y}) GameObject: {GridObj} ({ActualX}, {ActualY})";
}

public class StageController : MonoBehaviour
{
    public static StageController singlton;
    public StagePackageScriptableObject CurrentStage;

    public GridData[,] GridArray;
    public PlayerActor Player;

    [SerializeField]private GameObject PoolTilesContainer;
    public ObjectPooler TilesObjectPooler;

    private void Awake() => singlton = this;


    private void Start()
    {
        
        GridSetup();
        StageObjectPoolsSetup();
        PlayerSetup();
    }

    // void OnDrawGizmosSelected()
    // {
    //     for (int x = 0; x < GridArray.GetLength(0); x++)
    //     {
    //         for (int y = 0; y < GridArray.GetLength(1); y++)
    //         {
    //             Gizmos.color = Color.yellow;
    //             Gizmos.DrawSphere(new Vector3(GridArray[x,y].ActualX,0, GridArray[x,y].ActualY), 0.1f);
    //         }
    //     }
    // }

    private void GridSetup()
    {
        GridArray = new GridData[CurrentStage.MapMaxX, CurrentStage.MapMaxY];

        for (int x = 0; x < GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < GridArray.GetLength(1); y++)
            {
                GridArray[x,y] = new GridData(x, y, null, x*CurrentStage.GridSpacing , y*CurrentStage.GridSpacing);
                
                if(CurrentStage.ImportantLocations.Length > 0)
                {
                    foreach (StagePOI item in CurrentStage.ImportantLocations)
                    {
                        if(item.LocationX == x && item.LocationY == y)
                        {
                            GridArray[x,y].GridObj = item.Object;
                        }
                    }
                }
            }
        }
    }
    private void StageObjectPoolsSetup()
    {
        if(CurrentStage.GridObjects.Length > 1)
        {
            TilesObjectPooler = new ObjectPooler(CurrentStage.GridObjects[0].SpawnableGO,CurrentStage.GridObjects[0].AmountToPool, PoolTilesContainer, true);
            for (int i = 1; i < CurrentStage.GridObjects.Length; i++)
            {
                TilesObjectPooler.PoolMoreOjects(CurrentStage.GridObjects[i].SpawnableGO, CurrentStage.GridObjects[i].AmountToPool);
            }
        }
        else
        {
            TilesObjectPooler = new ObjectPooler(CurrentStage.GridObjects[0].SpawnableGO,CurrentStage.GridObjects[0].AmountToPool, PoolTilesContainer, false);
        }
    }
    private void PlayerSetup()
    {
        GridData StartingGrid = GridArray[CurrentStage.StartingLocation.LocationX, CurrentStage.StartingLocation.LocationY];
        Vector3 StartingPos = new Vector3(StartingGrid.ActualX,0,StartingGrid.ActualY); 

        Player.gameObject.transform.position = StartingPos;

        GameObject StartingTile = Instantiate(CurrentStage.StartingLocation.Object, StartingPos, Quaternion.identity, this.transform);
        StartingGrid.GridObj = StartingTile;
        StartingGrid.Locked = true;
        StartingGrid.TileType = StartingTile.GetComponent<TileData>().TileTypeTag;
        StartingTile.GetComponent<TileData>().CurrentX = StartingGrid.X;
        StartingTile.GetComponent<TileData>().CurrentY = StartingGrid.Y;

        Player.Activate();
    }


}
