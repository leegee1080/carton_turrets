using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableBulletObject : MonoBehaviour
{

    protected float _lifeTime;
    protected float _damage;
    protected float _speed;

    protected bool _fired = false;

    protected Turret _parentTurret;

    private void GrabInfoFromTurret(Turret t)
    {
        _lifeTime = t.BLifeTime;
        _damage = t.BDamage;
        _speed = t.BSpeed;
        _parentTurret = t;
    }

    virtual public void Fire(IPassableObject Turret)
    {
        GrabInfoFromTurret((Turret)Turret);
        gameObject.transform.localPosition = Vector3.zero;
        _fired = true;
    }

    private void FixedUpdate()
    {
        BulletMovement();
    }

    virtual public void BulletMovement()
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
            BulletCollide(other.gameObject);
        }
    }

    virtual public void BulletCollide(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponentInParent<EnemyActor>().TakeDamage(_damage);
    }
}
