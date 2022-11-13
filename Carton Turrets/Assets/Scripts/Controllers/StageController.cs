using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public string TileType = "";
    public bool Locked;
    public float ActualX;
    public float ActualY;

    public override string ToString() => $"({X}, {Y}) | GameObject: {GridObj} | TileType {TileType} | ({ActualX}, {ActualY})";
}

public class StageController : MonoBehaviour
{
    public StageState CurrentState = new StageState_Setup();
    public float GameTime = 0;

    public static StageController singlton;
    public StagePackageScriptableObject CurrentStage;

    public GridData[,] GridArray;
    public PlayerActor Player;
    private PiaMainControls PlayerInputActions;
    public InputAction move, activate;
    public InputAction pause;

    
    
    public List<string> TileProbabilityList = new List<string>();


    [Header("TilePools")]
    [SerializeField]private GameObject _poolTilesContainer;
    public ObjectPooler TilesObjectPooler;
    public Dictionary<string, ObjectPooler> TilePoolsDict = new Dictionary<string, ObjectPooler>();

    [Header("EnemyPools")]
    public EnemySpawnWave[] WaveArray;
    [SerializeField]private GameObject _genericEnemyGameObject;
    [SerializeField]private GameObject _poolEnemyContainer;
    [SerializeField]private int _enemiesToPool;
    public ObjectPooler EnemyObjectPooler;

    [Header("Death Particle Pool")]
    [SerializeField]private GameObject _genericDeathParticleContainer;
    [SerializeField]private GameObject _genericDeathParticleObject;
    public ObjectPooler DeathParticlePooler;

    [Header("Pickups Pool")]
    [SerializeField]private GameObject _pickupContainer;
    [SerializeField]private GameObject _pickupObject;
    public ObjectPooler PickupPooler;


    private void Awake()
    {
        singlton = this;
        PlayerInputActions = new PiaMainControls();
    }
    public void OnEnable()
    {
        move = PlayerInputActions.MainMap.PlayerMovement;
        move.Enable();
        activate = PlayerInputActions.MainMap.PlaceTurret;
        activate.Enable();
        pause = PlayerInputActions.MainMap.Pause;
        pause.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        activate.Disable();
        pause.Disable();
    }

    public int FindActivateControlsIndex()//looks at the controls on the right side for the button pressed
    {
        int GetActivatedSlot(Vector2 v)
        {
            if (v[0] > 0) return 1;
            if (v[0] < 0) return 3;
            if (v[1] > 0) return 0;
            if (v[1] < 0) return 2;
            return -1;
        }
        int index = GetActivatedSlot(StageController.singlton.activate.ReadValue<Vector2>().normalized);
        if(index == -1){return -1;}
        return index;
    }
    public int FindMoveControlsIndex()//looks at the controls on the left side for the button pressed
    {
        int GetActivatedSlot(Vector2 v)
        {
            if (v[0] > 0) return 1;
            if (v[0] < 0) return 3;
            if (v[1] > 0) return 0;
            if (v[1] < 0) return 2;
            return -1;
        }
        int index = GetActivatedSlot(StageController.singlton.move.ReadValue<Vector2>().normalized);
        if(index == -1){return -1;}
        return index;
    }

    private void Start()
    {
        GridSetup();
        StageObjectPoolsSetup();
        PlayerSetup();

        ChangeState(new StageState_Running());
    }

    public void ChangeState(StageState newState)
    {
        if(newState.name == CurrentState.name){Debug.Log("Attempt was made to change state to same current state"); return;}

        CurrentState.OnExitState(this);

        CurrentState = newState;

        CurrentState.OnEnterState(this);

    }

    private void FixedUpdate()
    {
        CurrentState.OnUpdateState(this);
    }

