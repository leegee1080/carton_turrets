using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Events;

public class UpgradeConfirmContainer : MonoBehaviour
{

    [System.Serializable]
    public class ShowConfirm : UnityEvent{}

    [System.Serializable]
    public class HideConfirm : UnityEvent{}

    public static UpgradeConfirmContainer singlton;
    [SerializeField]public ShowConfirm ShowEvent;
    [SerializeField]public HideConfirm HideEvent;
    private void Awake() => singlton = this;
    bool _isShown;


    [SerializeField]GameObject _levelUpPopupUIGameObject;
    [SerializeField]GameObject[] _visualGameObjects;
    IUpgradeable _chosenUpgrade;
    int _nextTier;
    [SerializeField] SpriteRenderer _chosenUpgradeSR;
    [SerializeField] TMP_Text _chosenUpgradeNameText;
    [SerializeField] TMP_Text _chosenUpgradeDescText;

    [SerializeField] GameObject _statChangeGroup;
    [SerializeField] TMP_Text _oldStatText;
    [SerializeField] TMP_Text _newStatText;

    private void Start()
    {
        Hide();
    }

    public void Show(IUpgradeable passedChosenUpgrade, int tier)
    {
        ShowEvent.Invoke();

        _levelUpPopupUIGameObject.SetActive(false);
        foreach (GameObject item in _visualGameObjects)
        {
            item.SetActive(true);   
        }
        _chosenUpgrade = passedChosenUpgrade;
        _nextTier = tier;
        _chosenUpgradeSR.sprite = _chosenUpgrade.Icon;
        _chosenUpgradeNameText.text = _chosenUpgrade.UpgradeName;
        _chosenUpgradeDescText.text = _chosenUpgrade.Tiers[_nextTier].InGameDesc;
        SetupStatCompare();

        ShowNextAvailSlot(passedChosenUpgrade, tier);

        _isShown = true;
    }

    void ShowNextAvailSlot(IUpgradeable upgrade,int tier)
    {
        int UpgradeableSlot = StageController.singlton.Player.ReturnPlayerFirstUpgradableSlot(upgrade.UpgradeType);
        if(UpgradeableSlot < 0){return;}

        if(upgrade.IsUnlimited){return;}
        if(tier >0){return;}
        switch (upgrade.UpgradeType)
        {
            case UpgradeType.PlayerUpgrade:
                CurrentUpgradesUI.singlton.ShowNextAvailableSlot(UpgradeableSlot);
                return;
            case UpgradeType.Equipment:
                CurrentEquipmentUI.singlton.ShowNextAvailableSlot(UpgradeableSlot);
                return;
            default:
                Debug.LogWarning("upgradeType is incorrect: " + upgrade.UpgradeType);
                return;
        }
    }

    public void SetupStatCompare()
    {
        if(_chosenUpgrade.Tiers[0].EquipFunc == PlayerStatEnum.none){_statChangeGroup.SetActive(false); return;}
        _statChangeGroup.SetActive(true);
        
        Dictionary<PlayerStatEnum, float> tempDict = new Dictionary<PlayerStatEnum, float>();
        Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable, bool> chosenUpgradeFunc;
        if(_nextTier == 0)
        {
            tempDict[_chosenUpgrade.Tiers[0].EquipFunc] = StageController.singlton.Player.PlayerCurrentStatDict[_chosenUpgrade.Tiers[0].EquipFunc];

            _oldStatText.text = tempDict[_chosenUpgrade.Tiers[0].EquipFunc].ToString();

            chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[_chosenUpgrade.Tiers[0].EquipFunc];
            chosenUpgradeFunc(_chosenUpgrade.Tiers[0].amt, tempDict, _chosenUpgrade, true);

            _newStatText.text = tempDict[_chosenUpgrade.Tiers[0].EquipFunc].ToString();
            return;
        }

        tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc] = StageController.singlton.Player.PlayerCurrentStatDict[_chosenUpgrade.Tiers[0].EquipFunc];

        _oldStatText.text = tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc].ToString();

        chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[_chosenUpgrade.Tiers[0].EquipFunc];
        chosenUpgradeFunc(_chosenUpgrade.Tiers[_nextTier].amt,tempDict, _chosenUpgrade, true);

        _newStatText.text = tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc].ToString();
        return;

    }
    public void Hide()
    {
        HideEvent.Invoke();

        _levelUpPopupUIGameObject.SetActive(true);
        foreach (GameObject item in _visualGameObjects)
        {
            item.SetActive(false);   
        }
        CurrentEquipmentUI.singlton.HideNextAvailableSlots();
        CurrentUpgradesUI.singlton.HideNextAvailableSlots();
        _isShown = false;  
    }
    public void ConfirmUpgrade()
    {
        Hide();
        LevelUpPopup.singlton.FindAndApplyUpgrade(_chosenUpgrade, StageController.singlton.Player.ReturnArrayToSearchBasedOnUpgradeType(_chosenUpgrade.UpgradeType));
        LevelUpPopup.singlton.Hide();
    }
    public void DenyUpgrade()
    {
        Hide();
    }

    private void Update()
    {
        if(_isShown)
        {
            int slot = StageController.singlton.FindActivateControlsIndex();
            if(slot == -1){return;}
            switch (slot)
            {
                case 1:
                    DenyUpgrade();
                    return;
                case 2:
                    ConfirmUpgrade();
                    return;
                default:
                    return;
            }
        }
    }
}

