using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpPickup : PickUps
{
    public Sprite[] Sprites;
    [SerializeField]private SpriteRenderer _expArtSR;
    [SerializeField]private ParticleSystem _expUpgradePS;
    [SerializeField]private int _baseExpAmount;
    [SerializeField]private int _expMulti;
    [SerializeField]private float _expMultiTimeBetweenUpgrades;
    public int ExpAmount;

    private IEnumerator _upgradeTimer;

    public override void Activate(IPassableObject obj)
    {
        base.Activate(obj);

        _expMultiTimeBetweenUpgrades = Mathf.Abs(_expMultiTimeBetweenUpgrades - StageController.singlton.GameTime) <= 0 ? 10 : Mathf.Abs(_expMultiTimeBetweenUpgrades - StageController.singlton.GameTime);

        if(_upgradeTimer != null){StopCoroutine(_upgradeTimer);}
        _upgradeTimer = UpgradeTimer();

        StartCoroutine(_upgradeTimer);
    }

    private IEnumerator UpgradeTimer()
    {
        for (int i = 0; i < 3; i++)
        {
            _expArtSR.sprite = Sprites[i];
            yield return new WaitForSeconds(_expMultiTimeBetweenUpgrades);
            ExpAmount *= _expMulti;
            _expUpgradePS.Play();
        }
    }
}
