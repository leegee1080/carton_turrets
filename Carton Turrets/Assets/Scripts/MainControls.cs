//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.2.0
//     from Assets/piaMainControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MainControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""piaMainControls"",
    ""maps"": [
        {
            ""name"": ""MainMap"",
            ""id"": ""ef0b6ca8-6e20-4b0e-a517-6f4ca56a391c"",
            ""actions"": [
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""6cb0eccb-fed1-4866-b85a-20f56c30a15d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""d22bc6c9-102f-4760-8b1e-216bd340b3b8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""135c2e07-da9c-4b45-ae01-673cc3412cde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""714822b7-b70d-4606-947e-8f31361c2454"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d4953028-292a-487a-b405-bccd6a5217df"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e3fd488-ed71-4584-b4c8-5b572ea5d65b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1382a902-e2e2-4cc8-ad9a-b1c1e7e1a66c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf9cb2d5-34e5-4801-a0f4-d70ca9be0ab2"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MainMap
        m_MainMap = asset.FindActionMap("MainMap", throwIfNotFound: true);
        m_MainMap_MoveUp = m_MainMap.FindAction("MoveUp", throwIfNotFound: true);
        m_MainMap_MoveRight = m_MainMap.FindAction("MoveRight", throwIfNotFound: true);
        m_MainMap_MoveDown = m_MainMap.FindAction("MoveDown", throwIfNotFound: true);
        m_MainMap_MoveLeft = m_MainMap.FindAction("MoveLeft", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // MainMap
    private readonly InputActionMap m_MainMap;
    private IMainMapActions m_MainMapActionsCallbackInterface;
    private readonly InputAction m_MainMap_MoveUp;
    private readonly InputAction m_MainMap_MoveRight;
    private readonly InputAction m_MainMap_MoveDown;
    private readonly InputAction m_MainMap_MoveLeft;
    public struct MainMapActions
    {
        private @MainControls m_Wrapper;
        public MainMapActions(@MainControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveUp => m_Wrapper.m_MainMap_MoveUp;
        public InputAction @MoveRight => m_Wrapper.m_MainMap_MoveRight;
        public InputAction @MoveDown => m_Wrapper.m_MainMap_MoveDown;
        public InputAction @MoveLeft => m_Wrapper.m_MainMap_MoveLeft;
        public InputActionMap Get() { return m_Wrapper.m_MainMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainMapActions set) { return set.Get(); }
        public void SetCallbacks(IMainMapActions instance)
        {
            if (m_Wrapper.m_MainMapActionsCallbackInterface != null)
            {
                @MoveUp.started -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveUp;
                @MoveUp.performed -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveUp;
                @MoveUp.canceled -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveUp;
                @MoveRight.started -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveRight;
                @MoveDown.started -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveDown;
                @MoveDown.performed -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveDown;
                @MoveDown.canceled -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveDown;
                @MoveLeft.started -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.performed -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveLeft;
                @MoveLeft.canceled -= m_Wrapper.m_MainMapActionsCallbackInterface.OnMoveLeft;
            }
            m_Wrapper.m_MainMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveUp.started += instance.OnMoveUp;
                @MoveUp.performed += instance.OnMoveUp;
                @MoveUp.canceled += instance.OnMoveUp;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @MoveDown.started += instance.OnMoveDown;
                @MoveDown.performed += instance.OnMoveDown;
                @MoveDown.canceled += instance.OnMoveDown;
                @MoveLeft.started += instance.OnMoveLeft;
                @MoveLeft.performed += instance.OnMoveLeft;
                @MoveLeft.canceled += instance.OnMoveLeft;
            }
        }
    }
    public MainMapActions @MainMap => new MainMapActions(this);
    public interface IMainMapActions
    {
        void OnMoveUp(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnMoveDown(InputAction.CallbackContext context);
        void OnMoveLeft(InputAction.CallbackContext context);
    }
}