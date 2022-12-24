using UnityEngine;
using TMPro;

public class CurrentEquipmentUI : MonoBehaviour
{
    public static CurrentEquipmentUI singlton;

    [SerializeField]GameObject _buttonsFrame;
    [SerializeField]SpriteRenderer[] _buttonSpritesArray;
    [SerializeField]TMP_Text[] _buttonTextArray;
    [SerializeField]TMP_Text[] _buttonLevelArray;
    [SerializeField]GameObject[] _availableSlotIndicatorArray;
    [SerializeField]GameObject[] _buttonCoverArray;
    [SerializeField]CooldownSplash[] _coolDownSplashArray;
    [SerializeField]float _startingCoverY;

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

    public void UpdateUpgradeTimers(float maxTimer, int slot, float time)
    { 
        _buttonCoverArray[slot].transform.localPosition = new Vector3(0, Mathf.Lerp(0, _startingCoverY, time/maxTimer), 0);
        if(time <= 0)
        {
             _buttonCoverArray[slot].SetActive(false);
             FlashEquCooldown(slot);
        }
        else
        {
             _buttonCoverArray[slot].SetActive(true);
        }
    }

    private void FlashEquCooldown(int slot)
    {
        _coolDownSplashArray[slot].BlastOffEffect();
    }
}
