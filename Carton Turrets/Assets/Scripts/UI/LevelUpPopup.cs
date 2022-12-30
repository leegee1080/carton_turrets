using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;


[System.Serializable]
public class ShowLevelUp : UnityEvent{}

[System.Serializable]
public class HideLevelUp : UnityEvent{}

public class LevelUpPopup : MonoBehaviour
{

    public static LevelUpPopup singlton;
    [SerializeField]public ShowLevelUp ShowEvent;
    [SerializeField]public HideLevelUp HideEvent;
    private void Awake() => singlton = this;

    [Header("Control Lockout")]
    bool _isShown;
    bool _controlsLocked;
    [SerializeField]float _controlLockoutTime;
    IEnumerator _lockoutTimer;

    [Header("UI")]
    [SerializeField]GameObject[] _elementsToShowArray;
    [SerializeField]GameObject[] _buttonArray;
    [SerializeField]SpriteRenderer[] _iconArray;
    [SerializeField]TMP_Text[] _textBtnArray;
    [SerializeField]TMP_Text[] _textDescArray;


    [Header("Upgrades")]
    public ScriptableObject[] UnfilteredEquipmentArray;
    public ScriptableObject[] UnfilteredUpgradeArray;
    public ScriptableObject[] FillInUpgradeList;
    public List<IUpgradeable> PotentialUpgradeArray = new List<IUpgradeable>();
    public IUpgradeable[] AvailableUpgradeArray;


    private void Start() {
        // Hide();
        UnfilteredEquipmentArray = StageController.singlton.CurrentStage.AvailableEquipment != null ? StageController.singlton.CurrentStage.AvailableEquipment: UnfilteredEquipmentArray;
        UnfilteredUpgradeArray = StageController.singlton.CurrentStage.AvailableUpgrades != null ? StageController.singlton.CurrentStage.AvailableUpgrades: UnfilteredUpgradeArray;
        FillInUpgradeList = StageController.singlton.CurrentStage.FillInUpgradesForMaxLevel != null ? StageController.singlton.CurrentStage.FillInUpgradesForMaxLevel: FillInUpgradeList;
        
        for (int i = 0; i < UnfilteredEquipmentArray.Length; i++)
        {
            if(!(UnfilteredEquipmentArray[i] is IUpgradeable)){Debug.LogError("There is an upgrade that does not impliment IUpgradable, index is: " + i); continue;}
            IUpgradeable equ = (IUpgradeable)UnfilteredEquipmentArray[i];
            if(equ.UpgradeType != UpgradeType.Equipment){Debug.LogError("There is an upgrade that is not of type equipment in the equipment array: " + i);}

        }
        for (int i = 0; i < UnfilteredUpgradeArray.Length; i++)
        {
            if(!(UnfilteredUpgradeArray[i] is IUpgradeable)){Debug.LogError("There is an upgrade that does not impliment IUpgradable, index is: " + i); continue;}
            IUpgradeable up = (IUpgradeable)UnfilteredUpgradeArray[i];
            if(up.UpgradeType != UpgradeType.PlayerUpgrade){Debug.LogError("There is an upgrade that is not of type PlayerUpgrade in the Upgrade array: " + i);}
        }
    }

    public void Show()
    {
        ShowEvent.Invoke();

        if(_elementsToShowArray.Length == 0){return;}
        foreach (GameObject item in _elementsToShowArray)
        {
            item.SetActive(true);
        }
        CreateAvailableUpgradeArray();
        Time.timeScale = 0;


        _isShown =true;

        //this is designed to allow the player time to react to the UI popups
        _controlsLocked = true;
        IEnumerator ControlLockTimer()
        {
            yield return new WaitForSecondsRealtime(_controlLockoutTime);
            _controlsLocked = false;
        }
        if(_lockoutTimer != null){StopCoroutine(_lockoutTimer);}
        _lockoutTimer = ControlLockTimer();
        StartCoroutine(_lockoutTimer);
    }

    public void Hide()
    {
        HideEvent.Invoke();

        if(_elementsToShowArray.Length == 0){return;}
        foreach (GameObject item in _elementsToShowArray)
        {
            item.SetActive(false);
        }
        Time.timeScale = 1;
        _isShown = false;
    }
    private void Update()
    {
        if(_isShown && !_controlsLocked)
        {
            int slot = StageController.singlton.FindMoveControlsIndex();
            if(slot == -1){return;}
            UpgradeChosen(slot);
        }
    }

