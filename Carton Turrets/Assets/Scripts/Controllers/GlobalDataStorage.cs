using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


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
    atPDir,
    atEDir,
    spin,
}
public enum ControllerUsed
{
    kb,
    gp,
    ph
}

public class EnumArraySaver
{
    // The enum type of the array
    private System.Type enumType;

    public EnumArraySaver(System.Type enumType)
    {
        this.enumType = enumType;
    }

    // Saves the enum array to PlayerPrefs
    public void Save(string key, System.Array array)
    {
        // Convert the enum array to a string array
        string[] stringArray = new string[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            stringArray[i] = array.GetValue(i).ToString();
        }

        // Save the string array to PlayerPrefs
        PlayerPrefs.SetString(key, string.Join(",", stringArray));
    }

    // Loads the enum array from PlayerPrefs
    public System.Array Load(string key)
    {
        // Return an empty array if the key does not exist in PlayerPrefs
        if (!PlayerPrefs.HasKey(key))
        {
            System.Array failArray = System.Array.CreateInstance(enumType, 1);
            failArray.SetValue(System.Enum.ToObject(enumType, 0), 0);
            return failArray;
        }

        // Load the string array from PlayerPrefs
        string[] stringArray = PlayerPrefs.GetString(key).Split(',');

        // Convert the string array to an enum array
        System.Array array = System.Array.CreateInstance(enumType, stringArray.Length);
        for (int i = 0; i < stringArray.Length; i++)
        {
            array.SetValue(System.Enum.Parse(enumType, stringArray[i]), i);
        }

        return array;
    }
}

public static class EnumSorter
{
    public static void SortEnumArray<T>(ref T[] values) where T : Enum
    {
        Array.Sort(values, (a, b) => Convert.ToInt32(a).CompareTo(Convert.ToInt32(b)));
    }
}


public static class PlatformDetector
{
    public static bool IsRunningOnDesktop()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer ||
               Application.platform == RuntimePlatform.LinuxPlayer ||
               Application.platform == RuntimePlatform.OSXPlayer;
    }

    public static bool IsRunningOnMobile()
    {
        return Application.platform == RuntimePlatform.Android ||
               Application.platform == RuntimePlatform.IPhonePlayer;
    }

    public static bool IsRunningOnConsole()
    {
        return Application.platform == RuntimePlatform.PS4 ||
               Application.platform == RuntimePlatform.XboxOne;
    }
}

public class GlobalDataStorage : MonoBehaviour
{
    public static GlobalDataStorage singleton;
    private void Awake()
    {

        if(singleton == null)
        {
            singleton = this;

            if (PlatformDetector.IsRunningOnDesktop())
            {
                ControllerUsed = ControllerUsed.kb;
            }
            else if (PlatformDetector.IsRunningOnMobile())
            {
                ControllerUsed = ControllerUsed.ph;
            }
            else if (PlatformDetector.IsRunningOnConsole())
            {
                ControllerUsed = ControllerUsed.gp;
            }

            if(!PlayerPrefs.HasKey("unlocked_characters")){ResetGame();}

            LoadSave();

            return;
        }
        
        Destroy(this.gameObject);
    }

    public int PlayerMoney = 100;
    public int PlayerTempWallet = 0;

    [Header("Game Options")]
    public float gameVolume;
    public float musicVolume;
    [SerializeField]ControllerUsed _showInInspectorControllerUsed;
    public ControllerUsed ControllerUsed
        {
            get{return _showInInspectorControllerUsed;}
            set{_showInInspectorControllerUsed = value; ControllerUsedChanged.Invoke();}
        }
    [HideInInspector]
    public UnityEvent ControllerUsedChanged;

    [SerializeField]bool _showInInspectorDamageNumbersOn;
    public bool DamageNumbersOn
        {
            get{return _showInInspectorDamageNumbersOn;}
            set{_showInInspectorDamageNumbersOn = value; DamageNumbersOptionChanged.Invoke();}
        }
    [HideInInspector]
    public UnityEvent DamageNumbersOptionChanged;

    [SerializeField]bool _showInInspectorOnScreenControlsOn;
    public bool OnScreenControlsOn
        {
            get{return _showInInspectorOnScreenControlsOn;}
            set{_showInInspectorOnScreenControlsOn = value; OnScreenControlsOptionChanged.Invoke();}
        }
    [HideInInspector]
    public UnityEvent OnScreenControlsOptionChanged;

    [SerializeField]bool _showInInspectorBloodOn;
    public bool BloodOn
        {
            get{return _showInInspectorBloodOn;}
            set{_showInInspectorBloodOn = value; BloodOptionChanged.Invoke();}
        }
    [HideInInspector]
    public UnityEvent BloodOptionChanged;



    [Header("Character Unlocks")]
    [SerializeField]PlayerScriptableObject[] _possiblePlayerSOArray;
    [SerializeField]PlayerCharacters[] _currentlyUnlockedCharacters;
    public PlayerCharacters ChosenCharacter;

