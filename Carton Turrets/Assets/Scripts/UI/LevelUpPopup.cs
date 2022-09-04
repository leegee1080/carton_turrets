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
    public ScriptableObject[] FillInUpgradeList;
    public List<IUpgradeable> PotentialUpgradeArray = new List<IUpgradeable>();
    public IUpgradeable[] AvailableUpgradeArray;

    private void Start() {
        Hide();

        Debug.Log("LevelUpPopup is gathering potential upgrades...(When the objects are selected: detect what the player has already)");
        Debug.Log("implement the commented notes in the TUrret class");
        
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

    public void UpgradeChosen(int index)
    {
        FindAndApplyUpgrade(AvailableUpgradeArray[index]);
        Hide();
    }

    void FindAndApplyUpgrade(IUpgradeable chosenUpgrade)
    {
        for (int i = 0; i < StageController.singlton.Player.CurrentUpgradesArray.Length; i++)
        {
            if(chosenUpgrade.UpgradeName == StageController.singlton.Player.CurrentUpgradesArray[i].name)
            {
                int tier = StageController.singlton.Player.CurrentUpgradesArray[i].Tier + 1;

                StageController.singlton.Player.CurrentUpgradesArray[i].Tier = tier;
                StageController.singlton.Player.CurrentUpgradesArray[i].SO.ApplyUpgrade(tier);
                return;
            }
        }
        chosenUpgrade.ApplyUpgrade(0);//otherwise start over
        return;
    }



    void CreateAvailableUpgradeArray()
    {
        PotentialUpgradeArray.Clear();
        
        if(CheckForAllFullUpgradeSlots()){//if player has no available slots, fill choices with money, health, and exp
            foreach (IUpgradeable item in FillInUpgradeList)
            {
                PotentialUpgradeArray.Add(item);
            }
        }else{ //else filter the unfiltered list
            foreach (IUpgradeable item in UnfilteredUpgradeArray)
            {
                if(CheckPlayerSlotsForMaxUpgrade(item)){continue;}//if not maxed out or all slots filled add the potential upgrade array
                PotentialUpgradeArray.Add(item);
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
    bool CheckForAllFullUpgradeSlots()
    {
        foreach (UpgradeSlot item in StageController.singlton.Player.CurrentUpgradesArray)
        {
            if(item.Tier < item.MaxAllowedTier){return false;}
            if(item.name == ""){return false;}
        }
        return true;
    }
    bool CheckPlayerSlotsForMaxUpgrade(IUpgradeable item)
    {
        foreach (UpgradeSlot equippedupgrade in StageController.singlton.Player.CurrentUpgradesArray)
        {
            if(equippedupgrade.name == ""){return false;}

            if(item.UpgradeName == equippedupgrade.name)
            {
                if(equippedupgrade.Tier < equippedupgrade.MaxAllowedTier){return false;}
                return true;
            }
        }
        return true;
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
}
