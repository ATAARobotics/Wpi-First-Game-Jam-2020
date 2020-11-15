using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPlayers : MonoBehaviour {

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform model;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("e") & player.GetComponent<Player_Movement>().jumpable) {
            Vector3 pos1 = player.position;
            Vector3 pos2 = model.position;

            player.position = pos2;
            model.position = pos1;
        }
    }
}
