using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthIndicatorUI : MonoBehaviour
{
    public static PlayerHealthIndicatorUI singlton;
    private void Awake() => singlton = this;

    [SerializeField]GameObject _green, _red;

    private void Start()
    {
        _red.SetActive(false);
        _green.SetActive(false);
    }

    public void HideUI()
    {
        _red.SetActive(false);
        _green.SetActive(false);
    }

    public void UpdateUI(float currentHealth, float maxHealth)
    {
        if(currentHealth >= maxHealth ){HideUI(); return;}

        _red.SetActive(true);
        _green.SetActive(true);
        _green.transform.localScale = new Vector3(Mathf.LerpUnclamped(0, 100f/maxHealth, currentHealth), _green.transform.localScale.y, _green.transform.localScale.z);
    }

}
