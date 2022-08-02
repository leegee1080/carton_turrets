using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableBulletObject : MonoBehaviour
{

    private float _lifeTime;
    private float _damage;
    private float _speed;

    private bool _fired = false;

    private void GrabInfoFromTurret(Turret t)
    {
        _lifeTime = t.BLifeTime;
        _damage = t.BDamage;
        _speed = t.BSpeed;
    }

    public void Fire(MonoBehaviour Turret)
    {
        GrabInfoFromTurret((Turret)Turret);
        gameObject.transform.localPosition = Vector3.zero;
        _fired = true;
    }

    private void FixedUpdate()
    {
        if(!_fired){return;}
        if(_lifeTime <=0){_fired = false; this.gameObject.SetActive(false);}

        gameObject.transform.position += transform.forward * _speed;
        _lifeTime -= Time.fixedDeltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInParent<EnemyActor>().TakeDamage(_damage);
        }
    }
}
