using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentUpgradesUI : MonoBehaviour
{
    public static CurrentUpgradesUI singlton;

    [SerializeField]GameObject _buttonsFrame;
    [SerializeField]SpriteRenderer[] _buttonSpritesArray;
    [SerializeField]TMP_Text[] _buttonTextArray;
    [SerializeField]TMP_Text[] _buttonLevelArray;
    [SerializeField]GameObject[] _availableSlotIndicatorArray;

    private void Awake()
    {
        singlton = this;
        _buttonsFrame.SetActive(true);
        foreach (SpriteRenderer item in _buttonSpritesArray)
        {
            item.sprite = null;
        }
        HideNextAvailableSlots();
        foreach (TMP_Text item in _buttonTextArray)
        {
            item.text = "";
        }
        foreach (TMP_Text item in _buttonLevelArray)
        {
            item.text = "";
        }
    }

    public void Hide()
    {
        _buttonsFrame.SetActive(false);
    }
    public void Show()
    {
        _buttonsFrame.SetActive(true);
    }

    public void ShowNextAvailableSlot(int slot)
    {
        _availableSlotIndicatorArray[slot].SetActive(true);
    }
    public void HideNextAvailableSlots()
    {
        foreach (GameObject item in _availableSlotIndicatorArray)
        {
            item.SetActive(false);
        }
    }

    public void UpdateUpgradeUI(int slot, Sprite icon, string name, string level)
    {
        _buttonSpritesArray[slot].sprite = icon;
        _buttonTextArray[slot].text = name;
        _buttonLevelArray[slot].text = level;
    }
}
