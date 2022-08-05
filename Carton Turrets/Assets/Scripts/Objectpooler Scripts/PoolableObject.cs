using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
 public class ActivatePooledObject : UnityEvent<IPassableObject>{}

[Serializable]
public class PoolableObject : MonoBehaviour
{
    public ActivatePooledObject ActivateObjectFunc;
}