    [Header("Map Unlocks")]
    [SerializeField]StagePackageScriptableObject[] _possiblePlayableMapSOArray;
    [SerializeField]PlayableMaps[] _currentlyUnlockedMaps;
    public PlayableMaps ChosenMap;

    [Header("Aim Unlocks")]
    public float AimAtEnemyCheckRange;
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

    [ContextMenu("ResetGame")]
    public void ResetGame()
    {
        Debug.Log("Game Reset");

        PlayerPrefs.DeleteAll();
        PlayerMoney = 100;

        _currentlyUnlockedCharacters = new PlayerCharacters[]{PlayerCharacters.eye};
        _currentlyUnlockedMaps = new PlayableMaps[]{PlayableMaps.rocky};
        _currentlyUnlockedAim = new PlayableAim[]{PlayableAim.playerDir};


        SaveGame();
    }

    [ContextMenu("LoadGame")]
    public void LoadSave()
    {
        Debug.Log("Game Loaded");

        PlayerMoney = PlayerPrefs.GetInt("money", 100);

        gameVolume = PlayerPrefs.GetFloat("gameVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);

        OnScreenControlsOn = PlayerPrefs.GetString("on_screen_controls_on", "True") == "True" ? true : false;
        BloodOn = PlayerPrefs.GetString("blood_on", "False") == "True" ? true : false;
        DamageNumbersOn = PlayerPrefs.GetString("damage_numbers_on", "False") == "True" ? true : false;


        EnumArraySaver enumPArraySaver = new EnumArraySaver(typeof(PlayerCharacters));
        EnumArraySaver enumMArraySaver = new EnumArraySaver(typeof(PlayableMaps));
        EnumArraySaver enumAArraySaver = new EnumArraySaver(typeof(PlayableAim));

        _currentlyUnlockedCharacters = (PlayerCharacters[])enumPArraySaver.Load("unlocked_characters");
        _currentlyUnlockedMaps = (PlayableMaps[])enumMArraySaver.Load("unlocked_maps");
        _currentlyUnlockedAim = (PlayableAim[])enumAArraySaver.Load("unlocked_aim");
    }

    [ContextMenu("SaveGame")]
    public void SaveGame()
    {
        Debug.Log("Game Saved");

        PlayerPrefs.SetInt("money", PlayerMoney);
        PlayerPrefs.SetFloat("gameVolume", gameVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);

        PlayerPrefs.SetString("on_screen_controls_on", OnScreenControlsOn.ToString());
        PlayerPrefs.SetString("blood_on", BloodOn.ToString());
        PlayerPrefs.SetString("damage_numbers_on", DamageNumbersOn.ToString());

        EnumArraySaver enumPArraySaver = new EnumArraySaver(typeof(PlayerCharacters));
        EnumArraySaver enumMArraySaver = new EnumArraySaver(typeof(PlayableMaps));
        EnumArraySaver enumAArraySaver = new EnumArraySaver(typeof(PlayableAim));

        enumPArraySaver.Save("unlocked_characters", _currentlyUnlockedCharacters);
        enumMArraySaver.Save("unlocked_maps", _currentlyUnlockedMaps);
        enumAArraySaver.Save("unlocked_aim", _currentlyUnlockedAim);
    }


    public void UnlockCharacter(PlayerCharacters unlock)
    {
        StageMoneyEarnedIndicatorUI.singlton.GiveGlobalMoneyToTrack();
        PlayerCharacters[] tempArray = new PlayerCharacters[_currentlyUnlockedCharacters.Length + 1];

        for (int i = 0; i < _currentlyUnlockedCharacters.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedCharacters[i];
        }

        tempArray[tempArray.Length -1] = unlock;

        EnumSorter.SortEnumArray(ref tempArray);

        _currentlyUnlockedCharacters = tempArray;
        SaveGame();
    }
    public void UnlockMap(PlayableMaps unlock)
    {
        StageMoneyEarnedIndicatorUI.singlton.GiveGlobalMoneyToTrack();
        PlayableMaps[] tempArray = new PlayableMaps[_currentlyUnlockedMaps.Length + 1];

        for (int i = 0; i < _currentlyUnlockedMaps.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedMaps[i];
        }

        tempArray[tempArray.Length -1] = unlock;

        EnumSorter.SortEnumArray(ref tempArray);

        _currentlyUnlockedMaps = tempArray;
        SaveGame();
    }
    public void UnlockAim(PlayableAim unlock)
    {
        StageMoneyEarnedIndicatorUI.singlton.GiveGlobalMoneyToTrack();
        PlayableAim[] tempArray = new PlayableAim[_currentlyUnlockedAim.Length + 1];

        for (int i = 0; i < _currentlyUnlockedAim.Length; i++)
        {
            tempArray[i] = _currentlyUnlockedAim[i];
        }

        tempArray[tempArray.Length -1] = unlock;

        EnumSorter.SortEnumArray(ref tempArray);

        _currentlyUnlockedAim = tempArray;
        SaveGame();
    }

}
