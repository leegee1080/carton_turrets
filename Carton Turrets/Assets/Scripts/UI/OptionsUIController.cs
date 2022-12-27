using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateGameOptionsToggles();

        UpdateVolumeSliders();
    }

#region SoundOptions
    [SerializeField] Slider _effectsSlider;
    [SerializeField] Slider _musicSlider;
    [SerializeField] TMP_Text _musicValue;
    [SerializeField] TMP_Text _effectsValue;



    public void UpdateVolumeSliders()
    {
        _effectsSlider.value = AudioController.singleton.currentGameVolumeLevel * 10;
        _musicSlider.value = AudioController.singleton.currentMusicVolumeLevel * 10;
        _effectsValue.text = "" + (AudioController.singleton.currentGameVolumeLevel * 10);
        _musicValue.text = "" + (AudioController.singleton.currentMusicVolumeLevel * 10);
    }
    public void ChangeEffectVolume(float change)
    {
        _effectsValue.text = ""+change;
        AudioController.singleton.ChangeVolume(change/10, Sound_Type_Tags.fx);
    }
    public void ChangeMusicVolume(float change)
    {
        _musicValue.text = ""+change;
        AudioController.singleton.ChangeVolume(change/10, Sound_Type_Tags.music);
    }

#endregion

#region GameOptions
    [SerializeField] GameObject _bloodToggleIndicatorGO;
    [SerializeField] GameObject _damnumbToggleIndicatorGO;
    [SerializeField] GameObject _onscContToggleIndicatorGO;

    public void UpdateGameOptionsToggles()
    {
        _bloodToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.BloodOn);
        _damnumbToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.DamageNumbersOn);
        _onscContToggleIndicatorGO.SetActive(GlobalDataStorage.singleton.OnScreenControlsOn);
    }

    public void ToggleBlood()
    {
        GlobalDataStorage.singleton.BloodOn = !GlobalDataStorage.singleton.BloodOn;
        UpdateGameOptionsToggles();
    }
    public void ToggleDamageNumbers()
    {
        GlobalDataStorage.singleton.DamageNumbersOn = !GlobalDataStorage.singleton.DamageNumbersOn;
        UpdateGameOptionsToggles();
    }
    public void ToggleOnScreenControls()
    {
        GlobalDataStorage.singleton.OnScreenControlsOn = !GlobalDataStorage.singleton.OnScreenControlsOn;
        UpdateGameOptionsToggles();
    }
#endregion
}
