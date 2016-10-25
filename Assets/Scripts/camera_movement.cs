using UnityEngine;

public class camera_movement : MonoBehaviour
{

    bool pressed = false;
    public float swing_angle;
    public float elevate_angle;

    // Use this for initialization
    void Start()
    {
        swing_angle = -50.0f;
        elevate_angle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Input.GetAxis("Vertical") * transform.forward;
            //transform.localPosition += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Input.GetAxis("Vertical") * transform.forward;
            //transform.localPosition += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Input.GetAxis("Horizontal") * transform.right;
            //transform.localPosition += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Input.GetAxis("Horizontal") * transform.right;
            //transform.localPosition += new Vector3(1, 0, 0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            pressed = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            pressed = false;
        }
        if (pressed)
        {
            swing_angle += (3 * Input.GetAxis("Mouse Y"));
            elevate_angle += (3 * Input.GetAxis("Mouse X"));
            transform.localRotation = Quaternion.identity;
            Quaternion newRot = Quaternion.Euler(-swing_angle, elevate_angle, 0);
            transform.localRotation = newRot;
        }
    }
}
