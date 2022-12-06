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
    [SerializeField]private bool _ignoreUseAltExploStatsBool = false;

    private void GrabInfoFromTurret(Turret t)
    {
        _lifeTime = t.ELifeTime;
        _damage = t.EDamage;
        _size = t.ESize;
        _speed = t.ESpeed;

        if(t.TurretData.UseAltExploStats && !_ignoreUseAltExploStatsBool)
        {
            _lifeTime = t.TurretData.ALTELifeTime;
            _damage = t.TurretData.ALTEDamage;
            _size = t.TurretData.ALTESize;
            _speed = t.TurretData.ALTESpeed;
        }
    }

    public void Fire(IPassableObject Turret)
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
