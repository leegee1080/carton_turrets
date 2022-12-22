using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MapChooseButton : MonoBehaviour
{
    [SerializeField]private PlayableMaps _thisMap;
    [SerializeField]private int _cost;
    [SerializeField]private bool _unlocked;

    [Header("Visual Vars")]
    [SerializeField]SpriteRenderer _sR;
    [SerializeField]string _mapName;
    [SerializeField]TMP_Text _txtBox;
    [SerializeField]GameObject _slctImageGO;

    private void Start()
    {
        if(_unlocked){return;}
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
        _txtBox.text = _mapName;

        if(soundsAndEffects)
        {
            //sounds and effects
            print("unlock loud");
        }
    }

    public void CheckForSelected()
    {
        if(GlobalDataStorage.singleton.ChosenMap == _thisMap)
        {
            _slctImageGO.SetActive(true);
            return;
        }
        _slctImageGO.SetActive(false);
    }

    private void SelectOption()
    {
        if(!_unlocked){return;}
        if(GlobalDataStorage.singleton.ChosenMap == _thisMap){return;}

        GlobalDataStorage.singleton.ChosenMap = _thisMap;

        MainMenuController.singleton.UpdateCharacterSelectButtons();

    }
}
