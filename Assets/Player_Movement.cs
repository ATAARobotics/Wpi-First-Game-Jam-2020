using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    
    public Rigidbody rb;
    public PhysicMaterial pm;
    public float speed = 1.0f;
    public float crouch_speed_multiplier = 0.3f;
    public Vector3 crouch_scale = new Vector3(0.0f,-0.5f,0.0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w")){
	    rb.AddForce(transform.forward*speed);
        }
        if(Input.GetKey("s")){
	    rb.AddForce(transform.forward*speed*(-1.0f));
        }
        if(Input.GetKey("a")){
	    rb.AddForce(transform.right*speed*(-1.0f));
        }
        if(Input.GetKey("d")){
	    rb.AddForce(transform.right*speed);
        }
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            pm.dynamicFriction = 0.05f;
            speed = speed*crouch_speed_multiplier;
            transform.localScale += crouch_scale;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            pm.dynamicFriction = 0.6f;
            speed = speed/crouch_speed_multiplier;
            transform.localScale -= crouch_scale;
        }
    }
}
