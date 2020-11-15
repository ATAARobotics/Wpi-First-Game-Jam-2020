﻿using System.Collections;
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

    private bool toggle;

    private float distance;
	
	
    public void ToggleButton() {
        //Debug.Log(playerDistance());
        if(playerDistance() < max_distance){
           // Debug.Log("toggle");
            toggle = !toggle;
            for (int i = 0; i < targeted.Length; i++) {
               // Debug.Log(i);
                targeted[i].transform.GetComponent<IsActive>().Toggle(toggle);
            }
        }
    }

    public float playerDistance() {
        distance = Mathf.Sqrt(Mathf.Abs(player.transform.position.x-transform.position.x)+Mathf.Abs(player.transform.position.y-transform.position.y)+Mathf.Abs(player.transform.position.z-transform.position.z));
        return distance;
    }
}
