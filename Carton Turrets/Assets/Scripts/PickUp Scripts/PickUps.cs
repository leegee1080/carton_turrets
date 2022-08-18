using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUps : MonoBehaviour
{
    public string PickUpId;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerActor>().PickupItem(this);
            this.gameObject.SetActive(false);
        }
    }

    public virtual void Activate(IPassableObject obj)
    {

    }
}
