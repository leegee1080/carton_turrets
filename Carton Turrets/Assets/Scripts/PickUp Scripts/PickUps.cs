using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupTypes
{
    exp,
    money,
    health
}
public class PickUps : MonoBehaviour
{
    public PickupTypes PickUpId;
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
