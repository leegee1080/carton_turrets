using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;
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

[Serializable]
public enum PlayerStatEnum
{
    none,
    money,
    CurrentHealth,
    MaxHealth,
    CurrentSpeed,
    ExpMultiplier,
    ExpGatherRange,
    LevelUpThresholdMultiplier,
    CurrentAbilityCooldown,
    CurrentTurretBonusShootSpeed,
    CurrentTurretBonusAmmo,
    CurrentBulletDamageBonus,
    CurrentBulletLifetimeBonus,
    CurrentBulletRangeBonus,
    CurrentBulletSpeedBonus,
    CurrentExploDamageBonus,
    CurrentExploSpeedBonus,
    CurrentExploSizeBonus,
    CurrentExploDamageRangeBonus,
    //turret bonuses
    BulletsShotPerReload,
    BulletSpreadAngle
}

[Serializable]
public class TurretBonusClass
{
    public PlayerStatEnum StatName;
    public float StatAmount;
}

public class PlayerActor : StageActor, IPassableObject
{

    public PlayerScriptableObject PlayerData;

    [Header("Progression Vars")]
    public int CurrentPlayerLevel;

    public UpgradeSlot[] CurrentEquipmentArray = new UpgradeSlot[4];
    public UpgradeSlot[] CurrentUpgradesArray = new UpgradeSlot[4];
    public UpgradeSlot[] CurrentTurretModsArray = new UpgradeSlot[2];

    public int CurrentExpAmount;
    public float LevelUpThreshold;

    public GameObject ExpPickupGameObject;
    public float[] TimerSlotCooldowns = new float[4]{0,0,0,0};

    [Header("TurretVars")]
    public GameObject TurretContainer;
    [SerializeField]private float turretPlaceOffset;

    public Dictionary<string, ObjectPooler> TurretObjectPools = new Dictionary<string, ObjectPooler>();

    [Header("Current Player Stats")]
    public Dictionary<PlayerStatEnum, float> PlayerCurrentStatDict = new Dictionary<PlayerStatEnum, float>();


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


