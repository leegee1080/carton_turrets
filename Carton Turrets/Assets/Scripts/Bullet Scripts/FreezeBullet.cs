using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : PoolableBulletObject
{
    bool _beamSet;
    [SerializeField]private BoxCollider _boxCollider;
    [SerializeField]private GameObject _effects;

    public override void BulletCollide(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponentInParent<EnemyActor>().Freeze(_damage);
    }

    override public void BulletMovement()
    {
        if(!_fired){return;}
        if(_beamSet == false)
        {
            _beamSet = true;
            gameObject.transform.position = _parentTurret._barrel.transform.position + (transform.forward * _speed * 100);
            _boxCollider.center = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y, -gameObject.transform.localPosition.z);
            _boxCollider.size =new Vector3(0.2f, 0.2f, 0.3f + (gameObject.transform.localPosition.z * 2));

        }
        this.gameObject.transform.rotation = _parentTurret._barrel.transform.rotation;
        _lifeTime -= Time.fixedDeltaTime;
        if(_lifeTime <=0){_fired = false;_beamSet = false; _effects.SetActive(false); this.gameObject.SetActive(false);}
    }

}