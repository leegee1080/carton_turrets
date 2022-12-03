using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPodController : MonoBehaviour
{
    [SerializeField]float _speed;
    [SerializeField]GameObject _artObject;
    [SerializeField]DropPodImpact _dpImpactObject;
    [SerializeField]bool _launched;
    [SerializeField]bool _impacted;

    public void Launch(Vector3 impactLocation)
    {
        this.transform.position = impactLocation;
        _artObject.SetActive(true);
        _launched = true;
    }

    private void FixedUpdate()
    {
        if(!_launched || _impacted){return;}

        if(Vector3.Distance(_artObject.transform.localPosition, Vector3.zero) > 0.1f)
        {
            float step = _speed *Time.deltaTime;
            _artObject.transform.localPosition =  Vector3.MoveTowards(_artObject.transform.localPosition, Vector3.zero, step);
            return;
        }

        _dpImpactObject.Nuke(this.transform.position);
        _impacted = true;
    }

    public void Impact()
    {
        StageController.singlton.Player.Activate();
        StageController.singlton.ChangeState(new StageState_Running());
        _artObject.SetActive(false);
    }
}