    public void TakeDamage(float amt)
    {
        if(CurrentStateClass.name != "normal"){return;}
        PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth] -= amt;
        CurrentHealth = PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth];
        BlinkSprite();
        if(PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth] <=0 )
        {
            ChangeState(new PlayerState_Dead());
        }
    }

    public void ActivateUpgradeSlot(int i)
    {
        if(CurrentEquipmentArray[i].name == ""){return;}

        if(TimerSlotCooldowns[i] > 0){return;}

        TimerSlotCooldowns[i] = CurrentEquipmentArray[i].SO.Cooldown / PlayerCurrentStatDict[PlayerStatEnum.CurrentAbilityCooldown];

        CurrentEquipmentArray[i].SO.Activate(CurrentEquipmentArray[i].Tier, i);
    }

    public int ReturnPlayerFirstUpgradableSlot(UpgradeType typeToSearch)
    {
        switch (typeToSearch)
        {
            case UpgradeType.PlayerUpgrade:
                for (int i = 0; i < CurrentUpgradesArray.Length; i++)
                {
                    if(CurrentUpgradesArray[i].name != ""){continue;}
                    return i;
                }
                return -1;
            case UpgradeType.Equipment:
                for (int i = 0; i < CurrentEquipmentArray.Length; i++)
                {
                    if(CurrentEquipmentArray[i].name != ""){continue;}
                    return i;
                }
                return -1;
            case UpgradeType.TurretMod:
                for (int i = 0; i < CurrentTurretModsArray.Length; i++)
                {
                    if(CurrentTurretModsArray[i].name != ""){continue;}
                    return i;
                }
                return -1;
            default:
                return -1;
        }
    }
    public UpgradeSlot[] ReturnArrayToSearchBasedOnUpgradeType(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.PlayerUpgrade:
                return CurrentUpgradesArray;
            case UpgradeType.Equipment:
                return CurrentEquipmentArray;
            case UpgradeType.TurretMod:
                return null;
            default:
                return null;
        }
    }



    public void PickupItem(PickUps item)
    {
        switch (item.PickUpId)
        {
            case "Exp":
                ExpPickup t = (ExpPickup)item;
                ApplyExp(t.ExpAmount);
                return;
            case "Money":
                MoneyPickup m = (MoneyPickup)item;
                StageMoneyEarnedIndicatorUI.singlton.UpdateMoneyAmountUI(m.MoneyAmount);
                return;
            default:
                Debug.Log("Pickup ID not found");
                return;
        }
    }

    public void ApplyExp(int xp)
    {
        CurrentExpAmount += (int)(xp * PlayerCurrentStatDict[PlayerStatEnum.ExpMultiplier]);
        CurrentExpIndicatorUI.singlton.UpdateExpAmountUI(CurrentExpAmount, (int)LevelUpThreshold);
        if(CurrentExpAmount >= LevelUpThreshold)
        {
            CurrentExpIndicatorUI.singlton.SetPrevLevelThreshold((int)LevelUpThreshold);

            LevelUpThreshold *= PlayerCurrentStatDict[PlayerStatEnum.LevelUpThresholdMultiplier];
            LevelUpPopup.singlton.Show();
            CurrentPlayerLevel += 1;

            CurrentExpIndicatorUI.singlton.UpdateLevelCountUI(CurrentPlayerLevel, (int)LevelUpThreshold);
        }

    }

    public void PlaceTurret(int slot)
    {
        if(CurrentEquipmentArray[slot].name == ""){Debug.LogWarning("Could not place turret, slot chosen is empty!"); return;}

        GameObject tTurret =  TurretObjectPools[CurrentEquipmentArray[slot].SO.UpgradeName].ActivateNextObject(this);
        tTurret.transform.position = gameObject.transform.position + (LastViewInput * turretPlaceOffset);
        tTurret.transform.rotation = Quaternion.LookRotation(LastViewInput*90);
    }

    public void DecUpgradeSlotTimers(float time)
    {
        for (int i = 0; i < TimerSlotCooldowns.Length; i++)
        {
            if(TimerSlotCooldowns[i] > 0)
            {
                TimerSlotCooldowns[i] -= time; 
                CurrentEquipmentUI.singlton.UpdateUpgradeTimers
                (
                    CurrentEquipmentArray[i].SO.Cooldown / PlayerCurrentStatDict[PlayerStatEnum.CurrentAbilityCooldown],
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

        PlayerCurrentStatDict[PlayerStatEnum.money] = 0;
        StageMoneyEarnedIndicatorUI.singlton.UpdateMoneyAmountUI((int)PlayerCurrentStatDict[PlayerStatEnum.money]);

        PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth] = PlayerData.MaxHealth;
        CurrentHealth = PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth];
        PlayerCurrentStatDict[PlayerStatEnum.MaxHealth] = PlayerData.MaxHealth;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentSpeed] = PlayerData.MaxSpeed;
        CurrentSpeed = PlayerCurrentStatDict[PlayerStatEnum.CurrentSpeed];
        PlayerCurrentStatDict[PlayerStatEnum.ExpMultiplier] = PlayerData.StartingPlayerExpBonus;
        PlayerCurrentStatDict[PlayerStatEnum.ExpGatherRange] = PlayerData.StartingPlayerExpGatherRange;
        ExpPickupGameObject.GetComponent<SphereCollider>().radius = PlayerData.StartingPlayerExpGatherRange;
        PlayerCurrentStatDict[PlayerStatEnum.LevelUpThresholdMultiplier] = PlayerData.LevelUpThresholdMultiplier;

        PlayerCurrentStatDict[PlayerStatEnum.CurrentAbilityCooldown] = PlayerData.MaxAbilityCooldownTime;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentTurretBonusShootSpeed] = PlayerData.StartingTurretBonusShootSpeed;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentTurretBonusAmmo] = PlayerData.StartingTurretBonusAmmo;

        PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletDamageBonus] = PlayerData.StartingBulletDamageBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletLifetimeBonus] = PlayerData.StartingBulletLifetimeBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletRangeBonus] = PlayerData.StartingBulletRangeBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentBulletSpeedBonus] = PlayerData.StartingBulletSpeedBonus;

        PlayerCurrentStatDict[PlayerStatEnum.CurrentExploDamageBonus] = PlayerData.StartingExploDamageBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSpeedBonus] = PlayerData.StartingExploSpeedBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentExploSizeBonus] = PlayerData.StartingExploSizeBonus;
        PlayerCurrentStatDict[PlayerStatEnum.CurrentExploDamageRangeBonus] = PlayerData.StartingExploDamageRangeBonus;


        SpriteRenderer s = (SpriteRenderer)MainSprite;
        s.sprite = PlayerData.InGameSprite;
        WalkingCurve = PlayerData.WalkingCurve;
        s.transform.localScale = new Vector3(PlayerData.SpriteSize,PlayerData.SpriteSize,PlayerData.SpriteSize);

        PlayerData.StartingTurret.ApplyUpgrade(0);
    }
    public override void Activate()
    {
        base.Activate();
        ActorArtContainer.SetActive(true);
        ChangeState(new PlayerState_Normal());
    }
    public override void Die()
    {
        base.Die();

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

        PlayerActor pa = (PlayerActor)_cont;


        pa.DecUpgradeSlotTimers(Time.fixedDeltaTime);

        PlayerHealthIndicatorUI.singlton.UpdateUI(pa.PlayerCurrentStatDict[PlayerStatEnum.CurrentHealth], pa.PlayerCurrentStatDict[PlayerStatEnum.MaxHealth]);


        int slot = StageController.singlton.FindActivateControlsIndex();
        if(slot != -1)
        {
            if(pa.TimerSlotCooldowns[slot] <= 0){pa.ActivateUpgradeSlot(slot);}
        }

        Vector2 v = StageController.singlton.move.ReadValue<Vector2>() * pa.PlayerCurrentStatDict[PlayerStatEnum.CurrentSpeed];
        PlayerDirectionIndicatorUI.singlton.UpdateDirectionIndicator(StageController.singlton.move.ReadValue<Vector2>());
        pa.rb.velocity = new Vector3(v.x, 0, v.y);
        if(pa.transform.position != pa.LastPos)
        {
            pa.LastViewInput = Vector3.Normalize(pa.transform.position - pa.LastPos);
            pa.CheckMapTiles();
            pa.LastPos = pa.transform.position;
            pa.FlipSpriteCheck(v.x);
            pa.RotateSpriteWalkAnimation(pa.PlayerCurrentStatDict[PlayerStatEnum.CurrentSpeed], size: pa.PlayerData.SpriteSize);
            return;
        }
        pa.RotateSpriteWalkAnimation(reset: true);
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