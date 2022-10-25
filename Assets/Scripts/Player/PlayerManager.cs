using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{

    public static PlayerManager instance;
    [SerializeField] Rigidbody playerRb;
    [SerializeField] MovementManager movementManager;
    [SerializeField] AnimationManager animationManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] GameObject meshObject;
    [SerializeField] GameObject upHeadPoint;

    public Rigidbody PlayerRb { get => playerRb; }
    public MovementManager MovementManager { get => movementManager; }
    public AnimationManager AnimationManager { get => animationManager; }
    public InputManager InputManager { get => inputManager; }
    public GameObject MeshObject { get => meshObject; }
    public GameObject UpHeadPoint { get => upHeadPoint; }
    public bool IsOccupied { get; set; }

    void Awake() {
        SingletonPattern();
    }

    void SingletonPattern(){
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
}
