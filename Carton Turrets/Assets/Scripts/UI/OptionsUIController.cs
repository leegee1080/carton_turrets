using System.Collections;
using System.Collections.Generic;
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
        // _effectsSlider.value = Whatever the sound singlton is;
        // _musicSlider.value = Whatever the sound singlton is;
        // _effectsValue.text = ""+Whatever the sound singlton is;
        // _musicValue.text = ""+Whatever the sound singlton is;
    }
    public void ChangeEffectVolume(float change)
    {
        print("New Effect Volume: " + change);
        _effectsValue.text = ""+change;
        // whatever the sound singlton is = change;
    }
    public void ChangeMusicVolume(float change)
    {
        print("New Music Volume: " + change);
        _musicValue.text = ""+change;
        // whatever the sound singlton is = change;
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
