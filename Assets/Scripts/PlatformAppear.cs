using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAppear : IsActive
{
    private bool alreadyAppeared;
    // Start is called before the first frame update
    void Start()
    {
        alreadyAppeared = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (state) {
           Debug.Log("Appear");

            transform.GetComponent<Renderer>().enabled = true;
            transform.GetComponent<Collider>().enabled = true;
            alreadyAppeared = true;
        }
        else {

            if (!alreadyAppeared) {
                transform.GetComponent<Renderer>().enabled = false;
                transform.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
