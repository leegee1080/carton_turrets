using UnityEngine;
using TMPro;


public class MapChooseButton : MonoBehaviour
{
    [SerializeField]private PlayableMaps _thisMap;
    private int _cost;
    [SerializeField]private bool _unlocked;

    [Header("Visual Vars")]
    [SerializeField]SpriteRenderer _sR;
    [SerializeField]TMP_Text _txtBox;
    [SerializeField]TMP_Text _descBox;
    [SerializeField]GameObject _slctImageGO;

    private void Start()
    {
        if(_unlocked){return;}
        _cost = GlobalDataStorage.singleton.ReturnPossibleMapSOArray()[(int)_thisMap].UnlockCost;
        _sR.sprite = GlobalDataStorage.singleton.ReturnPossibleMapSOArray()[(int)_thisMap].Icon;
        _txtBox.text = ""+_cost;
    }


    public void UpdateUnlockStatus()
    {
        PlayableMaps[] unlockedMaps = GlobalDataStorage.singleton.ReturnCurrentlyUnlockedMaps();

        foreach (PlayableMaps item in unlockedMaps)
        {
            if(item == _thisMap)
            {
                Unlock(false);
                return;
            }
        }
    }
    
    public void PressButton()
    {
        if(_unlocked){SelectOption();return;}
        if(GlobalDataStorage.singleton.PlayerMoney >= _cost)
        {
            GlobalDataStorage.singleton.PlayerMoney -= _cost;
            GlobalDataStorage.singleton.UnlockMap(_thisMap);
            Unlock(true);
            return;
        }
    }

    private void Unlock(bool soundsAndEffects)
    {
        _unlocked= true;
        _sR.color = new Color(255,255,255,1);
        _txtBox.text = GlobalDataStorage.singleton.ReturnPossibleMapSOArray()[(int)_thisMap].name;
        _descBox.text = GlobalDataStorage.singleton.ReturnPossibleMapSOArray()[(int)_thisMap].Desc;

        if(soundsAndEffects)
        {
            //sounds and effects
            AudioController.singleton.PlaySound("ui_menu_unlock");
        }
    }

    public void CheckForSelected()
    {
        // if(GlobalDataStorage.singleton.ChosenMap == _thisMap)
        // {
        //     _slctImageGO.SetActive(true);
        //     return;
        // }
        // _slctImageGO.SetActive(false);
    }

    private void SelectOption()
    {
        if(!_unlocked){return;}
        // if(GlobalDataStorage.singleton.ChosenMap == _thisMap){return;}

        GlobalDataStorage.singleton.ChosenMap = _thisMap;
        MainMenuController.singleton.UpdateMoneyUI();

        MainMenuController.singleton.UpdateMapSelectButtons();

        AudioController.singleton.PlaySound("ui_pick_map");

        MainMenuController.singleton.MapSelected();

    }
}
