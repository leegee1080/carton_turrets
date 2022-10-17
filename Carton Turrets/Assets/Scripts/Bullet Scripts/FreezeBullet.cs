using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : PoolableBulletObject
{


    public override void BulletCollide(GameObject collidedEnemy)
    {
        
    }

    override public void BulletMovement()
    {
        base.BulletMovement();
    }

}
