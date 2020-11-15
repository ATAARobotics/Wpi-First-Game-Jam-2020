using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_box : MonoBehaviour
{
    public GameObject death_box;
    public GameObject spawn;
    private kill_player kp;

    // Start is called before the first frame update
    void Start()
    {
        kp = death_box.GetComponent<kill_player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            kp.spawn_point = spawn;

        }
    }
}
