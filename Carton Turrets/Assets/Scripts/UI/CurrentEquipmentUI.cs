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

    public void UpdateUpgradeUI(int slot, Sprite icon, string text)
    {
        _buttonSpritesArray[slot].sprite = icon;
        _buttonTextArray[slot].text = text;
    }
}
