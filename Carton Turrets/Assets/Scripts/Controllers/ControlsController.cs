using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[Serializable]
public class HighlighterPackage
{
    public MenuControlsChild[] _availableControls;
    public MenuControlsChild _selectedControl;
    public MenuControlsChild _forceAcceptControl;
    public MenuControlsChild _forceDenyControl;
    public bool BlockCycliing;
}

public class ControlsController : MonoBehaviour
{
    public static ControlsController singleton;
    private void Awake()
    {

        if(singleton == null)
        {
            singleton = this;
            PlayerInputActions = new PiaMainControls();
            return;
        }
        
        Destroy(this.gameObject);
    }

    private PiaMainControls PlayerInputActions;
    public InputAction accept, deny, selectUp, selectDown, selectLeft, selectRight, pause;

    [SerializeField]HighlighterPackage _showInspectorCurrentHighligherPackage;    
    public HighlighterPackage CurrentHighligherPackage
    {
        get{return _showInspectorCurrentHighligherPackage;}
        set{_showInspectorCurrentHighligherPackage = value; UpdateHighligherPackage();}
    }
    [SerializeField]MenuControlsChild[] _availableControls;
    [SerializeField]bool _selectAvailable;
    [SerializeField]MenuControlsChild _selectedControl;
    int _menuControlsChildIndex= 0;

    public UnityEvent AcceptPressedEvent;
    public UnityEvent DenyPressedEvent;
    public UnityEvent UpPressedEvent;
    public UnityEvent RightPressedEvent;
    public UnityEvent LeftPressedEvent;
    public UnityEvent DownPressedEvent;
    public UnityEvent PausePressedEvent;


    public void OnEnable()
    {
        pause = PlayerInputActions.MainMap.Pause;
        pause.Enable();
        pause.performed += ctx => PausePressed(ctx);

        accept = PlayerInputActions.MainMap.Accept;
        accept.Enable();
        accept.performed += ctx => AcceptPressed(ctx);

        deny = PlayerInputActions.MainMap.Deny;
        deny.Enable();
        deny.performed += ctx => DenyPressed(ctx);

        selectUp = PlayerInputActions.MainMap.SelectUp;
        selectUp.Enable();
        selectUp.performed += ctx => UpPressed(ctx);

        selectDown = PlayerInputActions.MainMap.SelectDown;
        selectDown.Enable();
        selectDown.performed += ctx => DownPressed(ctx);

        selectLeft = PlayerInputActions.MainMap.SelectLeft;
        selectLeft.Enable();
        selectLeft.performed += ctx => LeftPressed(ctx);

        selectRight = PlayerInputActions.MainMap.SelectRight;
        selectRight.Enable();
        selectRight.performed += ctx => RightPressed(ctx);
    }
    private void OnDisable()
    {
        accept.Disable();
        selectUp.Disable();
        selectDown.Disable();
        selectLeft.Disable();
        selectRight.Disable();
        pause.Disable();
    }

    public void AcceptPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        AcceptPressedEvent.Invoke();
        if(CurrentHighligherPackage != null && CurrentHighligherPackage._forceAcceptControl != null){CurrentHighligherPackage._forceAcceptControl.AcceptPressed();return;}
        if(_selectedControl != null){_selectedControl.AcceptPressed();}
    }
    public void DenyPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        DenyPressedEvent.Invoke();
        if(CurrentHighligherPackage != null && CurrentHighligherPackage._forceDenyControl != null){CurrentHighligherPackage._forceDenyControl.AcceptPressed();return;}
    }
    public void UpPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        UpPressedEvent.Invoke();
        CycleSelected(-1);
    }
    public void RightPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        RightPressedEvent.Invoke();
        if(_selectedControl != null){_selectedControl.RightPressed();}
    }
    public void LeftPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        LeftPressedEvent.Invoke();
        if(_selectedControl != null){_selectedControl.LeftPressed();}
    }
    public void DownPressed(InputAction.CallbackContext ctx)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        DownPressedEvent.Invoke();
        CycleSelected(1);
    }
    public void PausePressed(InputAction.CallbackContext ctx)
    {
        PausePressedEvent.Invoke();
    }

    public void UpdateHighligherPackage()
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        foreach (MenuControlsChild item in _availableControls)
        {
            item.UpdateHighligher(false);
            item.gameObject.SetActive(false);
        }

        _availableControls = CurrentHighligherPackage._availableControls;
        _selectedControl = CurrentHighligherPackage._selectedControl;

        foreach (MenuControlsChild item in _availableControls)
        {
            if(item == null){continue;}
            item.gameObject.SetActive(true);
        }

        _menuControlsChildIndex = 0;
        if(_selectedControl != null){_selectedControl.UpdateHighligher(true);}
    }

    public void CycleSelected(int dir)
    {
        if(!GlobalDataStorage.singleton.OnScreenControlsOn){return;}
        if(!_selectAvailable){return;}
        if(CurrentHighligherPackage == null || _availableControls == null || _availableControls.Length == 0){return;}
        if(CurrentHighligherPackage.BlockCycliing){return;}

        _menuControlsChildIndex += dir;
        if(_menuControlsChildIndex <= -1) {_menuControlsChildIndex = _availableControls.Length-1;} 
        if(_menuControlsChildIndex >= _availableControls.Length){_menuControlsChildIndex = 0;}


        if(_selectedControl != null){_selectedControl.UpdateHighligher(false);}

        _selectedControl = _availableControls[_menuControlsChildIndex];

        _selectedControl.UpdateHighligher(true);
    }
}
