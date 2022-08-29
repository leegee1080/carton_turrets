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
    public ScriptableObject[] UnfilteredUpgradeArray;
    public List<IUpgradeable> PotentialUpgradeArray = new List<IUpgradeable>();
    public IUpgradeable[] AvailableUpgradeArray;

    private void Start() {
        Hide();

        Debug.Log("LevelUpPopup is gathering potential upgrades...(When the objects are selected: detect what the player has already)");
        Debug.Log("If the player already has the upgrade change the tier number in the Upgrade slot struct the player has to reflect the correct tier");
        Debug.Log("impletment the commented notes in the TUrret class");
        
        for (int i = 0; i < UnfilteredUpgradeArray.Length; i++)
        {
            if(!(UnfilteredUpgradeArray[i] is IUpgradeable)){Debug.LogError("There is an upgrade that does not impliment IUpgradable, index is: " + i);}
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
        PotentialUpgradeArray.Clear();

        //if player has no available slots, fill choices with money, health, and exp


        foreach (IUpgradeable item in UnfilteredUpgradeArray)//grab upgrades that have more tiers to go
        {
            foreach (UpgradeSlot equippedupgrade in StageController.singlton.Player.CurrentUpgradesArray)
            {
                if(item.UpgradeName != equippedupgrade.name)
                {
                    continue;
                }
                else
                {
                    
                }
            }
        }


        IUpgradeable[] temp = new IUpgradeable[PotentialUpgradeArray.Count];

        for (int i = 0; i < _buttonArray.Length; i++)
        {
            if(i >= PotentialUpgradeArray.Count){_buttonArray[i].SetActive(false); continue;}
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
