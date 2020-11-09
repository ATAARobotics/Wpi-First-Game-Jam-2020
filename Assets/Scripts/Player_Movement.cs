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
    public float base_friction = 10.0f;
    public float side_speed_scale = 0.5f;
    public float in_air_speed_scale = 0.4f;
    public float jump_speed_loss = 0.9f;
    public float crouch_height_multiplier = 1.0f;
    public float crouch_height_penalty = 1.0f;
    
    private float speed = 0.0f;
    private float rotation_x = 0.0f;
    private Vector3 velocity = new Vector3(0.0f,0.0f,0.0f);
    private Vector2 velocity2D = new Vector2(0.0f,0.0f);
    private bool jumpable = false;
    private int objects_contacting = 0;
    public float velocity_magnitude = 0.0f;
    public GameObject player_model;
    private Animation am;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        am = player_model.GetComponent<Animation>();
        Cursor.lockState = CursorLockMode.Locked;
        speed = base_speed;
        am["Walking"].speed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        velocity_magnitude = velocity2D.magnitude;
        velocity = new Vector3(rb.velocity.x,rb.velocity.y,rb.velocity.z);
        if((Input.GetKey("w")||Input.GetKey("a")||Input.GetKey("s")||Input.GetKey("d"))&&(!Input.GetKey(KeyCode.LeftControl))&&jumpable){   
            velocity = new Vector3(0.0f,rb.velocity.y,0.0f);
        }
        if(Input.GetKey("w")&&(jumpable)){
	    velocity += (transform.forward*speed);
            
        }
        if(Input.GetKey("s")&&jumpable){
	    velocity += transform.forward*speed*(-1.0f);
        }
        if(Input.GetKey("a")&&jumpable){
	    velocity += transform.right*speed*(-1.0f)*side_speed_scale;
        }
        if(Input.GetKey("d")&&jumpable){
	    velocity += transform.right*speed*side_speed_scale;
        }
        if(!Input.GetKey("w")&&!Input.GetKey("a")&&!Input.GetKey("s")&&!Input.GetKey("d")){
            am.Play("Idle");
        }
        if(Input.GetKey("w")||Input.GetKey("a")||Input.GetKey("s")||Input.GetKey("d")){
            am.Play("Walking");
        }
        if(Input.GetKey("w")&&!(jumpable)){
	    velocity += transform.forward*speed*in_air_speed_scale;
            velocity2D = new Vector2(velocity.x,velocity.z);
            if (velocity2D.magnitude != 0){
                velocity2D = velocity2D*(velocity_magnitude/velocity2D.magnitude);
            }
            if(velocity_magnitude > -0.01 && velocity_magnitude < 0.01){
                velocity += transform.forward*speed*in_air_speed_scale;
            }else{
            velocity = new Vector3(velocity2D.x,velocity.y,velocity2D.y);
            }
            am.Play("Idle");
        }
        if(Input.GetKey("s")&&!(jumpable)){
	    velocity += transform.forward*speed*(-1.0f)*in_air_speed_scale;
            velocity2D = new Vector2(velocity.x,velocity.z);
            if (velocity2D.magnitude != 0){
                velocity2D = velocity2D*(velocity_magnitude/velocity2D.magnitude);
            }
            velocity = new Vector3(velocity2D.x,velocity.y,velocity2D.y);
            am.Play("Idle");
        }
        if(Input.GetKey("a")&&!(jumpable)){
	    velocity += transform.right*speed*(-1.0f)*side_speed_scale*in_air_speed_scale;
            velocity2D = new Vector2(velocity.x,velocity.z);
            if (velocity2D.magnitude != 0){
                velocity2D = velocity2D*(velocity_magnitude/velocity2D.magnitude);
            }
            velocity = new Vector3(velocity2D.x,velocity.y,velocity2D.y);
            am.Play("Idle");
        }
        if(Input.GetKey("d")&&!(jumpable)){
	    velocity += transform.right*speed*side_speed_scale*in_air_speed_scale;
            velocity2D = new Vector2(velocity.x,velocity.z);
            if (velocity2D.magnitude != 0){
                velocity2D = velocity2D*(velocity_magnitude/velocity2D.magnitude);
            }
            velocity = new Vector3(velocity2D.x,velocity.y,velocity2D.y);
            am.Play("Idle");
        }
        if(Input.GetKey("space")&&(jumpable)){
	    velocity += transform.up*jump_speed;
            velocity.x = velocity.x*jump_speed_loss;
            velocity.z = velocity.z*jump_speed_loss;
            objects_contacting = 0;
            am.Play("Idle");
        }
        velocity2D = new Vector2(velocity.x,velocity.z);
        rb.velocity = velocity;
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            pm.dynamicFriction = 0.05f;
            speed = 0.0f;
            transform.localScale += crouch_scale;
            rb.velocity = rb.velocity*crouch_speed_multiplier;
            velocity.y = velocity.y*crouch_height_multiplier-crouch_height_penalty;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y*crouch_height_multiplier-crouch_height_penalty,rb.velocity.z);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)&&jumpable){
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y*crouch_height_multiplier-crouch_height_penalty,rb.velocity.z);
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            pm.dynamicFriction = base_friction;
            speed = base_speed;
            transform.localScale -= crouch_scale;
        }
        rotation_x+=Mathf.Clamp((Input.GetAxis("Mouse X"))*mouse_sensitivity,-5.0f,5.0f);
        transform.localRotation = Quaternion.Euler(0, rotation_x, 0);
        if (objects_contacting > 0){
            jumpable = true;
        }else{
            jumpable = false;
        }
    }

    void OnTriggerEnter(Collider col){
        objects_contacting = objects_contacting+1;
    }
    void OnTriggerExit(Collider col){
        objects_contacting = objects_contacting-1;
        if (objects_contacting<0){objects_contacting = 0;}
    }
}
