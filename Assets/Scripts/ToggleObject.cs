using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ToggleObject : MonoBehaviour {

    public GameObject player;

    [SerializeField]
    private float max_distance;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private Transform floor;

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
    }

    public void ToggleButton() {
        //Debug.Log(playerDistance());
        if(playerDistance() < max_distance){
            open = !open;
        }
    }

    public float playerDistance() {
        distance = Mathf.Sqrt(Mathf.Abs(player.transform.position.x-transform.position.x)+Mathf.Abs(player.transform.position.y-transform.position.y)+Mathf.Abs(player.transform.position.z-transform.position.z));
        return distance;
    }
}
