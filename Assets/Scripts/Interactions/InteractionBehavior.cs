using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
public abstract class InteractionBehavior : MonoBehaviour{
    protected PlayerManager PlayerManager { get => PlayerManager.instance; }
    protected InputManager InputManager { get => PlayerManager.instance.InputManager; }
    [SerializeField] protected Collider triggerCollider;
    protected abstract void ActionBehavior();

    protected async void CoolDownTrigger(float coolDownTime){
        InputManager.OnActionButtonPerformed -= OnTap;
        const int MILLISECONDS_CONVERSION = 1000;
        await Task.Delay((int)(coolDownTime * MILLISECONDS_CONVERSION));
        triggerCollider.enabled = true;
    }

    void OnTap(object sender, InputAction.CallbackContext buttonContext) {
        ActionBehavior();
    }
    
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag(GameTags.Player.ToString()) )
            InputManager.OnActionButtonPerformed += OnTap;
    }

    void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag(GameTags.Player.ToString()))
            InputManager.OnActionButtonPerformed -= OnTap;
    }
}
