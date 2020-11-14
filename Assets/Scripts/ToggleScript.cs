using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Jobs;

public class ToggleScript : MonoBehaviour {

    public GameObject player;

    [SerializeField]
    private float max_distance;


    [SerializeField]
    private Transform[] targeted;

<<<<<<< HEAD:Assets/Scripts/ToggleObject.cs
    private float distance;
    // Update is called once per frame
    void Update() {
        if (open) {
            floor.GetComponent<Renderer>().enabled = false;
            floor.GetComponent<Collider>().enabled = false;
        } else {
            floor.GetComponent<Renderer>().enabled = true;
            floor.GetComponent<Collider>().enabled = true;
        }
        if(playerDistance() < max_distance&&Input.GetButton("Fire1")){
            open = !open;
        }
    }
=======
    private bool toggle;
>>>>>>> LevelDesign:Assets/Scripts/ToggleScript.cs

    private float distance;
    public void ToggleButton() {
        //Debug.Log(playerDistance());
<<<<<<< HEAD:Assets/Scripts/ToggleObject.cs
        
=======
        if(playerDistance() < max_distance){
           // Debug.Log("toggle");
            toggle = !toggle;
            for (int i = 0; i < targeted.Length; i++) {
               // Debug.Log(i);
                targeted[i].transform.GetComponent<IsActive>().Toggle(toggle);
            }
        }
>>>>>>> LevelDesign:Assets/Scripts/ToggleScript.cs
    }

    public float playerDistance() {
        distance = Mathf.Sqrt(Mathf.Abs(player.transform.position.x-transform.position.x)+Mathf.Abs(player.transform.position.y-transform.position.y)+Mathf.Abs(player.transform.position.z-transform.position.z));
        return distance;
    }
}
