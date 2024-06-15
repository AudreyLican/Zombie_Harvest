using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMouvement : MonoBehaviour
{
    public float mouseSensitivity = 300f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f; //must be negative because when we look up its goes to a negative value
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotation around the x axis (Look up and down)
        xRotation -= mouseY;

        //Clamp the rotation (if mouse mouving to much on top or buttom, we want to block it)
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //Rotation around the y axis (Look left and right)
        yRotation += mouseX;

        //Apply rotation to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
