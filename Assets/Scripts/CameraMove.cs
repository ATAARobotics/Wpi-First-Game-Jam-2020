using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float mouse_sensitivity = 0.05f;
    private float rotation_z = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        Camera cm = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        rotation_z+=Mathf.Clamp((-Input.GetAxis("Mouse Y"))*mouse_sensitivity,-5.0f,5.0f);
        transform.localRotation = Quaternion.Euler(rotation_z, 0, 0);
    }
}
