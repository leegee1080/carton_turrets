using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

[Serializable]
public struct UpgradeSlot
{
    public string name;
    public IUpgradeable SO;
    public int Tier;
    public int MaxAllowedTier;
}

public class PlayerActor : StageActor, IPassableObject
{
    private PiaMainControls PlayerInputActions;

    public InputAction move, activate;
    // public InputAction move, placeturret;
    public PlayerScriptableObject PlayerData;

    [Header("Progression Vars")]
    public int CurrentPlayerLevel;
    public float MaxHealth;
    public UpgradeSlot[] CurrentUpgradesArray = new UpgradeSlot[4];
    public float ExpMultiplier;
    public int CurrentExpAmount;
    public float LevelUpThreshold;
    public float LevelUpThresholdMultiplier;
    public GameObject ExpPickupGameObject;
    public float[] TimerSlotCooldowns = new float[4]{0,0,0,0};

    [Header("TurretVars")]
    public GameObject TurretContainer;
    [SerializeField]private float turretPlaceOffset;
    public float CurrentAbilityCooldown;
    public Dictionary<string, ObjectPooler> TurretObjectPools = new Dictionary<string, ObjectPooler>();
    public float CurrentTurretBonusShootSpeed;
    // public float CurrentTurretBonusLifeTime;
    public float CurrentTurretBonusAmmo;
    public float CurrentBulletDamageBonus;
    public float CurrentBulletLifetimeBonus;
    public float CurrentBulletRangeBonus;
    public float CurrentBulletSpeedBonus;
    public float CurrentExploDamageBonus;
    public float CurrentExploSpeedBonus;
    public float CurrentExploSizeBonus;
    public float CurrentExploDamageRangeBonus;


    [Header("Bullet Vars")]
    public GameObject BulletContainer;
    public Dictionary<string, ObjectPooler> BulletObjectPools = new Dictionary<string, ObjectPooler>();

