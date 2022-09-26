using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UpgradeConfirmContainer : MonoBehaviour
{

    public static UpgradeConfirmContainer singlton;
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
        _levelUpPopupUIGameObject.SetActive(false);
        foreach (GameObject item in _visualGameObjects)
        {
            item.SetActive(true);   
        }
        _chosenUpgrade = passedChosenUpgrade;
        _nextTier = tier;
        _chosenUpgradeSR.sprite = _chosenUpgrade.Icon;
        _chosenUpgradeNameText.text = _chosenUpgrade.UpgradeName;
        _chosenUpgradeDescText.text = _chosenUpgrade.UpgradeDesc;
        SetupStatCompare();
        _isShown = true;
    }

    public void SetupStatCompare()
    {
        if(_chosenUpgrade.Tiers[0].EquipFunc == PlayerStatEnum.none){_statChangeGroup.SetActive(false); return;}
        _statChangeGroup.SetActive(true);
        
        Dictionary<PlayerStatEnum, float> tempDict = new Dictionary<PlayerStatEnum, float>();
        Action<float, Dictionary<PlayerStatEnum, float>, IUpgradeable> chosenUpgradeFunc;
        if(_nextTier == 0)
        {
            
            tempDict[_chosenUpgrade.Tiers[0].EquipFunc] = StageController.singlton.Player.PlayerCurrentStatDict[_chosenUpgrade.Tiers[0].EquipFunc];
        

            _oldStatText.text = tempDict[_chosenUpgrade.Tiers[0].EquipFunc].ToString();

            chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[_chosenUpgrade.Tiers[0].EquipFunc];
            chosenUpgradeFunc(_chosenUpgrade.Tiers[0].amt, tempDict, _chosenUpgrade);

            _newStatText.text = tempDict[_chosenUpgrade.Tiers[0].EquipFunc].ToString();
            return;
        }

        tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc] = StageController.singlton.Player.PlayerCurrentStatDict[_chosenUpgrade.Tiers[0].EquipFunc];

        _oldStatText.text = tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc].ToString();

        chosenUpgradeFunc = PublicUpgradeClasses.PlayerUpgradeEquipFuncDict[_chosenUpgrade.Tiers[0].EquipFunc];
        chosenUpgradeFunc(_chosenUpgrade.Tiers[_nextTier].amt,tempDict, _chosenUpgrade);

        _newStatText.text = tempDict[_chosenUpgrade.Tiers[_nextTier].EquipFunc].ToString();
        return;

    }
    public void Hide()
    {
        _levelUpPopupUIGameObject.SetActive(true);
        foreach (GameObject item in _visualGameObjects)
        {
            item.SetActive(false);   
        }
        _isShown = false;  
    }
    public void ConfirmUpgrade()
    {
        Hide();
        LevelUpPopup.singlton.FindAndApplyUpgrade(_chosenUpgrade);
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
                case 3:
                    ConfirmUpgrade();
                    return;
                default:
                    return;
            }
        }
    }
}

