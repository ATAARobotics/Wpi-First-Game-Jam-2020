using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeToEnd : IsActive
{
    private bool playerHere = false;
    private bool stillHere = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (state && playerHere) {
            SceneManager.LoadScene("End");

        }
    }

    private void OnCollisionStay(Collision collision) {

        if (collision.transform.tag == "Player") {
            playerHere = true;
            //Debug.Log("Hit 1!");
        }

    }

    private void OnCollisionExit(Collision collision) {
        if (collision.transform.tag == "Player") {
            playerHere = false;
        }
    }
}
