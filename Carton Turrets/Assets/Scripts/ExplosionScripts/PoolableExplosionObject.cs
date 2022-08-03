using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableExplosionObject : MonoBehaviour
{
    [SerializeField]ParticleSystem _ps;
    [SerializeField]SphereCollider _pc;

    private float _lifeTime;
    private float _damage;
    private float _size;
    private float _speed;

    private bool _fired = false;

    private void GrabInfoFromTurret(Turret t)
    {
        _lifeTime = t.ELifeTime;
        _damage = t.EDamage;
        _size = t.ESize;
        _speed = t.ESpeed;
    }

    public void Fire(MonoBehaviour Turret)
    {
        GrabInfoFromTurret((Turret)Turret);
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = new Vector3(_size, _size, _size);
        _fired = true;
        _ps.Play();
    }

    private void FixedUpdate()
    {
        if(!_fired){return;}
        if(_lifeTime <=0){_fired = false; this.gameObject.SetActive(false);}

        _lifeTime -= Time.fixedDeltaTime;
    }
    private void OnCollisionEnter(Collision other)
    {

        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponentInParent<EnemyActor>().TakeDamage(_damage);
        }
    }
}
