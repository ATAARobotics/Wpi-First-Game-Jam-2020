using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour {

    [SerializeField]
    KeyCode restartButton;
    
    // Update is called once per frame
    void Update() {
        if (Input.GetKey(restartButton)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