    public void UpgradeChosen(int index)
    {
        if(index >= AvailableUpgradeArray.Length){return;}

        UpgradeConfirmContainer.singlton.Show
        (
            AvailableUpgradeArray[index],
            FindAndReturnNextAvailableTier
            (
                AvailableUpgradeArray[index],
                StageController.singlton.Player.ReturnArrayToSearchBasedOnUpgradeType(AvailableUpgradeArray[index].UpgradeType)
            )
        );
    }

    public int FindAndReturnNextAvailableTier(IUpgradeable chosenUpgrade, UpgradeSlot[] upgradeArray)
    {
        StageController SCref =  StageController.singlton;

        for (int i = 0; i < upgradeArray.Length; i++)
        {
            if(chosenUpgrade.UpgradeName == upgradeArray[i].name)
            {
                int tier = upgradeArray[i].Tier + 1;
                return tier;
            }
        }
        return 0;
    }

    public void FindAndApplyUpgrade(IUpgradeable chosenUpgrade, UpgradeSlot[] upgradeArray)
    {
        StageController SCref =  StageController.singlton;


        for (int i = 0; i < upgradeArray.Length; i++)
        {
            if(chosenUpgrade.UpgradeName == upgradeArray[i].name)
            {
                int tier = upgradeArray[i].Tier + 1;

                upgradeArray[i].Tier = tier;
                upgradeArray[i].SO.ApplyUpgrade(tier);
                switch (chosenUpgrade.UpgradeType)
                {
                    case UpgradeType.Equipment:
                        CurrentEquipmentUI.singlton.UpdateUpgradeUI(i,  upgradeArray[i].SO.Icon,  upgradeArray[i].SO.UpgradeName,tier >= upgradeArray[i].MaxAllowedTier ? "MAX": (tier+1).ToString());
                        break;
                    case UpgradeType.PlayerUpgrade:
                        CurrentUpgradesUI.singlton.UpdateUpgradeUI(i,  upgradeArray[i].SO.Icon,  upgradeArray[i].SO.UpgradeName, tier >= upgradeArray[i].MaxAllowedTier ? "MAX": (tier+1).ToString());
                        break;
                    default:
                        Debug.LogError("Chosen Upgrade was not a valid type");
                        break;
                }
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
            foreach (IUpgradeable item in UnfilteredEquipmentArray)
            {
                if(CheckPlayerSlotsForMaxEquipment(item)){continue;}//if not maxed out or all slots filled add the potential upgrade array
                PotentialUpgradeArray.Add(item);
            }
            foreach (IUpgradeable item in UnfilteredUpgradeArray)
            {
                if(CheckPlayerSlotsForMaxUpgrade(item)){continue;}//if not maxed out or all slots filled add the potential upgrade array
                PotentialUpgradeArray.Add(item);
            }
        }

        IUpgradeable[] tempPotentialUpgrades = ArrayShuffle(PotentialUpgradeArray);

        // IUpgradeable[] temp = new IUpgradeable[PotentialUpgradeArray.Count];
        IUpgradeable[] temp = new IUpgradeable[tempPotentialUpgrades.Length];


        for (int i = 0; i < _buttonArray.Length; i++)
        {
            if(i >= tempPotentialUpgrades.Length){_buttonArray[i].SetActive(false); continue;}
            _buttonArray[i].SetActive(true);
            temp[i] = (IUpgradeable)tempPotentialUpgrades[i];
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
        foreach (UpgradeSlot item in StageController.singlton.Player.CurrentEquipmentArray)
        {
            if(item.Tier < item.MaxAllowedTier){return false;}
            if(item.name == ""){return false;}
        }
        return true;
    }
    bool CheckPlayerSlotsForMaxEquipment(IUpgradeable item)
    {
        foreach (UpgradeSlot equippedupgrade in StageController.singlton.Player.CurrentEquipmentArray)
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
            _textBtnArray[i].text = AvailableUpgradeArray[i].UpgradeName;
            _textDescArray[i].text = AvailableUpgradeArray[i].Tiers[0].InGameDesc;
        }
    }

    IUpgradeable[] ArrayShuffle(List<IUpgradeable> original)
    {

        IUpgradeable[] temp = new IUpgradeable[original.Count];

        List<IUpgradeable> aftList = new List<IUpgradeable>();

        for (int i = 0; i < temp.Length; i++)
        {
            int r = Random.Range(0, original.Count);
            
            aftList.Add(original[r]);

            original.RemoveAt(r);
        }

        for (int i = 0; i < aftList.Count; i++)
        {
            temp[i] = aftList[i];
        }

        return temp;
    }
}
