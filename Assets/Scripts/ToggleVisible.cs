using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisible : IsActive
{
    private bool alreadyDisappeared;
    // Start is called before the first frame update
    void Start()
    {
        alreadyDisappeared = false;
    }

    // Update is called once per frame
    void Update() {
        if (state) {
            //Debug.Log("dissapear");
            
                transform.GetComponent<Renderer>().enabled = false;
                transform.GetComponent<Collider>().enabled = false;
                alreadyDisappeared = true;
            
        } else {

            if (!alreadyDisappeared)
            {
                transform.GetComponent<Renderer>().enabled = true;
                transform.GetComponent<Collider>().enabled = true;
            }
        }
    }

}
