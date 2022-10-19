using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : PoolableBulletObject
{


    public override void BulletCollide(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponentInParent<EnemyActor>().Freeze(_damage);
    }

    override public void BulletMovement()
    {
        base.BulletMovement();
    }

}
