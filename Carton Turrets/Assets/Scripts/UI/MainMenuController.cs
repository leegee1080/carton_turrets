using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController singleton;
    private void Awake() => singleton = this;

    [SerializeField]float _timeBufferForSceneLoad;





    private void Start()
    {
        IEnumerator BufferTimer()
        {
            yield return new WaitForSecondsRealtime(_timeBufferForSceneLoad);
            GlobalVolumeController.singleton.ShowScene();
        }
        StartCoroutine(BufferTimer());

        UpdateCharacterUnlockButtons();
        UpdateCharacterSelectButtons();
        UpdateMapUnlockButtons();
        UpdateMapSelectButtons();
        UpdateAimUnlockButtons();
        UpdateAimSelectButtons();

        // UpdateGameOptionsToggles();

        // UpdateVolumeSliders();

    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void NewGame()
    {
        GlobalVolumeController.singleton.NewScene(1);
    }

// #region SoundOptions
//     [SerializeField] Slider _effectsSlider;
//     [SerializeField] Slider _musicSlider;
//     [SerializeField] TMP_Text _musicValue;
//     [SerializeField] TMP_Text _effectsValue;



//     public void UpdateVolumeSliders()
//     {
//         // _effectsSlider.value = Whatever the sound singlton is;
//         // _musicSlider.value = Whatever the sound singlton is;
//         // _effectsValue.text = ""+Whatever the sound singlton is;
//         // _musicValue.text = ""+Whatever the sound singlton is;
//     }
//     public void ChangeEffectVolume(float change)
//     {
//         print("New Effect Volume: " + change);
//         _effectsValue.text = ""+change;
//         // whatever the sound singlton is = change;
//     }
//     public void ChangeMusicVolume(float change)
//     {
//         print("New Music Volume: " + change);
//         _musicValue.text = ""+change;
//         // whatever the sound singlton is = change;
//     }

// #endregion

// #region GameOptions
//     [SerializeField] GameObject _bloodToggleIndicatorGO;
//     [SerializeField] GameObject _damnumbToggleIndicatorGO;
//     [SerializeField] GameObject _onscContToggleIndicatorGO;

//     public void UpdateGameOptionsToggles()
//     {
//         _bloodToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.BloodOn);
//         _damnumbToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.DamageNumbersOn);
//         _onscContToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.OnScreenControlsOn);
//     }

//     public void ToggleBlood()
//     {
//         GlobalDataStorage.singleton.BloodOn = !GlobalDataStorage.singleton.BloodOn;
//         UpdateGameOptionsToggles();
//     }
//     public void ToggleDamageNumbers()
//     {
//         GlobalDataStorage.singleton.DamageNumbersOn = !GlobalDataStorage.singleton.DamageNumbersOn;
//         UpdateGameOptionsToggles();
//     }
//     public void ToggleOnScreenControls()
//     {
//         GlobalDataStorage.singleton.OnScreenControlsOn = !GlobalDataStorage.singleton.OnScreenControlsOn;
//         UpdateGameOptionsToggles();
//     }
// #endregion

#region UnlockUpdates
    [Header("Character Unlocks")]
    [SerializeField]UnlockChooseButton[] _characterButtons;
    [SerializeField]SpriteRenderer _chosenCharacterSR;
    [SerializeField]TMP_Text _chosenCharacterText;

    [Header("Map Unlocks")]
    [SerializeField]MapChooseButton[] _mapButtons;
    [SerializeField]SpriteRenderer _chosenMapSR;
    [SerializeField]TMP_Text _chosenMapText;

    [Header("Map Unlocks")]
    [SerializeField]AimChooseButton[] _aimButtons;
    [SerializeField]SpriteRenderer _chosenAimSR;
    [SerializeField]TMP_Text _chosenAimText;
    public void UpdateCharacterUnlockButtons()
    {
        foreach (UnlockChooseButton item in _characterButtons)
        {
            item.UpdateUnlockStatus();
        }
    }
    public void UpdateCharacterSelectButtons()
    {
        foreach (UnlockChooseButton item in _characterButtons)
        {
            item.CheckForSelected();
        }

        PlayerScriptableObject[] possiblePlayerSOArray = GlobalDataStorage.singleton.ReturnPossiblePlayerSOArray();

        _chosenCharacterSR.sprite = possiblePlayerSOArray[(int)GlobalDataStorage.singleton.ChosenCharacter].InGameSprite;
        _chosenCharacterText.text = possiblePlayerSOArray[(int)GlobalDataStorage.singleton.ChosenCharacter].name;

    }
    public void UpdateMapUnlockButtons()
    {
        foreach (MapChooseButton item in _mapButtons)
        {
            item.UpdateUnlockStatus();
        }
    }
    public void UpdateMapSelectButtons()
    {
        foreach (MapChooseButton item in _mapButtons)
        {
            item.CheckForSelected();
        }

        StagePackageScriptableObject[] possibleMapSOArray = GlobalDataStorage.singleton.ReturnPossibleMapSOArray();

        _chosenMapSR.sprite = possibleMapSOArray[(int)GlobalDataStorage.singleton.ChosenMap].Icon;
        _chosenMapText.text = possibleMapSOArray[(int)GlobalDataStorage.singleton.ChosenMap].name;

    }
    public void UpdateAimUnlockButtons()
    {
        foreach (AimChooseButton item in _aimButtons)
        {
            item.UpdateUnlockStatus();
        }
    }
    public void UpdateAimSelectButtons()
    {
        foreach (AimChooseButton item in _aimButtons)
        {
            item.CheckForSelected();
        }

        AimScriptableObject[] possibleAimSOArray = GlobalDataStorage.singleton.ReturnPossibleAimSOArray();

        _chosenAimSR.sprite = possibleAimSOArray[(int)GlobalDataStorage.singleton.ChosenAim].Icon;
        _chosenAimText.text = possibleAimSOArray[(int)GlobalDataStorage.singleton.ChosenAim].Name;

    }
#endregion
}
