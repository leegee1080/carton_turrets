using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameKillerController : MonoBehaviour
{
    [SerializeField]float _deathGrowthSpeed;
    [SerializeField]float _deathDamage;
    private void FixedUpdate() 
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x + _deathGrowthSpeed, this.transform.localScale.y + _deathGrowthSpeed, this.transform.localScale.z + _deathGrowthSpeed);
    }

    public void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            BulletCollideEnemy(other.gameObject);
        }
        if(other.gameObject.tag == "Player")
        {
            BulletCollidePlayer(other.gameObject);
        }
    }

    public void BulletCollideEnemy(GameObject collidedEnemy)
    {
        collidedEnemy.GetComponentInParent<EnemyActor>().TakeDamage(_deathDamage);
    }
    public void BulletCollidePlayer(GameObject collidedPlayer)
    {
        collidedPlayer.GetComponentInParent<PlayerActor>().TakeDamage(_deathDamage);
    }
}
