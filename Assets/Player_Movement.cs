using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    
    public Rigidbody rb;
    public PhysicMaterial pm;
    public float base_speed = 15.0f;
    public float crouch_speed_multiplier = 0.3f;
    public Vector3 crouch_scale = new Vector3(0.0f,-0.5f,0.0f);
    public float mouse_sensitivity = 0.05f;
    public float jump_speed = 5.0f;
    
    private float speed = 0.0f;
    private float rotation_x = 0.0f;
    private Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
        Cursor.lockState = CursorLockMode.Locked;
        speed = base_speed;    
    }

    // Update is called once per frame
    void Update()
    {
        velocity = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        if((Input.GetKey("w")||Input.GetKey("a")||Input.GetKey("s")||Input.GetKey("d"))&&(!Input.GetKey(KeyCode.LeftControl))){   
            velocity = new Vector3(0.0f,rb.velocity.y,0.0f);
        }
        if(Input.GetKey("w")){
	    velocity += transform.forward*speed;
        }
        if(Input.GetKey("s")){
	    velocity += transform.forward*speed*(-1.0f);
        }
        if(Input.GetKey("a")){
	    velocity += transform.right*speed*(-1.0f);
        }
        if(Input.GetKey("d")){
	    velocity += transform.right*speed;
        }
if(rb.velocity.y==0){
            if(Input.GetKey("space")){
	        velocity += transform.up*jump_speed;
            }
           print("aaaaaaaaaaaaaaaaaaaa");
        }


        rb.velocity = velocity;
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            pm.dynamicFriction = 0.05f;
            speed = speed*crouch_speed_multiplier;
            transform.localScale += crouch_scale;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            pm.dynamicFriction = 0.6f;
            speed = base_speed;
            transform.localScale -= crouch_scale;
        }
        rotation_x+=Mathf.Clamp((Input.GetAxis("Mouse X"))*mouse_sensitivity,-5.0f,5.0f);
        transform.localRotation = Quaternion.Euler(0, rotation_x, 0);
    }
}
