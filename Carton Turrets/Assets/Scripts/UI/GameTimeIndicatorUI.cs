using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimeIndicatorUI : MonoBehaviour
{
    public static GameTimeIndicatorUI singlton;
    private void Awake() => singlton = this;

    [SerializeField]TMP_Text _timerText;

    public void UpdateTime(float currentTime)
    {
        float round = Mathf.Round(currentTime);
        _timerText.text = "" +round;
    }
}