    [Header("Explosion Vars")]
    public GameObject ExplosionContainer;
    public Dictionary<string, ObjectPooler> ExplosionObjectPools = new Dictionary<string, ObjectPooler>();

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
        activate = PlayerInputActions.MainMap.PlaceTurret;
        activate.Enable();
        // activate.performed += context => PlaceTurret(context);
    }
    private void OnDisable()
    {
        // placeturret.performed -= context => PlaceTurret(context);
        move.Disable();
        activate.Disable();
    }

    public void TakeDamage(float amt)
    {
        if(CurrentStateClass.name != "normal"){return;}
        CurrentHealth -= amt;
        BlinkSprite();
        if(CurrentHealth <=0 )
        {
            ChangeState(new PlayerState_Dead());
        }
    }

    public void ActivateUpgradeSlot(int i)
    {
        if(CurrentUpgradesArray[i].name == ""){Debug.LogWarning("Slot chosen is empty!"); return;}

        if(TimerSlotCooldowns[i] > 0){return;}

        TimerSlotCooldowns[i] = CurrentUpgradesArray[i].SO.Cooldown / CurrentAbilityCooldown;

        CurrentUpgradesArray[i].SO.Activate(CurrentUpgradesArray[i].Tier, i);
    }


    public void PickupItem(PickUps item)
    {
        switch (item.PickUpId)
        {
            case "Exp":
                ExpPickup t = (ExpPickup)item;
                ApplyExp(t.ExpAmount);
                return;
            
            default:
                Debug.Log("Pickup ID not found");
                return;
        }
    }

    public void ApplyExp(int xp)
    {
        CurrentExpAmount += (int)(xp * ExpMultiplier);
        CurrentExpIndicatorUI.singlton.UpdateExpAmountUI(CurrentExpAmount, (int)LevelUpThreshold);
        if(CurrentExpAmount >= LevelUpThreshold)
        {
            CurrentExpIndicatorUI.singlton.SetPrevLevelThreshold((int)LevelUpThreshold);

            LevelUpThreshold *= LevelUpThresholdMultiplier;
            LevelUpPopup.singlton.Show();
            CurrentPlayerLevel += 1;

            CurrentExpIndicatorUI.singlton.UpdateLevelCountUI(CurrentPlayerLevel, (int)LevelUpThreshold);
        }

    }

    public void PlaceTurret(int slot)
    {
        if(CurrentUpgradesArray[slot].name == ""){Debug.LogWarning("Could not place turret, slot chosen is empty!"); return;}

        GameObject tTurret =  TurretObjectPools[CurrentUpgradesArray[slot].name].ActivateNextObject(this);
        tTurret.transform.position = gameObject.transform.position + (LastViewInput * turretPlaceOffset);
        tTurret.transform.rotation = Quaternion.LookRotation(LastViewInput*90);
    }

    public void DecUpgradeSlotTimers(float time)
    {
        for (int i = 0; i < TimerSlotCooldowns.Length; i++)
        {
            if(TimerSlotCooldowns[i] > 0)
            {
                TimerSlotCooldowns[i] -= time; CurrentEquipmentUI.singlton.UpdateUpgradeTimers
                (
                    CurrentUpgradesArray[i].SO.Cooldown / CurrentAbilityCooldown,
                    i,
                    TimerSlotCooldowns[i]
                );
            }
            else
            {
                TimerSlotCooldowns[i] = 0;
            }
        }
    }


    public override void Setup()
    {
        base.Setup();
        CurrentPlayerLevel = 1;

        CurrentHealth = PlayerData.MaxHealth;
        MaxHealth = PlayerData.MaxHealth;
        CurrentSpeed = PlayerData.MaxSpeed;
        ExpMultiplier = PlayerData.StartingPlayerExpBonus;
        LevelUpThresholdMultiplier = PlayerData.LevelUpThresholdMultiplier;

        CurrentAbilityCooldown = PlayerData.MaxAbilityCooldownTime;
        CurrentTurretBonusShootSpeed = PlayerData.StartingTurretBonusShootSpeed;
        CurrentTurretBonusAmmo = PlayerData.StartingTurretBonusAmmo;

        CurrentBulletDamageBonus = PlayerData.StartingBulletDamageBonus;
        CurrentBulletLifetimeBonus = PlayerData.StartingBulletLifetimeBonus;
        CurrentBulletRangeBonus= PlayerData.StartingBulletRangeBonus;
        CurrentBulletSpeedBonus= PlayerData.StartingBulletSpeedBonus;

        CurrentExploDamageBonus= PlayerData.StartingExploDamageBonus;
        CurrentExploSpeedBonus= PlayerData.StartingExploSpeedBonus;
        CurrentExploSizeBonus= PlayerData.StartingExploSizeBonus;
        CurrentExploDamageRangeBonus= PlayerData.StartingExploDamageRangeBonus;


        SpriteRenderer s = (SpriteRenderer)MainSprite;
        s.sprite = PlayerData.InGameSprite;

        PlayerData.StartingTurret.ApplyUpgrade(0);
    }
    public override void Activate()
    {
        base.Activate();
        ChangeState(new PlayerState_Normal());
    }
    public override void Die()
    {
        base.Die();
        move.Disable();
        activate.Disable();
        GameObject part = StageController.singlton.DeathParticlePooler.ActivateNextObject(null);
        part.transform.position = new Vector3(ActorArtContainer.transform.position.x, 0.1f, ActorArtContainer.transform.position.z);
        ActorArtContainer.SetActive(false);
        ExpPickupGameObject.SetActive(false);
        PlayerHealthIndicatorUI.singlton.HideUI();
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
                int randIndex = UnityEngine.Random.Range(0, StageController.singlton.TileProbabilityList.Count);
                item.GridObj = StageController.singlton.TilePoolsDict[StageController.singlton.TileProbabilityList[randIndex]].ActivateNextObject(this);
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
    public override string name {get {return "frozen";}}
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
    public override string name {get {return "normal";}}
    public override void OnEnterState(StageActor _cont)
    {
        
    }   
    public override void OnExitState(StageActor _cont)
    {

    }   
    public override void OnUpdateState(StageActor _cont)
    {
        int GetActivatedSlot(Vector2 v)
        {
            if (v[0] > 0) return 1;
            if (v[0] < 0) return 3;
            if (v[1] > 0) return 0;
            if (v[1] < 0) return 2;
            return -1;
        }
        PlayerActor pa = (PlayerActor)_cont;


        pa.DecUpgradeSlotTimers(Time.fixedDeltaTime);

        PlayerHealthIndicatorUI.singlton.UpdateUI(pa.CurrentHealth, pa.MaxHealth);

        Vector2 vAct = pa.activate.ReadValue<Vector2>().normalized;

        int slot = GetActivatedSlot(vAct);
        if(slot != -1)
        {
            if(pa.TimerSlotCooldowns[slot] <= 0){pa.ActivateUpgradeSlot(slot);}
        }

        Vector2 v = pa.move.ReadValue<Vector2>() * pa.CurrentSpeed;
        PlayerDirectionIndicatorUI.singlton.UpdateDirectionIndicator(pa.move.ReadValue<Vector2>());
        pa.rb.velocity = new Vector3(v.x, 0, v.y);
        if(pa.transform.position != pa.LastPos)
        {
            pa.LastViewInput = Vector3.Normalize(pa.transform.position - pa.LastPos);
            pa.CheckMapTiles();
            pa.LastPos = pa.transform.position;
            pa.FlipSpriteCheck(v.x);
            pa.MainSprite.material.SetFloat("_ShakeUvSpeed", 7f);
            return;
        }
        pa.MainSprite.material.SetFloat("_ShakeUvSpeed", 0f);
    }   
}
public class PlayerState_Dead: ActorStatesAbstractClass
{
    public override string name {get {return "dead";}}
    public override void OnEnterState(StageActor _cont)
    {
        PlayerActor pa = (PlayerActor)_cont;
        pa.Die();
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
    public override string name {get {return "pause";}}
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