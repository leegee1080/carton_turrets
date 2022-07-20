using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretSetupPackage
{
    public TurretScriptableObject TurretTemplate;
    public TurretUpgradeScriptableObject[] TurretUpgrades;

    public TurretSetupPackage(TurretScriptableObject tt, TurretUpgradeScriptableObject[] tuArray)
    {
        TurretTemplate = tt;
        TurretUpgrades = tuArray;
    }
}

public class Turret : StageActor
{
    [SerializeField]GameObject _turretArtObject;
    [SerializeField] TurretSetupPackage _turretPackage;
    [SerializeField] MeshRenderer _mR;
    [SerializeField] MeshFilter _mF;

    public void Awake()
    {
        _turretArtObject.SetActive(false);
    }

    public override void OnEnable()
    {
        //null
    }

    public override void Setup()
    {
        base.Setup();

        _turretPackage = new TurretSetupPackage(StageController.singlton.TurretRequester.StartingTurret, StageController.singlton.TurretRequester.CurrentTurretUpgrades);

        _turretArtObject.SetActive(true);
        _mF.mesh = _turretPackage.TurretTemplate.TurretModel;
        _mR.material = _turretPackage.TurretTemplate.TurretMaterial;
    }


    public override void Die()
    {
        base.Die();
        _turretArtObject.SetActive(false);
    }
}
