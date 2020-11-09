using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ToggleObject : MonoBehaviour {

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private Transform floor;


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
        open = !open;
        Debug.Log("Hit!");
    }
}
