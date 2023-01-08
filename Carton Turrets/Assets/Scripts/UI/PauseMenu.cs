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
    [SerializeField] GameObject _pauseMenuDimmerGO;

    [SerializeField]GameObject[] _pauseMenuObjects;
    [SerializeField]TMP_Text _pauseMenuTitleText;
    [SerializeField]GameObject[] _UIToHideOnPause;
    [SerializeField]PlayerStatPauseMenu[] _StatBlocks;
    bool _gamePaused;

    [SerializeField]TMP_Text _quitButtonText;
    [SerializeField]TMP_Text _pauseButtonText;



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
        if(!StageController.singlton.PauseButtonGO.activeSelf){return;}
        if(_gamePaused){UnPauseGame(); return;}
        PauseGame(type);
    }

    public void PauseGame(PauseMenuType type)
    {

        PauseGameEvent.Invoke();

        _pauseMenuDimmerGO.SetActive(true);

        _pauseButtonText.text = ">";

        StageController.singlton.ShowPause();

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

        _pauseMenuDimmerGO.SetActive(false);

        _pauseButtonText.text = "||";

        StageController.singlton.ShowMainControls();

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
                _quitButtonText.text = "Leave Stage";
                break;
            case PauseMenuType.lose:
                _pauseMenuTitleText.text = "Game Over";
                _quitButtonText.text = "Leave Stage";
                break;
            case PauseMenuType.win:
                _pauseMenuTitleText.text = "Level Complete";
                _quitButtonText.text = "Leave Stage";
                break;
            default:
                break;
        }
    }

    public void LeaveStage()
    {
        AudioController.singleton.StopSound("endgame_nuke_alarm");
        AudioController.singleton.PlaySound("ui_gamestart");
        AudioController.singleton.FadeSoundOut(0.05f, StageController.singlton.CurrentStage.SignatureMusic);

        GlobalDataStorage.singleton.PlayerTempWallet += StageMoneyEarnedIndicatorUI.singlton.PublicMoneyAmountEarnedInLevel;
        if(GlobalDataStorage.singleton.PlayerTempWallet<=0){GlobalVolumeController.singleton.NewScene(1);return;}//no money no bonus
        GlobalVolumeController.singleton.NewScene(3);

    }
}
