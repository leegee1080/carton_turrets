using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]GameObject[] _pauseMenuObjects;
    [SerializeField]GameObject[] _UIToHideOnPause;
    [SerializeField]PlayerStatPauseMenu[] _StatBlocks;
    bool _gamePaused;


    public static PauseMenu singleton;
    private void Awake() => singleton = this;

    private void Start()
    {
        StageController.singlton.pause.performed += ctx => PauseToggle(ctx);
        foreach (GameObject item in _pauseMenuObjects)
        {
            item.SetActive(false);
        }

        _gamePaused = false;
    }

    public void ButtonExposedPauseToggle()
    {
        PauseToggle(new InputAction.CallbackContext());
    }

    public void PauseToggle(InputAction.CallbackContext ctx)
    {
        if(_gamePaused){UnPauseGame(); return;}
        PauseGame();
    }

    public void PauseGame()
    {
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

        _gamePaused = true;
    }

    public void UnPauseGame()
    {
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
}
