using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create variable
    public CharacterController cc;
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = 20f;
    private Vector3 moveDir;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        // calculate direction movement

        // Allow to take/test all horizontal axes, so left or right wither it's a joysticks, keybords ... All input possible associate to horizontal
        moveDir = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDir.y, Input.GetAxis("Vertical") * moveSpeed);

        // Check espace && is on the floor
        //does't work, player or code need to get updated
        if (Input.GetButtonDown("Jump") && cc.isGrounded)
        {
            // Jump
            moveDir.y = jumpForce;
        }

        if (Input.GetButtonDown("Jump"))
        {
            // Jump
            moveDir.y = jumpForce;
        }

        // Gravity
        moveDir.y -= gravity * Time.deltaTime;

        // If moving (if movement != 0)
        if(moveDir.x != 0 || moveDir.z != 0)
        {
            // Tunr player to right direction (make fluid transition)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(moveDir.x, 0, moveDir.z)), 0.15f);
        }
        // Deplacement
        cc.Move(moveDir * Time.deltaTime);
    }
}
