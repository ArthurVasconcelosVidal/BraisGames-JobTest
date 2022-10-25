using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class GetObject : InteractionBehavior{

    GameObject MeshObject { get => PlayerManager.instance.MeshObject; }
    GameObject UpHeadPoint { get => PlayerManager.instance.UpHeadPoint; }
    [SerializeField] Rigidbody objectRb;
    [SerializeField] Collider objectCollider;
    [SerializeField] float toUpHeadSpeed;
    [SerializeField] float throwForce;
    [SerializeField] float triggerCoolDownTime = 2;
    [SerializeField] bool isUsing = false;

    protected override void ActionBehavior(){
        if (!isUsing && !PlayerManager.IsOccupied){
            isUsing = true;
            PlayerManager.IsOccupied = true;
            triggerCollider.enabled = false;
            ToUpHead(UpHeadPoint, toUpHeadSpeed);
            this.transform.SetParent(UpHeadPoint.transform);
        }else{
            isUsing = false;
            PlayerManager.IsOccupied = false;
            this.transform.SetParent(null);
            ThrowObject(throwForce);
            CoolDownTrigger(triggerCoolDownTime);
        } 
    }

    async void ToUpHead(GameObject point, float speed) {
        const float END_TIME = 1;
        float percent = 0;
        Vector3 startPosition = transform.position;
        objectRb.isKinematic = true;
        objectCollider.enabled = false;
        
        while (percent < END_TIME) {
            percent += speed * Time.fixedDeltaTime;
            transform.position = Vector3.Slerp(startPosition, point.transform.position, percent);
            await Task.Yield();
        }
    }

    void ThrowObject(float throwForce){
        const float FORWARD_FORCE = 4;
        objectRb.isKinematic = false;
        objectCollider.enabled = true;
        var direction = (MeshObject.transform.up + (MeshObject.transform.forward * FORWARD_FORCE)).normalized;
        objectRb.AddForce(direction * throwForce, ForceMode.Impulse);
    }
}
