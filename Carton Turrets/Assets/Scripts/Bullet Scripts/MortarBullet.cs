using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBullet : PoolableBulletObject
{
    [SerializeField] GameObject _explosionObject;
    
    override public void BulletMovement()
    {
        if(!_fired){return;}
        if(_lifeTime <=0)
        {
            _fired = false;
            Explode();
            this.gameObject.SetActive(false);
        }

        gameObject.transform.position += transform.forward * _speed;
        _lifeTime -= Time.fixedDeltaTime;
    }

    private void Explode()
    {
        _explosionObject.SetActive(true);
        _explosionObject.transform.position = gameObject.transform.position;
        _explosionObject.transform.rotation = gameObject.transform.rotation;
        _explosionObject.GetComponent<PoolableObject>().ActivateObjectFunc.Invoke(_parentTurret);
    }
}
