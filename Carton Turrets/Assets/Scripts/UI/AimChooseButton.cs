using UnityEngine;
using TMPro;

public class AimChooseButton : MonoBehaviour
{
    [SerializeField]private PlayableAim _thisAim;
    [SerializeField]private int _cost;
    [SerializeField]private bool _unlocked;

    [Header("Visual Vars")]
    [SerializeField]SpriteRenderer _sR;
    [SerializeField]string _aimName;
    [SerializeField]TMP_Text _txtBox;
    [SerializeField]GameObject _slctImageGO;

    private void Start()
    {
        if(_unlocked){return;}
        _txtBox.text = ""+_cost;
    }


    public void UpdateUnlockStatus()
    {
        PlayableAim[] unlockedAim = GlobalDataStorage.singleton.ReturnCurrentlyUnlockedAim();

        foreach (PlayableAim item in unlockedAim)
        {
            if(item == _thisAim)
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
            GlobalDataStorage.singleton.UnlockAim(_thisAim);
            Unlock(true);
            return;
        }
    }

    private void Unlock(bool soundsAndEffects)
    {
        _unlocked= true;
        _sR.color = new Color(255,255,255,1);
        _txtBox.text = _aimName;

        if(soundsAndEffects)
        {
            //sounds and effects
            AudioController.singleton.PlaySound("ui_menu_unlock");
        }
    }

    public void CheckForSelected()
    {
        if(GlobalDataStorage.singleton.ChosenAim == _thisAim)
        {
            _slctImageGO.SetActive(true);
            return;
        }
        _slctImageGO.SetActive(false);
    }

    private void SelectOption()
    {
        if(!_unlocked){return;}
        if(GlobalDataStorage.singleton.ChosenAim == _thisAim){return;}

        GlobalDataStorage.singleton.ChosenAim = _thisAim;

        MainMenuController.singleton.UpdateAimSelectButtons();

        AudioController.singleton.PlaySound("ui_pick_aim");
    }
}
