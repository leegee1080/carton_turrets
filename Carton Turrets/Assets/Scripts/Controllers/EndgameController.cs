using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameController : MonoBehaviour, IDroppodControllable
{
    [SerializeField]float _speed;
    [SerializeField]float _deathGrowthSpeed;
    [SerializeField]GameObject _artObject;
    [SerializeField]DropPodImpact _bombImpactObject;
    [SerializeField]GameObject _bombKillerCollider;
    [SerializeField]bool _launched;
    [SerializeField]bool _impacted;


    private void FixedUpdate()
    {
        if(!_launched || _impacted){return;}

        if(Vector3.Distance(_artObject.transform.localPosition, Vector3.zero) > 0.1f)
        {
            float step = _speed *Time.deltaTime;
            _artObject.transform.localPosition =  Vector3.MoveTowards(_artObject.transform.localPosition, Vector3.zero, step);
            return;
        }

        _bombImpactObject.Nuke(this.transform.position);
        _impacted = true;
    }

    public void LaunchBomb(Vector3 impactLocation)
    {
        this.transform.position = impactLocation;
        _artObject.SetActive(true);
        _launched = true;
    }
    public void Impact()
    {

    }
    public void CompleteLanding()
    {
        _bombKillerCollider.SetActive(true);
    }
}
