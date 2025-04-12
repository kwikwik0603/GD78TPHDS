using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    //X & Y sensitivity
    public float sensX;
    public float sensY;

    //orientation transform
    public Transform orientation;

    //x & y rotation of camera
    float xRotation;
    float yRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //locks cursor position and sets it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //get user mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //unity handleing rotation and input
        yRotation += mouseX;
        xRotation -= mouseY;

        //clamping mouse input for X axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotate cam & orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
