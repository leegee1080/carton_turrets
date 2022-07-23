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

    public void Fire(MonoBehaviour Turret)
    {
        Turret tu = (Turret)Turret;

        Fired = true;
    }

    private void FixedUpdate()
    {
        if(!Fired){return;}
        if(LifeTime <=0){Fired = false; this.gameObject.SetActive(false);}

        _rb.velocity = new Vector3(0, 0, Speed);
        LifeTime -= Time.fixedDeltaTime;
    }
}
