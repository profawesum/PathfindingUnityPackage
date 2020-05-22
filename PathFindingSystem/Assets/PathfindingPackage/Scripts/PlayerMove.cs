using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{

    //reference to the character controller
    CharacterController characterController;

    [Header ("Player movement variables")]
    //various movement 
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    //move direction vector
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        //get the character controller
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //checks to see if the player is grounded
        if (characterController.isGrounded)
        {
            //gets the move direction
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;

            //checks to see if the player has jumped
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

       //apply gravity to the player as it is not using a rigidbody
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);
    }
}