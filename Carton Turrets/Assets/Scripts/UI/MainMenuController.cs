using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]float _timeBufferForSceneLoad;
    private void Start()
    {
        IEnumerator BufferTimer()
        {
            yield return new WaitForSecondsRealtime(_timeBufferForSceneLoad);
            GlobalVolumeController.singleton.ShowScene();
        }
        StartCoroutine(BufferTimer());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
