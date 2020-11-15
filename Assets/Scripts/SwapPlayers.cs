using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapPlayers : MonoBehaviour {

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Transform model;
	
	private kill_player[] kp;
	private GameObject[] spawn_points;
    // Start is called before the first frame update
	public void Start() {
		kp = GameObject.FindObjectsOfType<kill_player>();
		spawn_points = new GameObject[kp.Length];
		for (int i=0;i<kp.Length;i++){
			spawn_points[i] = kp[i].spawn_point;
		}
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("e") & player.GetComponent<Player_Movement>().jumpable) {
			GameObject temp;
			for (int i=0;i<kp.Length;i++){
				temp = spawn_points[i];
				spawn_points[i] = kp[i].spawn_point;
				kp[i].spawn_point = temp;
			}
			
            Vector3 pos1 = player.position;
            Vector3 pos2 = model.position;

            player.position = pos2;
            model.position = pos1;
        }
    }
}
