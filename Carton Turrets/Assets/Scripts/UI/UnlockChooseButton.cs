using UnityEngine;
using TMPro;


public class UnlockChooseButton : MonoBehaviour
{
    [SerializeField]private PlayerCharacters _thisCharacter;
    [SerializeField]private int _cost;
    [SerializeField]private bool _unlocked;

    [Header("Visual Vars")]
    [SerializeField]SpriteRenderer _sR;
    [SerializeField]string _characterName;
    [SerializeField]TMP_Text _txtBox;
    [SerializeField]GameObject _slctImageGO;

    [Header("Sound Vars")]
    [SerializeField]string _pickSound;

    private void Start()
    {
        if(_unlocked){return;}
        _txtBox.text = ""+_cost;
    }


    public void UpdateUnlockStatus()
    {
        PlayerCharacters[] unlockedCharacters = GlobalDataStorage.singleton.ReturnCurrentlyUnlockedCharacters();

        foreach (PlayerCharacters item in unlockedCharacters)
        {
            if(item == _thisCharacter)
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
            GlobalDataStorage.singleton.UnlockCharacter(_thisCharacter);
            Unlock(true);
            return;
        }
    }

    private void Unlock(bool soundsAndEffects)
    {
        _unlocked= true;
        _sR.color = new Color(255,255,255,1);
        _txtBox.text = _characterName;

        if(soundsAndEffects)
        {
            //sounds and effects
            AudioController.singleton.PlaySound("ui_menu_unlock");
        }
    }

    public void CheckForSelected()
    {
        if(GlobalDataStorage.singleton.ChosenCharacter == _thisCharacter)
        {
            _slctImageGO.SetActive(true);
            return;
        }
        _slctImageGO.SetActive(false);
    }

    private void SelectOption()
    {
        if(!_unlocked){return;}
        if(GlobalDataStorage.singleton.ChosenCharacter == _thisCharacter){return;}

        GlobalDataStorage.singleton.ChosenCharacter = _thisCharacter;

        MainMenuController.singleton.UpdateCharacterSelectButtons();

        AudioController.singleton.PlaySound(_pickSound);

    }
}
