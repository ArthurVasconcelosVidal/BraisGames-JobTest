using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchInteraction : InteractionBehavior{
    bool state = true;
    [SerializeField] GameObject torchLight;
    [SerializeField] GameObject torchParticles;
    protected override void ActionBehavior(){
        state = !state;
        torchLight.SetActive(state);
        torchParticles.SetActive(state);
    }
}
