using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableBulletObject : MonoBehaviour
{
    public float LifeTime;
    public float Damage;
    public float Speed;

    public bool Fired = false;

    public void Fire(MonoBehaviour Turret)
    {
        Fired = true;
        Turret tu = (Turret)Turret;
    }

    private void FixedUpdate()
    {
        if(!Fired){return;}
    }
}
