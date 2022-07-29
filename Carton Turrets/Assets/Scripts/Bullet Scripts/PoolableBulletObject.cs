using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableBulletObject : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;

    public float LifeTime;
    public float Damage;
    public float Speed;

    public bool Fired = false;

    private void GrabInfoFromTurret(Turret t)
    {
        LifeTime = t.BLifeTime;
        Damage = t.BDamage;
        Speed = t.BSpeed;
    }

    public void Fire(MonoBehaviour Turret)
    {
        GrabInfoFromTurret((Turret)Turret);
        _rb.gameObject.transform.localPosition = Vector3.zero;
        Fired = true;
    }

    private void FixedUpdate()
    {
        if(!Fired){return;}
        if(LifeTime <=0){Fired = false; this.gameObject.SetActive(false);}

        _rb.velocity = transform.forward * Speed;
        LifeTime -= Time.fixedDeltaTime;
    }
}
