using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBullet : PoolableBulletObject
{
    override public void BulletMovement()
    {
        if(!_fired){return;}

        this.gameObject.transform.rotation = _parentTurret._barrel.transform.rotation;

        _lifeTime -= Time.fixedDeltaTime;
        
        if(_lifeTime <=0){_fired = false; this.gameObject.SetActive(false);}
    }
}
