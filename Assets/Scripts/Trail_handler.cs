using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail_handler : MonoBehaviour
{

    public TrailRenderer tr;
    public GameObject player;
    public Player_Movement pm;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<TrailRenderer>();
        Player_Movement pm = player.GetComponent<Player_Movement>(); 
    }

    // Update is called once per frame
    void Update()
    {
        tr.time = pm.velocity_magnitude;
    }
}
