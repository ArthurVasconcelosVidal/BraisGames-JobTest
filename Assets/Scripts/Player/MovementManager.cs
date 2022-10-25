using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovementManager : MonoBehaviour{
    InputManager InputManager { get => PlayerManager.instance.InputManager; }
    AudioManager AudioManager { get => AudioManager.instance; }
    Rigidbody PlayerRb { get => PlayerManager.instance.PlayerRb; }
    GameObject MeshObject { get => PlayerManager.instance.MeshObject; }
    AnimationManager animationManager { get => PlayerManager.instance.AnimationManager; }
    [SerializeField] float rotationVelocity;
    [SerializeField] float movementSpeed;

    void FixedUpdate() {
        Vector3 direction = ObjectRelatedDirection(InputManager.StickValue, Camera.main.gameObject);
        float stickSqrMag = InputManager.StickValue.sqrMagnitude;
        if (direction != Vector3.zero){
            MovePlayer(direction, stickSqrMag);
            RotatePlayer(direction, stickSqrMag);
        }
        animationManager.AnimatorController.SetFloat("movementVelocity", stickSqrMag);
    }

    Vector3 ObjectRelatedDirection(Vector2 inputDirection, GameObject relatedObject){
        var forwardDirection = Vector3.ProjectOnPlane(relatedObject.transform.forward, transform.up);
        var rightDirection = Vector3.ProjectOnPlane(relatedObject.transform.right, transform.up);
        Vector3 finalDirection = forwardDirection * inputDirection.y + rightDirection * inputDirection.x;
        return finalDirection.normalized;
    }

    void MovePlayer(Vector3 direction, float stickMag){
        PlayerRb.MovePosition(transform.position + direction * (stickMag * movementSpeed) * Time.fixedDeltaTime);
    }

    void RotatePlayer(Vector3 direction, float stickMag){
        var newRotation = Quaternion.LookRotation(direction, MeshObject.transform.up); 
        MeshObject.transform.rotation = Quaternion.Lerp(MeshObject.transform.rotation, newRotation, stickMag * rotationVelocity * Time.fixedDeltaTime);
    }

    public void SilentWalkFootSound(AnimationEvent evt) {
        const float MID_WEIGHT_TRANSITION = 0.5f;
        if(evt.animatorClipInfo.weight > MID_WEIGHT_TRANSITION)
            AudioManager.PlayAudio(SoundList.FootStepLight);
    }
    public void NormalWalkFootSound(AnimationEvent evt){
        const float MID_WEIGHT_TRANSITION = 0.5f;
        bool state = Convert.ToBoolean(evt.intParameter);

        if (state && evt.animatorClipInfo.weight > MID_WEIGHT_TRANSITION)
            AudioManager.PlayAudio(SoundList.FootStepHeavyL);
        else if(evt.animatorClipInfo.weight > MID_WEIGHT_TRANSITION)
            AudioManager.PlayAudio(SoundList.FootStepHeavyR);
    }
}
