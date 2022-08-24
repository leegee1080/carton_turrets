using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopup : MonoBehaviour
{

    public static LevelUpPopup singlton;

    private void Awake() => singlton = this;

    [Header("UI")]
    [SerializeField]GameObject[] _elementsToShowArray;

    [SerializeField]GameObject[] _buttonArray;
    [SerializeField]SpriteRenderer[] _iconArray;
    [SerializeField]TMP_Text[] _textArray;

    [Header("Upgrades")]
    public ScriptableObject[] PotentialUpgradeArray;
    public IUpgradeable[] AvailableUpgradeArray;

    private void Start() {
        Hide();
        
        if(PotentialUpgradeArray[0] is IUpgradeable)
        {
            Debug.Log("LevelUpPopup is gathering potential upgrades...(When the objects are selected: detect what the player has already)");
            //gather this list from a central global location
        }
        else
        {
            Debug.LogError("There is an upgrade that does not impliment IUpgradable, index is: " + 0);
            return;
        }
    }

    public void Show()
    {
        if(_elementsToShowArray.Length == 0){return;}
        foreach (GameObject item in _elementsToShowArray)
        {
            item.SetActive(true);
        }
        CreateAvailableUpgradeArray();
        Time.timeScale = 0;
    }
    public void Hide()
    {
        if(_elementsToShowArray.Length == 0){return;}
        foreach (GameObject item in _elementsToShowArray)
        {
            item.SetActive(false);
        }

        Time.timeScale = 1;
    }

    void CreateAvailableUpgradeArray()
    {
        IUpgradeable[] temp = new IUpgradeable[PotentialUpgradeArray.Length];

        for (int i = 0; i < _buttonArray.Length; i++)
        {
            if(i >= PotentialUpgradeArray.Length){_buttonArray[i].SetActive(false); continue;}
            _buttonArray[i].SetActive(true);
            temp[i] = (IUpgradeable)PotentialUpgradeArray[i];
        }
        AvailableUpgradeArray = temp;
        ApplyUpgradeArt();
    }

    void ApplyUpgradeArt()
    {
        for (int i = 0; i < _buttonArray.Length; i++)
        {
            if(!_buttonArray[i].activeSelf){continue;}
            _iconArray[i].sprite = AvailableUpgradeArray[i].Icon;
            _textArray[i].text = AvailableUpgradeArray[i].UpgradeName;
        }
    }

    public void UpgradeChosen(int index)
    {
        Debug.Log("tier is one above the tier the player already has");
        //upgrade tier is 0 just for now
        AvailableUpgradeArray[index].ApplyUpgrade(0);
        Hide();
    }

}