    public void DropExp(Vector3 location)
    {
        GameObject p = PickupPooler.ActivateNextObject(null);
        p.transform.position = location;
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

    // void OnGUI()
    // {
    //     if (GUI.Button(new Rect(10, 10, 300, 200), "Print Grid"))
    //     {
    //         for (int x = 0; x < GridArray.GetLength(0); x++)
    //         {
    //             for (int y = 0; y < GridArray.GetLength(1); y++)
    //             {
    //                 print(GridArray[x,y]);
    //             }
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
                            GameObject NewTile = Instantiate(item.Object, new Vector3(GridArray[x,y].ActualX, 0, GridArray[x,y].ActualY), Quaternion.identity, this.transform);
                            GridArray[x,y].GridObj = NewTile;
                            GridArray[x,y].TileType = NewTile.GetComponent<TileData>().TileTypeTag;
                            NewTile.GetComponent<TileData>().CurrentX = x;
                            NewTile.GetComponent<TileData>().CurrentY = y;
                        }
                    }
                }
            }
        }
    }
    private void StageObjectPoolsSetup()
    {
        //tile objects
        for (int i = 0; i < CurrentStage.GridObjects.Length; i++)
        {
            for (int l = 0; l < CurrentStage.GridObjects[i].AmountToPool; l++)
            {
                TileProbabilityList.Add(CurrentStage.GridObjects[i].SpawnableGO.GetComponent<TileData>().TileTypeTag);
            }

            TilePoolsDict[CurrentStage.GridObjects[i].SpawnableGO.GetComponent<TileData>().TileTypeTag] 
                = new ObjectPooler(CurrentStage.GridObjects[i].SpawnableGO,CurrentStage.GridObjects[i].AmountToPool + 10, _poolTilesContainer, false);
        }

        //enemy objects
        WaveArray = (EnemySpawnWave[])CurrentStage.Waves.Clone();
        EnemyObjectPooler = new ObjectPooler(_genericEnemyGameObject, _enemiesToPool, _poolEnemyContainer, false);

        //death particles
        DeathParticlePooler = new ObjectPooler(_genericDeathParticleObject, _enemiesToPool, _genericDeathParticleContainer, false);

        //pickup particles
        PickupPooler = new ObjectPooler(_pickupObject, _enemiesToPool, _pickupContainer, false);
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
        
        Player.Setup();
        Player.Activate();//this is just to test the player
    }

    public void CheckEnemySpawnWave()
    {
        IEnumerator SpawnEnemyOnTimer(float time, EnemySpawnWave wave)
        {
            for (int s = 0; s < wave.Amount; s++)
            {
                EnemyInfo ei = new EnemyInfo();
                ei.info = wave.Enemy;
                EnemyObjectPooler.ActivateNextObject(ei);
                yield return new WaitForSeconds(time);
            }
        }

        for (int i = 0; i < WaveArray.Length; i++)
        {
            if(!WaveArray[i].Spawned && WaveArray[i].EnemySpawnStartThreshold <= GameTime)
            {
                WaveArray[i].EnemySpawnCoRoutine = SpawnEnemyOnTimer(WaveArray[i].SpawnInterval, WaveArray[i]);
                StartCoroutine(WaveArray[i].EnemySpawnCoRoutine);
                WaveArray[i].Spawned = true;
            }
        }
    }

}

public abstract class StageState
{
    public abstract string name{get;}
    public virtual void OnEnterState(StageController _cont){}
    public virtual void OnExitState(StageController _cont){}
    public virtual void OnUpdateState(StageController _cont){}
}

public class StageState_Running: StageState
{
    public override string name {get {return "run";}}
    public override void OnEnterState(StageController _cont)
    {
        
    }   
    public override void OnExitState(StageController _cont)
    {
        
    }   
    public override void OnUpdateState(StageController _cont)
    {
        _cont.GameTime += Time.fixedDeltaTime;
        GameTimeIndicatorUI.singlton.UpdateTime(_cont.GameTime);
        _cont.CheckEnemySpawnWave();
    }   
}
public class StageState_Pause: StageState
{
    public override string name {get {return "pause";}}
    public override void OnEnterState(StageController _cont)
    {
        
    }   
    public override void OnExitState(StageController _cont)
    {
        
    }   
    public override void OnUpdateState(StageController _cont)
    {

    }   
}
public class StageState_Setup: StageState
{
    public override string name {get {return "setup";}}
    public override void OnEnterState(StageController _cont)
    {
        
    }   
    public override void OnExitState(StageController _cont)
    {
        
    }   
    public override void OnUpdateState(StageController _cont)
    {

    }   
}