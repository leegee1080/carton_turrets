using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuControlsChild : MonoBehaviour
{
    [SerializeField]Sprite[] _possibleSprites;
    [SerializeField]bool _reactToControlType;
    [SerializeField]SpriteRenderer _sr;
    [SerializeField]GameObject _highlighter;
    public UnityEvent AcceptPressedEvent;
    public UnityEvent DenyPressedEvent;
    public UnityEvent LeftPressedEvent;
    public UnityEvent RightPressedEvent;
    [SerializeField]Button _simulatedButton;
    [SerializeField]Slider _simulatedSlider;

    private void OnDestroy()
    {
        GlobalDataStorage.singleton.ControllerUsedChanged.RemoveListener(UpdateControlType);
        GlobalDataStorage.singleton.OnScreenControlsOptionChanged.RemoveListener(UpdateOnScreen);
    }
    private void Start()
    {
        _highlighter.SetActive(false);
        GlobalDataStorage.singleton.ControllerUsedChanged.AddListener(UpdateControlType);
        GlobalDataStorage.singleton.OnScreenControlsOptionChanged.AddListener(UpdateOnScreen);
    }

    public void UpdateHighligher(bool toggle)
    {
        _highlighter.SetActive(toggle);
    }

    public void AcceptPressed()
    {
        AcceptPressedEvent.Invoke();
        if(_simulatedButton == null){return;}
        _simulatedButton.onClick.Invoke();
    }
    public void DenyPressed()
    {
        DenyPressedEvent.Invoke();
        if(_simulatedButton == null){return;}
        _simulatedButton.onClick.Invoke();
    }


    public void LeftPressed()
    {
        LeftPressedEvent.Invoke();
        if(_simulatedSlider == null){return;}
        _simulatedSlider.value += -1;
        _simulatedSlider.onValueChanged.Invoke(_simulatedSlider.value);
    }
    public void RightPressed()
    {
        RightPressedEvent.Invoke();
        if(_simulatedSlider == null){return;}
        _simulatedSlider.value += 1;
        _simulatedSlider.onValueChanged.Invoke(_simulatedSlider.value);
    }

    public void UpdateOnScreen()
    {
        _sr.gameObject.SetActive(GlobalDataStorage.singleton.OnScreenControlsOn);
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){_highlighter.gameObject.SetActive(GlobalDataStorage.singleton.OnScreenControlsOn);}
    }

    public void UpdateControlType()
    {
        if(_sr == null || !_sr.gameObject.activeSelf){return;}
        _sr.sprite = _possibleSprites[(int)GlobalDataStorage.singleton.ControllerUsed];
    }
}
