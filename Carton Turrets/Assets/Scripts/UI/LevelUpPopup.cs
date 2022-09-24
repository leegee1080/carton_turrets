using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopup : MonoBehaviour
{

    public static LevelUpPopup singlton;
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
    public ScriptableObject[] UnfilteredUpgradeArray;
    public ScriptableObject[] FillInUpgradeList;
    public List<IUpgradeable> PotentialUpgradeArray = new List<IUpgradeable>();
    public IUpgradeable[] AvailableUpgradeArray;


    private void Start() {
        Hide();

        Debug.Log("LevelUpPopup is gathering potential upgrades. (Upgrade this to pull from one global source (Load into a global constant that pulls from a folder maybe))");
        Debug.Log("implement the commented notes in the Turret class");
        Debug.Log("implement the commented notes in the EnemyActor Class");
        
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

        int UpgradeableSlot = StageController.singlton.Player.ReturnPlayerFirstUpgradableSlot();
        if(UpgradeableSlot > 0){CurrentEquipmentUI.singlton.ShowNextAvailableSlot(UpgradeableSlot);}


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
        if(_elementsToShowArray.Length == 0){return;}
        foreach (GameObject item in _elementsToShowArray)
        {
            item.SetActive(false);
        }
        Time.timeScale = 1;
        _isShown = false;
        CurrentEquipmentUI.singlton.HideNextAvailableSlots();
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
        FindAndApplyUpgrade(AvailableUpgradeArray[index]);
        Hide();
    }

    void FindAndApplyUpgrade(IUpgradeable chosenUpgrade)
    {
        StageController SCref =  StageController.singlton;

        for (int i = 0; i < SCref.Player.CurrentUpgradesArray.Length; i++)
        {
            if(chosenUpgrade.UpgradeName == SCref.Player.CurrentUpgradesArray[i].name)
            {
                int tier = SCref.Player.CurrentUpgradesArray[i].Tier + 1;

                SCref.Player.CurrentUpgradesArray[i].Tier = tier;
                SCref.Player.CurrentUpgradesArray[i].SO.ApplyUpgrade(tier);
                CurrentEquipmentUI.singlton.UpdateUpgradeUI(i,  SCref.Player.CurrentUpgradesArray[i].SO.Icon,  SCref.Player.CurrentUpgradesArray[i].name, tier.ToString());
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
            _textDescArray[i].text = AvailableUpgradeArray[i].UpgradeDesc;
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
