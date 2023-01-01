using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsController : MonoBehaviour
{
    [SerializeField] HighlighterPackage _creditsHighligher;
    private void Start()
    {
        CreditsScreenIn();
        ControlsController.singleton.CurrentHighligherPackage = _creditsHighligher;
    }

    public void BacktoMainMenu()
    {
        AudioController.singleton.FadeSoundOut(0.005f, "music_credits");
        GlobalVolumeController.singleton.NewScene(1);
    }

    private void CreditsScreenIn()
    {
        AudioController.singleton.FadeSoundIn(0.005f, "music_credits");
        GlobalVolumeController.singleton.ShowScene();
    }
}
