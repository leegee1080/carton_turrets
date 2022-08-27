using UnityEngine;


[CreateAssetMenu(fileName = "New Turret", menuName = "Scriptable Objects/New Turret")]
public class TurretScriptableObject : ActorDataScriptableObject, IUpgradeable
{
    [Header("Turret Stats")]
    public float TLifeTime;
    public float TReloadTime;
    public int TAmmo;
    public float TColliderSize;

    [Header("Bullet Stats")]
    public float BLifeTime;
    public float BDamage;
    public float BSpeed;

    [Header("Explo Stats")]
    public float ELifeTime;
    public float EDamage;
    public float ESpeed;
    public float ESize;

    [Header("Turret Vars")]
    public int TurretAmountToPool;
    public int BulletAmountToPool;
    public int ExplosionAmountToPool;
    public GameObject TurretGameObject;
    public GameObject BulletGameObject;
    public GameObject ExplosionGameObject;

    [Header("Turret Art")]
    public Mesh Mesh;
    [field: SerializeField]public Sprite Icon {get; set;}
    [field: SerializeField]public string UpgradeName {get; set;}
    [field: SerializeField]public int MaxUpgradeTier {get; set;}


    public void ApplyUpgrade(int chosenTier)
    {
        PlayerActor pd = StageController.singlton.Player;

        UpgradeSlot NewTurret = new UpgradeSlot();
        NewTurret.name = UpgradeName;
        NewTurret.SO = this;
        NewTurret.Tier = chosenTier;
        NewTurret.MaxAllowedTier = MaxUpgradeTier;

        //apply to first open slot
        for (int i = 0; i < pd.CurrentUpgradesArray.Length; i++)
        {
            if(pd.CurrentUpgradesArray[i].name != ""){continue;}
            pd.CurrentUpgradesArray[i] = NewTurret;
            EquipTurret(NewTurret, i, pd);
            return;
        }
        
    }   
    public void Activate(int chosenTier)
    {
        PlaceTurret();
    }

    private void PlaceTurret()
    {
        if(!_turretReloaded){return;}

        if(CurrentUpgradesArray[CurrentTurretIndex].name == ""){Debug.LogWarning("Could not place turret, slot chosen is empty!"); return;}

        GameObject tTurret =  TurretObjectPools[CurrentUpgradesArray[CurrentTurretIndex].name].ActivateNextObject(this);
        tTurret.transform.position = gameObject.transform.position + (LastViewInput * turretPlaceOffset);
        tTurret.transform.rotation = Quaternion.LookRotation(LastViewInput*90);


        CurrentTurretIndex+=1;
        if(CurrentTurretIndex > 2 || CurrentUpgradesArray[CurrentTurretIndex].name == ""){CurrentTurretIndex = 0;}

        _turretReloaded = false;
        IEnumerator Reload()
        {
            yield return new WaitForSeconds(CurrentReloadTimerMax);
            _turretReloaded = true;
        }
        StartCoroutine(Reload());
    }

    private void EquipTurret(UpgradeSlot newTurret, int slot, PlayerActor pd)
    {
        TurretScriptableObject TSO = newTurret.SO as TurretScriptableObject;

        pd.TurretObjectPools[pd.CurrentUpgradesArray[slot].name] = new ObjectPooler(TSO.TurretGameObject, TSO.TurretAmountToPool, pd.TurretContainer, false);
        pd.BulletObjectPools[pd.CurrentUpgradesArray[slot].name] = new ObjectPooler(TSO.BulletGameObject, TSO.BulletAmountToPool, pd.BulletContainer, false);
        pd.ExplosionObjectPools[pd.CurrentUpgradesArray[slot].name] = new ObjectPooler(TSO.ExplosionGameObject, TSO.ExplosionAmountToPool, pd.ExplosionContainer, false);

        return;    
    }
}
