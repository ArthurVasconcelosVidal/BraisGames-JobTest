using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
public class InputManager : MonoBehaviour{
    
    PlayerInput playerInput;
    [SerializeField] FloatingJoystick floatingJoystick;
    public bool FloatingJoystickActive { get => floatingJoystick.enabled; }

    [Header("Stick values (DEBUG)")]
    [SerializeField] Vector2 stickValue;
    public Vector2 StickValue { get { return stickValue; } }
    Vector2 fingerStartPosition;
    
    public event EventHandler<InputAction.CallbackContext> 
    OnActionButtonPerformed, OnActionButtonCanceled;

    void Awake() {
        playerInput = new PlayerInput();
    }

    void Start() {
        StickConfig();
        ActionButtonPerformedConfig();
        ActionButtonCanceledConfig();
    }

    void StickConfig(){
        playerInput.ControllerActions.RightStick.performed += ContextMenu => {
            stickValue = ContextMenu.ReadValue<Vector2>();
            stickValue = stickValue.normalized * stickValue.sqrMagnitude;
        };
        playerInput.ControllerActions.RightStick.canceled += ContextMenu => {
            stickValue = Vector2.zero;
        };

        EnhancedTouch.Touch.onFingerDown += finger => {
            fingerStartPosition = finger.currentTouch.startScreenPosition;
            floatingJoystick.joystickRect.anchoredPosition = fingerStartPosition;
        };
        EnhancedTouch.Touch.onFingerMove += finger => {
            if (FloatingJoystickActive){
                var offset = finger.currentTouch.screenPosition - fingerStartPosition;
                Vector2 knobPosition = Vector2.ClampMagnitude(offset, floatingJoystick.MaxKnobMovement);
                floatingJoystick.knobRect.anchoredPosition = knobPosition;
                stickValue = knobPosition/floatingJoystick.MaxKnobMovement;
            }
        };
        EnhancedTouch.Touch.onFingerUp += finger => {
            const int MINIMUM_FINGER_COUNT = 2;
            if(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count < MINIMUM_FINGER_COUNT){
                floatingJoystick.gameObject.SetActive(false);
                fingerStartPosition = Vector2.zero;
                stickValue = Vector2.zero;
                floatingJoystick.knobRect.anchoredPosition = Vector2.zero;
            }
        };

        playerInput.TouchActions.HoldTouch.performed += ctx =>{
            floatingJoystick.gameObject.SetActive(true);
        };
    }

    void ActionButtonPerformedConfig(){
        playerInput.TouchActions.TapTouch.performed += ContextMenu => OnActionButtonPerformed?.Invoke(this,ContextMenu);
        playerInput.ControllerActions.SouthButton.performed += ContextMenu => OnActionButtonPerformed?.Invoke(this,ContextMenu);
    }

    void ActionButtonCanceledConfig(){
        playerInput.TouchActions.TapTouch.canceled += ContextMenu => OnActionButtonCanceled?.Invoke(this,ContextMenu);
        playerInput.ControllerActions.SouthButton.canceled += ContextMenu => OnActionButtonCanceled?.Invoke(this,ContextMenu);
    }

    void OnEnable() {
        playerInput.Enable();
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable(); //Debug Purpose
    }

    void OnDisable() {
        playerInput.Disable();
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable(); //Debug Purpose
    }
}
