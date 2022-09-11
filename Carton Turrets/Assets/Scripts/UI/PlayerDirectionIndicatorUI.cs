using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionIndicatorUI : MonoBehaviour
{
    public static PlayerDirectionIndicatorUI singlton;
    private void Awake() => singlton = this;

    [SerializeField]private GameObject DirectionIndicatorGameObject;
    [SerializeField]private float IndicatorOffset;

    private void Start()
    {
        DirectionIndicatorGameObject.SetActive(false);
    }

    public void UpdateDirectionIndicator(Vector2 dir)
    {
        if(dir[0] == 0 && dir[1] == 0){DirectionIndicatorGameObject.SetActive(false); return;}
        DirectionIndicatorGameObject.SetActive(true);
        DirectionIndicatorGameObject.transform.localPosition = dir*IndicatorOffset;
    }
}
