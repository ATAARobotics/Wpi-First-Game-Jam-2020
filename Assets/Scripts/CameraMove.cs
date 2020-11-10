using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float mouse_sensitivity = 0.05f;
    private float rotation_z = 0.0f;
    private Camera cm;


    // Start is called before the first frame update
    void Start()
    {
        Camera cm = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        rotation_z+=Mathf.Clamp((-Input.GetAxis("Mouse Y"))*mouse_sensitivity,-5.0f,5.0f);
        rotation_z = Mathf.Clamp(rotation_z,-90.0f,90.0f);
        transform.localRotation = Quaternion.Euler(rotation_z, 0, 0);

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Button") {
                    ToggleScript button = hit.transform.GetComponent<ToggleScript>();
                    button.ToggleButton();
                    Debug.Log(button);
                }
            }
        }
    }
}
