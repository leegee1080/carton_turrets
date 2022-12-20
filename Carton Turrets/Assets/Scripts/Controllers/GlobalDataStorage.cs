using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum PlayerCharacters
{
    eye,
    eye2,
    eye3,
    bluewiz,
    grnwiz,
    redwiz

}
public class GlobalDataStorage : MonoBehaviour
{
    public static GlobalDataStorage singleton;
    private void Awake()
    {

        if(singleton == null)
        {
            singleton = this;
            return;
        }
        
        Destroy(this.gameObject);
    }

    public int PlayerMoney = 100;

    [Header("Character Unlocks")]
    [SerializeField]PlayerScriptableObject[] _possiblePlayerSOArray;
    [SerializeField]UnlockChooseButton[] _characterButtons;
    [SerializeField]PlayerCharacters[] _currentlyUnlockedCharacters;
    public PlayerCharacters ChosenCharacter;
    [SerializeField]SpriteRenderer _chosenCharacterSR;
    [SerializeField]TMP_Text _chosenCharacterText;

    [Header("Map Unlocks")]
    [SerializeField]UnlockChooseButton[] _mapButtons;
    [SerializeField]PlayerCharacters[] _currentlyUnlockedMaps;
    public PlayerCharacters ChosenMap;


    private void Start()
    {
        UpdateCharacterUnlockButtons();
        UpdateCharacterSelectButtons();

    }

    public void UpdateCharacterUnlockButtons()
    {
        foreach (UnlockChooseButton item in _characterButtons)
        {
            item.UpdateUnlockStatus(_currentlyUnlockedCharacters);
        }
    }
    public void UpdateCharacterSelectButtons()
    {
        foreach (UnlockChooseButton item in _characterButtons)
        {
            item.CheckForSelected();
        }

        _chosenCharacterSR.sprite = _possiblePlayerSOArray[(int)ChosenCharacter].InGameSprite;
        _chosenCharacterText.text = _possiblePlayerSOArray[(int)ChosenCharacter].name;

    }

    public void UnlockCharacter(PlayerCharacters unlock)
    {
        print("Character Unlocked: "+ unlock);
        PlayerCharacters[] tempArray = new PlayerCharacters[_currentlyUnlockedCharacters.Length + 1];

        for (int i = 0; i < _currentlyUnlockedCharacters.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedCharacters[i];
        }

        tempArray[tempArray.Length -1] = unlock;


        _currentlyUnlockedCharacters = tempArray;
    }

}
