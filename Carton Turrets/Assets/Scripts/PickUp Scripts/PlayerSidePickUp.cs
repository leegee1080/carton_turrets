using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSidePickUp : MonoBehaviour
{
    [SerializeField]float speed;
    private void OnTriggerStay(Collider other)
    {
        other.gameObject.transform.position = Vector3.MoveTowards(other.gameObject.transform.position, this.transform.position, speed);
        
    }
}
