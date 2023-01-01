using System.Collections;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController singleton;
    private void Awake() => singleton = this;

    [SerializeField]float _timeBufferForSceneLoad;
    [SerializeField]HighlighterPackage _mainHighligher;
    [SerializeField]HighlighterPackage _quitHighligher;
    [SerializeField]HighlighterPackage _newgameHighligher;
    [SerializeField]HighlighterPackage _unlockHighligher;


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

        StageMoneyEarnedIndicatorUI.singlton.GiveGlobalMoneyToTrack();

        AudioController.singleton.FadeSoundIn(0.05f, "music_mainmenu");

        // UpdateGameOptionsToggles();

        // UpdateVolumeSliders();

        ControlsController.singleton.CurrentHighligherPackage = _mainHighligher;

    }
    
    public void ReturnToMenu()
    {
        ControlsController.singleton.CurrentHighligherPackage = _mainHighligher;
    }

    public void ShowQuitChoice()
    {
        ControlsController.singleton.CurrentHighligherPackage = _quitHighligher;
    }
    public void ShowNewGameChoice()
    {
        ControlsController.singleton.CurrentHighligherPackage = _newgameHighligher;
    }
    public void ShowUnlock()
    {
        ControlsController.singleton.CurrentHighligherPackage = _unlockHighligher;
    }

    public void QuitGame()
    {
        AudioController.singleton.FadeSoundOut(0.05f, "music_mainmenu");
        GlobalVolumeController.singleton.QuitGame();
    }

    public void Credits()
    {
        AudioController.singleton.FadeSoundOut(0.05f, "music_mainmenu");
        GlobalVolumeController.singleton.NewScene(4);
    }
    public void NewGame()
    {
        AudioController.singleton.FadeSoundOut(0.05f, "music_mainmenu");
        GlobalVolumeController.singleton.NewScene(2);
    }

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
