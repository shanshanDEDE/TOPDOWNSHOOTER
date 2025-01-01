//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Input Manager/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Charcater"",
            ""id"": ""84c034d6-cf8c-4c08-9733-898b68ebe0b3"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""a248f4e0-5f5f-4c8a-b633-a13bc4c23a23"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""150de37c-9679-4e72-aa69-98a3a4f69dcd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""7ce42636-e472-4ff3-ad96-c23089d868dc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""59bc2eb0-6df0-432d-bf29-83e944d72b47"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 1"",
                    ""type"": ""Button"",
                    ""id"": ""8cc4117f-eb43-4dcd-a774-516076dcf77c"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 2"",
                    ""type"": ""Button"",
                    ""id"": ""8fa71b85-ce86-4f6c-9aa7-b242ac77e8bf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 3"",
                    ""type"": ""Button"",
                    ""id"": ""b1f48131-ff66-41fe-a6cc-43413312eda3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 4"",
                    ""type"": ""Button"",
                    ""id"": ""0676696e-a74d-42f2-ab5b-9d3487f8b388"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Equip Slot - 5"",
                    ""type"": ""Button"",
                    ""id"": ""b9bdf238-e66b-4d68-a656-333dd82fd47f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Drop Current Weapon"",
                    ""type"": ""Button"",
                    ""id"": ""0dfd7b29-92cd-4681-9894-09ebf5348736"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""6338260f-8306-4e03-897b-6fc60d6a243e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Toogle Weapon Mode"",
                    ""type"": ""Button"",
                    ""id"": ""b9ba80e0-55b7-4d5a-afa3-84bbe357c032"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""959f1faf-3327-4ae7-beb7-28ae5b394c92"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0c6b233b-9263-4099-a17d-6c7c30f83577"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""1fe3cdbd-4c1c-4107-b5e2-a46ba4e53ca9"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""02ba689f-9e43-43dc-9805-0f11a282d05d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""788637cf-f485-4b3b-9e3c-a5ec76957c6d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bbf08bb9-a795-4e7a-89da-c09180c73fa7"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""35c50fdd-0fec-48e1-b8c5-13a3b990b1f3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""746a4320-2b71-4ce9-8996-55bd08b978d0"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5cf0bb4e-7f52-476c-a237-8aea94f0d32f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0413a46e-58d5-4191-9541-055ddc18b04b"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4abf95f1-109f-4e8b-bd56-7aa6b6f237ab"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e849e4da-176f-4e28-aaf2-9dca8f6b25c3"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5963997d-6af1-4b0a-9c8e-9164485f91e0"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e24acf73-4508-4e4e-8898-74334c66dc4e"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Equip Slot - 5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dae13a69-2160-466f-9362-66d8e159a212"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop Current Weapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84504073-4381-423e-9a23-102adbf18d66"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""643d49be-0503-4c99-b72a-c8bfed5db175"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toogle Weapon Mode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee046f7e-52a6-4fc6-9bbb-a4b78f948e3b"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Charcater
        m_Charcater = asset.FindActionMap("Charcater", throwIfNotFound: true);
        m_Charcater_Fire = m_Charcater.FindAction("Fire", throwIfNotFound: true);
        m_Charcater_Movement = m_Charcater.FindAction("Movement", throwIfNotFound: true);
        m_Charcater_Aim = m_Charcater.FindAction("Aim", throwIfNotFound: true);
        m_Charcater_Run = m_Charcater.FindAction("Run", throwIfNotFound: true);
        m_Charcater_EquipSlot1 = m_Charcater.FindAction("Equip Slot - 1", throwIfNotFound: true);
        m_Charcater_EquipSlot2 = m_Charcater.FindAction("Equip Slot - 2", throwIfNotFound: true);
        m_Charcater_EquipSlot3 = m_Charcater.FindAction("Equip Slot - 3", throwIfNotFound: true);
        m_Charcater_EquipSlot4 = m_Charcater.FindAction("Equip Slot - 4", throwIfNotFound: true);
        m_Charcater_EquipSlot5 = m_Charcater.FindAction("Equip Slot - 5", throwIfNotFound: true);
        m_Charcater_DropCurrentWeapon = m_Charcater.FindAction("Drop Current Weapon", throwIfNotFound: true);
        m_Charcater_Reload = m_Charcater.FindAction("Reload", throwIfNotFound: true);
        m_Charcater_ToogleWeaponMode = m_Charcater.FindAction("Toogle Weapon Mode", throwIfNotFound: true);
        m_Charcater_Interaction = m_Charcater.FindAction("Interaction", throwIfNotFound: true);
    }

    ~@PlayerControls()
    {
        UnityEngine.Debug.Assert(!m_Charcater.enabled, "This will cause a leak and performance issues, PlayerControls.Charcater.Disable() has not been called.");
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

    // Charcater
    private readonly InputActionMap m_Charcater;
    private List<ICharcaterActions> m_CharcaterActionsCallbackInterfaces = new List<ICharcaterActions>();
    private readonly InputAction m_Charcater_Fire;
    private readonly InputAction m_Charcater_Movement;
    private readonly InputAction m_Charcater_Aim;
    private readonly InputAction m_Charcater_Run;
    private readonly InputAction m_Charcater_EquipSlot1;
    private readonly InputAction m_Charcater_EquipSlot2;
    private readonly InputAction m_Charcater_EquipSlot3;
    private readonly InputAction m_Charcater_EquipSlot4;
    private readonly InputAction m_Charcater_EquipSlot5;
    private readonly InputAction m_Charcater_DropCurrentWeapon;
    private readonly InputAction m_Charcater_Reload;
    private readonly InputAction m_Charcater_ToogleWeaponMode;
    private readonly InputAction m_Charcater_Interaction;
    public struct CharcaterActions
    {
        private @PlayerControls m_Wrapper;
        public CharcaterActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Charcater_Fire;
        public InputAction @Movement => m_Wrapper.m_Charcater_Movement;
        public InputAction @Aim => m_Wrapper.m_Charcater_Aim;
        public InputAction @Run => m_Wrapper.m_Charcater_Run;
        public InputAction @EquipSlot1 => m_Wrapper.m_Charcater_EquipSlot1;
        public InputAction @EquipSlot2 => m_Wrapper.m_Charcater_EquipSlot2;
        public InputAction @EquipSlot3 => m_Wrapper.m_Charcater_EquipSlot3;
        public InputAction @EquipSlot4 => m_Wrapper.m_Charcater_EquipSlot4;
        public InputAction @EquipSlot5 => m_Wrapper.m_Charcater_EquipSlot5;
        public InputAction @DropCurrentWeapon => m_Wrapper.m_Charcater_DropCurrentWeapon;
        public InputAction @Reload => m_Wrapper.m_Charcater_Reload;
        public InputAction @ToogleWeaponMode => m_Wrapper.m_Charcater_ToogleWeaponMode;
        public InputAction @Interaction => m_Wrapper.m_Charcater_Interaction;
        public InputActionMap Get() { return m_Wrapper.m_Charcater; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharcaterActions set) { return set.Get(); }
        public void AddCallbacks(ICharcaterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharcaterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharcaterActionsCallbackInterfaces.Add(instance);
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @EquipSlot1.started += instance.OnEquipSlot1;
            @EquipSlot1.performed += instance.OnEquipSlot1;
            @EquipSlot1.canceled += instance.OnEquipSlot1;
            @EquipSlot2.started += instance.OnEquipSlot2;
            @EquipSlot2.performed += instance.OnEquipSlot2;
            @EquipSlot2.canceled += instance.OnEquipSlot2;
            @EquipSlot3.started += instance.OnEquipSlot3;
            @EquipSlot3.performed += instance.OnEquipSlot3;
            @EquipSlot3.canceled += instance.OnEquipSlot3;
            @EquipSlot4.started += instance.OnEquipSlot4;
            @EquipSlot4.performed += instance.OnEquipSlot4;
            @EquipSlot4.canceled += instance.OnEquipSlot4;
            @EquipSlot5.started += instance.OnEquipSlot5;
            @EquipSlot5.performed += instance.OnEquipSlot5;
            @EquipSlot5.canceled += instance.OnEquipSlot5;
            @DropCurrentWeapon.started += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled += instance.OnDropCurrentWeapon;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @ToogleWeaponMode.started += instance.OnToogleWeaponMode;
            @ToogleWeaponMode.performed += instance.OnToogleWeaponMode;
            @ToogleWeaponMode.canceled += instance.OnToogleWeaponMode;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
        }

        private void UnregisterCallbacks(ICharcaterActions instance)
        {
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @EquipSlot1.started -= instance.OnEquipSlot1;
            @EquipSlot1.performed -= instance.OnEquipSlot1;
            @EquipSlot1.canceled -= instance.OnEquipSlot1;
            @EquipSlot2.started -= instance.OnEquipSlot2;
            @EquipSlot2.performed -= instance.OnEquipSlot2;
            @EquipSlot2.canceled -= instance.OnEquipSlot2;
            @EquipSlot3.started -= instance.OnEquipSlot3;
            @EquipSlot3.performed -= instance.OnEquipSlot3;
            @EquipSlot3.canceled -= instance.OnEquipSlot3;
            @EquipSlot4.started -= instance.OnEquipSlot4;
            @EquipSlot4.performed -= instance.OnEquipSlot4;
            @EquipSlot4.canceled -= instance.OnEquipSlot4;
            @EquipSlot5.started -= instance.OnEquipSlot5;
            @EquipSlot5.performed -= instance.OnEquipSlot5;
            @EquipSlot5.canceled -= instance.OnEquipSlot5;
            @DropCurrentWeapon.started -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled -= instance.OnDropCurrentWeapon;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @ToogleWeaponMode.started -= instance.OnToogleWeaponMode;
            @ToogleWeaponMode.performed -= instance.OnToogleWeaponMode;
            @ToogleWeaponMode.canceled -= instance.OnToogleWeaponMode;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
        }

        public void RemoveCallbacks(ICharcaterActions instance)
        {
            if (m_Wrapper.m_CharcaterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharcaterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharcaterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharcaterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharcaterActions @Charcater => new CharcaterActions(this);
    public interface ICharcaterActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnEquipSlot1(InputAction.CallbackContext context);
        void OnEquipSlot2(InputAction.CallbackContext context);
        void OnEquipSlot3(InputAction.CallbackContext context);
        void OnEquipSlot4(InputAction.CallbackContext context);
        void OnEquipSlot5(InputAction.CallbackContext context);
        void OnDropCurrentWeapon(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnToogleWeaponMode(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
    }
}
