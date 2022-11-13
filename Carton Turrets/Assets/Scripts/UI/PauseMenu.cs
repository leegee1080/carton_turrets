using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]GameObject[] _pauseMenuObjects;
    [SerializeField]GameObject[] _UIToHideOnPause;
    [SerializeField]PlayerStatPauseMenu[] _StatBlocks;
    public PauseMenu singleton;

    private void Awake() => singleton = this;

    private void Start()
    {
        foreach (GameObject item in _pauseMenuObjects)
        {
            item.SetActive(false);
        }
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
    }
}
