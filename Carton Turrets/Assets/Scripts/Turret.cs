using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSetupPackage
{
    public TurretScriptableObject TurretTemplate;
    public TurretUpgradeScriptableObject[] TurretUpgrades;
}

public class Turret : StageActor
{

    public TurretSetupPackage PassedPackage;
    [SerializeField] MeshRenderer _mR;
    [SerializeField] MeshFilter _mF;

    public override void OnEnable()
    {
        //null
    }

    public override void Setup()
    {
        base.Setup();
        _mF.mesh = PassedPackage.TurretTemplate.TurretModel;
        _mR.material = PassedPackage.TurretTemplate.TurretMaterial;
    }



}
