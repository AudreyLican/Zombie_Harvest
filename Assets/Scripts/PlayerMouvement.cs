using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck; // check if player is grounded, to be able to jump
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    //storing player last position to know if we are moving
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Reseting the default velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z; // (right - red axis, forward - blue axis)

        //Actually moving the player
        controller.Move(move * speed * Time.deltaTime);

        //Check if player can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Going up (&& avoid player jumping when he is already in the air)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //falling down (apply gravity)
        velocity.y += gravity * Time.deltaTime;

        //Executing the jump
        controller.Move(velocity * Time.deltaTime);


        //Check is player is actually moving
        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            // if last position is not equal to the current one
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Set last poistion as currenyt positin when player stop moving
        lastPosition = gameObject.transform.position;
    }
}
