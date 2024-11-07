using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;
    public float speed = 12f;// player speed and how fast they move
    public float gravity = -9.81f; // gravity to affect player jump
    public float jHeight = 3f; // how high the player can jump

    public Transform groundCheck; // an empty object the checks if the player is on the ground
    public float groundDistance = 0.1f; // how far the player has to be from the ground to be checked
    public LayerMask Ground; // check if the object is the Ground 

    Vector3 velocity; 
    bool isGrounded; // boolean to check if the player is on the ground




    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, Ground); // checks if its on the ground 
        
        if (isGrounded && velocity.y < 0) // resets the velocity for jumping
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal"); // input from ws
        float z = Input.GetAxis("Vertical"); // input from ad

        Vector3 move = transform.right * x + transform.forward * z; // turn for the inputs into a vector 3 
        controller.Move(move * speed * Time.deltaTime); // move along the x or z axis

        if (Input.GetButtonDown("Jump") && isGrounded) // if player presses space bar and is on the ground
        {
            velocity.y = Mathf.Sqrt(jHeight * -2f * gravity); // jump height times -2 times gravity = how high player jumps
        }

        velocity.y += gravity * Time.deltaTime; // changes the velocity when the player is falling

        controller.Move(velocity * Time.deltaTime); // applies the change in velocity



    }
}
