using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentEquipmentUI : MonoBehaviour
{
    public static CurrentEquipmentUI singlton;
    private void Awake() => singlton = this;

    [SerializeField]SpriteRenderer[] _buttonSpritesArray;
    [SerializeField]TMP_Text[] _buttonTextArray;
    [SerializeField]GameObject[] _buttonCoverArray;
    [SerializeField]float _startingCoverScale;

    public void UpdateUpgradeUI(int slot, Sprite icon, string text)
    {
        _buttonSpritesArray[slot].sprite = icon;
        _buttonTextArray[slot].text = text;
    }

    public void UpdateUpgradeTimers(float maxTimer, int slot, float time)
    {
        _buttonCoverArray[slot].transform.localScale = new Vector3(_startingCoverScale, Mathf.LerpUnclamped( 0f, _startingCoverScale/maxTimer, time), _startingCoverScale);
    }
}
