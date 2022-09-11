using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentExpIndicatorUI : MonoBehaviour
{
    public static CurrentExpIndicatorUI singlton;
    private void Awake() => singlton = this;


    [SerializeField]TMP_Text _currentLevelText, _currentExpText;
    [SerializeField]GameObject _sliderFillControllerGameObject;
    int prevLevelThreshold = 0;

    public void UpdateExpAmountUI(int currentExp, int nextLevelThreshold)
    {
        _currentExpText.text =  currentExp + "/" + nextLevelThreshold;
        _sliderFillControllerGameObject.transform.localScale = new Vector3(Mathf.Lerp(0, 1, (float)(currentExp -prevLevelThreshold) / (float)(nextLevelThreshold - prevLevelThreshold)),1 ,1);
    }
    public void UpdateLevelCountUI(int currentLevel, int nextLevelThreshold)
    {
        _currentLevelText.text = currentLevel + "";
        _currentExpText.text =  0 + "/" + nextLevelThreshold;
        _sliderFillControllerGameObject.transform.localScale = new Vector3(0,1,1);
    }

    public void SetPrevLevelThreshold(int threshold)
    {
        prevLevelThreshold = threshold;
    }
}
