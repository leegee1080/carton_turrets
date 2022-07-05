using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyControllerGroup : MonoBehaviour
{
    public static DontDestroyControllerGroup singleton;
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
