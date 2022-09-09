using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionIndicatorUI : MonoBehaviour
{
    public static PlayerDirectionIndicatorUI singlton;
    private void Awake() => singlton = this;
}
