using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GiantPot : InteractionBehavior{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material baseMaterial;
    [SerializeField] float launchForce;
    [SerializeField] float timeBetweenLaunch = 0.2f;
    [SerializeField] GameObject launchPoint;
    AudioManager AudioManager { get => AudioManager.instance; }
    Queue<GameObject> potions = new Queue<GameObject>();
    bool isOccupied = false;   

    protected override async void ActionBehavior(){
        const int MILLISECONDS_CONVERSION = 1000;
        if (!isOccupied){
            int potionCount = potions.Count;
            if(potionCount > 0){
                for (int i = 0; i < potionCount; i++){
                    isOccupied = true;
                    var potion = potions.Dequeue();
                    await Task.Delay((int)(timeBetweenLaunch * MILLISECONDS_CONVERSION));
                    LaunchItem(potion);
                }
                meshRenderer.material = baseMaterial;
            }else{
                AudioManager.PlayAudio(SoundList.InteractionFail);
            }
        }
        
    }

    void StorePotion(GameObject potion) {
        potions.Enqueue(potion);
        potion.gameObject.SetActive(false);
    }

    void ChangeMaterial(GameObject obj){
        var objMeshRender = obj.GetComponent<MeshRenderer>();
        meshRenderer.material = objMeshRender.material;
    }

    void LaunchItem(GameObject item){
        const float FORWARD_LAUNCH_FORCE = 4;
        item.SetActive(true);
        Rigidbody itemRb = item.GetComponent<Rigidbody>();
        item.transform.position = launchPoint.transform.position;
        var direction = (transform.up + (transform.forward * FORWARD_LAUNCH_FORCE)).normalized;
        itemRb.AddForce(direction * launchForce, ForceMode.Impulse);
        isOccupied = false;
        
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag(GameTags.Potion.ToString())){
            StorePotion(other.gameObject);
            ChangeMaterial(other.gameObject);
            AudioManager.PlayAudio(SoundList.InteractionOK);
        }else if(other.gameObject.CompareTag(GameTags.Junk.ToString())){
            LaunchItem(other.gameObject);
            AudioManager.PlayAudio(SoundList.InteractionFail);
        }
    }
}
