﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour {

    [SerializeField]
    private string buttonTag;
    [SerializeField]
    private KeyCode interactButton;

    int i;
    void OnTriggerStay(Collider other) {
        if (other.tag == buttonTag) {
            if (Input.GetKeyUp(interactButton)) {
                i = 0;
                foreach (ToggleObject button in other.GetComponents<ToggleObject>()) {
                    button.ToggleButton();
                    i++;
                }
                Debug.Log(i);
            }
        }
    }
}
