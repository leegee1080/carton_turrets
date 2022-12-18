using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;

public enum PauseMenuType
{
    lose,
    win,
    normal
}

[System.Serializable]
public class PauseGame : UnityEvent{}

[System.Serializable]
public class UnPauseGame : UnityEvent{}

public class PauseMenu : MonoBehaviour
{
    [SerializeField]public PauseGame PauseGameEvent;
    [SerializeField]public UnPauseGame UnPauseGameEvent;

    [SerializeField]GameObject[] _pauseMenuObjects;
    [SerializeField]TMP_Text _pauseMenuTitleText;
    [SerializeField]GameObject[] _UIToHideOnPause;
    [SerializeField]PlayerStatPauseMenu[] _StatBlocks;
    bool _gamePaused;



    public static PauseMenu singleton;
    private void Awake() => singleton = this;

    private void Start()
    {
        StageController.singlton.pause.performed += ctx => PauseToggle(ctx, PauseMenuType.normal);
        foreach (GameObject item in _pauseMenuObjects)
        {
            item.SetActive(false);
        }

        _gamePaused = false;
    }

    public void ButtonExposedPauseToggle()
    {
        PauseToggle(new InputAction.CallbackContext(), PauseMenuType.normal);
    }

    public void PauseToggle(InputAction.CallbackContext ctx, PauseMenuType type)
    {
        if(_gamePaused){UnPauseGame(); return;}
        PauseGame(type);
    }

    public void PauseGame(PauseMenuType type)
    {

        PauseGameEvent.Invoke();

        foreach (GameObject item in _UIToHideOnPause)
        {
            Time.timeScale = 0;
            item.SetActive(false);
        }

        foreach (GameObject item in _pauseMenuObjects)
        {
            item.SetActive(true);
        }

        foreach (PlayerStatPauseMenu item in _StatBlocks)
        {
            Dictionary<PlayerStatEnum, float> localPlayerCurrentStatDict = StageController.singlton.Player.PlayerCurrentStatDict;

            item.UpdateStatBlock(localPlayerCurrentStatDict[item._upgradeType].ToString());
        }
        ChangePauseType(type);
        _gamePaused = true;
    }

    public void UnPauseGame()
    {

        UnPauseGameEvent.Invoke();

        foreach (GameObject item in _UIToHideOnPause)
        {
            Time.timeScale = 1;
            item.SetActive(true);
        }

        foreach (GameObject item in _pauseMenuObjects)
        {
            item.SetActive(false);
        }

        _gamePaused = false;
    }

    public void ChangePauseType(PauseMenuType type)
    {
        switch (type)
        {
            case PauseMenuType.normal:
                _pauseMenuTitleText.text = "Game Paused";
                break;
            case PauseMenuType.lose:
                _pauseMenuTitleText.text = "Game Over";
                break;
            case PauseMenuType.win:
                _pauseMenuTitleText.text = "Level Complete";
                break;
            default:
                break;
        }
    }
}
