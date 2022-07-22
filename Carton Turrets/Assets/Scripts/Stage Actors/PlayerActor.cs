using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerActor : StageActor
{
    private PiaMainControls PlayerInputActions;
    public InputAction move, placeturret;

    [Header("TurretVars")]
    [SerializeField]private float turretPlaceOffset;
    private float _reloadTimerMax;
    private bool _turretReloaded = true;

    public TurretScriptableObject StartingTurret;
    public TurretUpgradeScriptableObject[] CurrentTurretUpgrades;
    // public Turret CurrentTurret;

    [Header("View Vars")]
    public float ViewDistance;
    public Vector3 LastViewInput;

    public Vector3 LastPos;
    
    [Header("Phys Vars")]
    public Rigidbody rb;

    private void Awake()
    {
        PlayerInputActions = new PiaMainControls();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        move = PlayerInputActions.MainMap.PlayerMovement;
        move.Enable();
        placeturret = PlayerInputActions.MainMap.PlaceTurret;
        placeturret.Enable();
        placeturret.performed += context => PlaceTurret(context);
    }
    private void OnDisable()
    {
        placeturret.performed -= context => PlaceTurret(context);
        move.Disable();
        placeturret.Disable();
    }


    private void PlaceTurret(InputAction.CallbackContext context)
    {
        if(!_turretReloaded){return;}

        GameObject tTurret =  StageController.singlton.TurretsObjectPooler.ActivateNextObject(this);
        tTurret.transform.position = gameObject.transform.position + (LastViewInput * turretPlaceOffset);
        tTurret.transform.rotation = Quaternion.LookRotation(LastViewInput*90);
        // StageController.singlton.PlaceTurret(this, gameObject.transform.position + (LastViewInput * turretPlaceOffset), LastViewInput*90);
        _turretReloaded = false;
        IEnumerator Reload()
        {
            yield return new WaitForSeconds(_reloadTimerMax);
            _turretReloaded = true;
        }
        StartCoroutine(Reload());
    }


    public override void Setup()
    {
        base.Setup();
        PlayerScriptableObject startingData = (PlayerScriptableObject)ActorData;
        StartingTurret = startingData.TurretTemplate;
        CurrentTurretUpgrades = startingData.StartingTurretUpgrades;
        _reloadTimerMax = startingData.MaxTurretReloadTime;
    }
    public override void Activate()
    {
        base.Activate();
        ChangeState(new PlayerState_Normal());
    }
    public override void Die()
    {
        base.Die();
    }

    public void CheckMapTiles()
    {
        IEnumerable query = from GridData gd in StageController.singlton.GridArray  
            where Vector3.Distance(new Vector3(gd.ActualX, 0, gd.ActualY), this.gameObject.transform.position) < ViewDistance && gd.Locked == false && gd.GridObj == null
            select gd;
        
        foreach (GridData item in query)
        {
            TileData SelectedTile = null;

            if(item.TileType != "")
            {
                item.GridObj = StageController.singlton.TilePoolsDict[item.TileType].ActivateNextObject(this);
                SelectedTile = item.GridObj.GetComponent<TileData>();
            }
            else
            {
                int randIndex = Random.Range(0, StageController.singlton.TileProbabilityList.Count);
                item.GridObj = StageController.singlton.TilePoolsDict[StageController.singlton.TileProbabilityList[randIndex]].ActivateNextObject(this);
                // item.GridObj = StageController.singlton.TilesObjectPooler.ActivateNextObject();
                SelectedTile = item.GridObj.GetComponent<TileData>();
                item.TileType = SelectedTile.TileTypeTag;
            }

            StageController.singlton.GridArray[SelectedTile.CurrentX,SelectedTile.CurrentY].GridObj = null;
            SelectedTile.CurrentX = item.X;
            SelectedTile.CurrentY = item.Y;
            item.GridObj.transform.position = new Vector3(item.ActualX, 0, item.ActualY);
        }
    }
}

public class PlayerState_Frozen: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}
public class PlayerState_Normal: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {

    }   
    public override void OnUpdateState(StageActor _cont)
    {
        PlayerActor pa = (PlayerActor)_cont;
        Vector2 v = pa.move.ReadValue<Vector2>() * pa.CurrentSpeed;

        pa.rb.velocity = new Vector3(v.x, 0, v.y);

        if(pa.transform.position != pa.LastPos)
        {
            pa.LastViewInput = Vector3.Normalize(pa.transform.position - pa.LastPos);
            pa.CheckMapTiles();
            pa.LastPos = pa.transform.position;
        }

    }   
}
public class PlayerState_Dead: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}
public class PlayerState_Pause: ActorStatesAbstractClass
{
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {
        
    }   
    public override void OnUpdateState(StageActor _cont)
    {

    }   
}