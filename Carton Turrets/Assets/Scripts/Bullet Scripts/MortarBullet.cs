using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : PoolableBulletObject
{
    [SerializeField] GameObject _explosionObject;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float _arcHeight;

    float _timeElapsed = 0;
    
    private void Awake()
    {
        _timeElapsed = 0;
    }

    override public void BulletMovement()
    {
        if(!_fired){return;}
        if(_lifeTime <=0)
        {
            _fired = false;
            Explode();
            this.gameObject.SetActive(false);
        }

        Vector3 nextPos = new Vector3
        (
            gameObject.transform.position.x + (transform.forward.x * _speed),
            curve.Evaluate(_timeElapsed) * _arcHeight,
            gameObject.transform.position.z + (transform.forward.z * _speed)
        );

        gameObject.transform.position = nextPos;
        _lifeTime -= Time.fixedDeltaTime;
        _timeElapsed += Time.fixedDeltaTime/_parentTurret.BLifeTime;
    }

    private void Explode()
    {
        _explosionObject.SetActive(true);
        _explosionObject.transform.position = gameObject.transform.position;
        _explosionObject.transform.rotation = gameObject.transform.rotation;
        _explosionObject.GetComponent<PoolableObject>().ActivateObjectFunc.Invoke(_parentTurret);
        _timeElapsed=0;
    }
}
