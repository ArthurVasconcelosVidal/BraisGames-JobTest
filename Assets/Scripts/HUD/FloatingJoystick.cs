using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingJoystick : MonoBehaviour{
    public RectTransform joystickRect;
    public RectTransform knobRect;
    float maxKnobMovement;
    public float MaxKnobMovement { get => maxKnobMovement; }

    void OnEnable() {
        const float OCCUPIED_JOYSTICK_AREA = 0.15f;
        const float OCCUPIED_KNOB_AREA = 0.15f;
        const float HALF_LENGTH = 2f;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        SetRectScreenSize(joystickRect, OCCUPIED_JOYSTICK_AREA, screenSize);
        SetRectScreenSize(knobRect, OCCUPIED_KNOB_AREA, joystickRect.sizeDelta);
        maxKnobMovement = joystickRect.sizeDelta.x/HALF_LENGTH;
    }

    void SetRectScreenSize(RectTransform rectTransform, float occupiedArea, Vector2 areaSize){
        float areaMag = areaSize.magnitude;
        float rectSize = areaMag * occupiedArea;
        rectTransform.sizeDelta = new Vector2(rectSize,rectSize);
    }
}
