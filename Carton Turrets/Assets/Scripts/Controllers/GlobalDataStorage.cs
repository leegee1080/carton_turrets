using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public enum PlayerCharacters
{
    eye,
    eye2,
    eye3,
    bluewiz,
    grnwiz,
    redwiz

}
public enum PlayableMaps
{
    rocky,
}

public enum PlayableAim
{
    playerDir,
    rightDir,
    leftDir,
    atPDir,
    atEDir
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

    [Header("Game Options")]
    public bool BloodOn;
    public bool DamageNumbersOn;
    public bool OnScreenControlsOn;

    [Header("Character Unlocks")]
    [SerializeField]PlayerScriptableObject[] _possiblePlayerSOArray;
    [SerializeField]PlayerCharacters[] _currentlyUnlockedCharacters;
    public PlayerCharacters ChosenCharacter;

    [Header("Map Unlocks")]
    [SerializeField]StagePackageScriptableObject[] _possiblePlayableMapSOArray;
    [SerializeField]PlayableMaps[] _currentlyUnlockedMaps;
    public PlayableMaps ChosenMap;

    [Header("Aim Unlocks")]
    [SerializeField]AimScriptableObject[] _possibleAimSOArray;
    [SerializeField]PlayableAim[] _currentlyUnlockedAim;
    public PlayableAim ChosenAim;


    //player so returns
    public PlayerScriptableObject[] ReturnPossiblePlayerSOArray(){return _possiblePlayerSOArray;}
    public PlayerCharacters[] ReturnCurrentlyUnlockedCharacters(){return _currentlyUnlockedCharacters;}
    public PlayerScriptableObject ReturnChosenPlayerSO(){return _possiblePlayerSOArray[(int)ChosenCharacter];}

    //map so returns
    public StagePackageScriptableObject[] ReturnPossibleMapSOArray(){return _possiblePlayableMapSOArray;}
    public PlayableMaps[] ReturnCurrentlyUnlockedMaps(){return _currentlyUnlockedMaps;}
    public StagePackageScriptableObject ReturnChosenMapSO(){return _possiblePlayableMapSOArray[(int)ChosenMap];}

    //aim so returns
    public AimScriptableObject[] ReturnPossibleAimSOArray(){return _possibleAimSOArray;}
    public PlayableAim[] ReturnCurrentlyUnlockedAim(){return _currentlyUnlockedAim;}
    public AimScriptableObject ReturnChosenAimSO(){return _possibleAimSOArray[(int)ChosenAim];}

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
    public void UnlockMap(PlayableMaps unlock)
    {
        print("Map Unlocked: "+ unlock);
        PlayableMaps[] tempArray = new PlayableMaps[_currentlyUnlockedMaps.Length + 1];

        for (int i = 0; i < _currentlyUnlockedMaps.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedMaps[i];
        }

        tempArray[tempArray.Length -1] = unlock;


        _currentlyUnlockedMaps = tempArray;
    }
    public void UnlockAim(PlayableAim unlock)
    {
        print("Aim Unlocked: "+ unlock);
        PlayableAim[] tempArray = new PlayableAim[_currentlyUnlockedAim.Length + 1];

        for (int i = 0; i < _currentlyUnlockedAim.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedAim[i];
        }

        tempArray[tempArray.Length -1] = unlock;


        _currentlyUnlockedAim = tempArray;
    }

}
