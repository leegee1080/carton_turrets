using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IColliderMessageable
{
    public void RecMessageEnter(GameObject obj);
    public void RecMessageStay(GameObject obj);
}

public class ChildColliderMessager : MonoBehaviour
{
    public string TagToCheck;

    [SerializeField]private MonoBehaviour _parentScript;

    [SerializeField]private IColliderMessageable _classToMessageEnter;
    [SerializeField]private IColliderMessageable _classToMessageStay;

    private void Awake()
    {
        _classToMessageEnter = (IColliderMessageable)_parentScript;
        _classToMessageStay = (IColliderMessageable)_parentScript;
    }

    private void OnTriggerEnter(Collider other)
    {

        _classToMessageEnter.RecMessageEnter(other.gameObject);

    }

    private void OnTriggerStay(Collider other)
    {

        _classToMessageEnter.RecMessageStay(other.gameObject);

    }
}
