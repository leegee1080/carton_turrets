using UnityEngine;
using UnityEngine.UI;

public class DeathCoverContainer : MonoBehaviour
{
    [SerializeField]Image _img;
    [SerializeField]PlayerActor _player;
    [SerializeField]AnimationCurve _blackoutCurve;

    StagePackageScriptableObject _gridInfoSO;
    float _mapStart;
    float _mapEdge;

    [Range(0, 100)]
    [SerializeField]float _deathPercentageThreshold;
    [SerializeField]float _deathPercentageThresholdDamageAmount;


    private void Start()
    {
        _gridInfoSO = StageController.singlton.CurrentStage;
        _mapStart = _gridInfoSO.StartingLocation.LocationX * _gridInfoSO.GridSpacing;
        _mapEdge = (_gridInfoSO.MapMaxX + _gridInfoSO.MapMaxY * _gridInfoSO.GridSpacing )/2;
    }

    private void FixedUpdate()
    {
        if(_player.CurrentStateClass.name != "normal"){return;}
        
        float dist = Vector3.Distance(_player.transform.position, new Vector3(_mapStart, 0, _mapStart));
        
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, _blackoutCurve.Evaluate(((_mapEdge - (dist + (10 * _gridInfoSO.GridSpacing)))/_mapEdge)));
        if(((_mapEdge - (dist + (10 * _gridInfoSO.GridSpacing)))/_mapEdge) <= (_deathPercentageThreshold/100))
        {
            _player.TakeDamage(_deathPercentageThresholdDamageAmount);
        }
    }
}
