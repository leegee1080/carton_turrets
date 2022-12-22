using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController singleton;
    private void Awake() => singleton = this;

    [SerializeField]float _timeBufferForSceneLoad;

    [Header("Character Unlocks")]
    [SerializeField]UnlockChooseButton[] _characterButtons;
    [SerializeField]SpriteRenderer _chosenCharacterSR;
    [SerializeField]TMP_Text _chosenCharacterText;

    [Header("Map Unlocks")]
    [SerializeField]MapChooseButton[] _mapButtons;
    [SerializeField]SpriteRenderer _chosenMapSR;
    [SerializeField]TMP_Text _chosenMapText;



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
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void NewGame()
    {
        GlobalVolumeController.singleton.NewScene(1);
    }

#region UnlockUpdates
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
#endregion
}
