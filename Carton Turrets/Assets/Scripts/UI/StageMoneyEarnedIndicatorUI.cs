using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageMoneyEarnedIndicatorUI : MonoBehaviour
{

    public static StageMoneyEarnedIndicatorUI singlton;
    private void Awake() => singlton = this;


    [SerializeField] float _UIPulseUpdateTime;
    [SerializeField] GameObject _artContainer;
    [SerializeField] TMP_Text _moneyAmountText;
    IEnumerator _currentPulserCoroutine;
    [SerializeField]int _moneyToAdd = 0;
    int _textValue = 0;

    public int PublicMoneyAmountEarnedInLevel;


    public void UpdateMoneyAmountUI(int amt)
    {
        if(_currentPulserCoroutine != null){StopCoroutine(_currentPulserCoroutine);}
        _moneyToAdd += amt;
        _currentPulserCoroutine = MoneyTextPulser();
        StartCoroutine(_currentPulserCoroutine);
    }

    IEnumerator MoneyTextPulser()
    {
        while(_moneyToAdd > 0)
        {
            AudioController.singleton.PlaySound("ui_coin_collect");
            _moneyToAdd -= 1;
            _textValue += 1;
            _moneyAmountText.text = _textValue.ToString();
            PublicMoneyAmountEarnedInLevel = _textValue;
            yield return new WaitForSecondsRealtime(_UIPulseUpdateTime);
        }
    }

    public void GiveGlobalMoneyToTrack()
    {
        _textValue = GlobalDataStorage.singleton.PlayerMoney;
    }

    public void UpdateInterface()
    {
        _moneyAmountText.text = _textValue.ToString();
    }

    [ContextMenu("Give Ten Money")]
    public void GiveTenMoney()
    {
        UpdateMoneyAmountUI(10);
    }
}
