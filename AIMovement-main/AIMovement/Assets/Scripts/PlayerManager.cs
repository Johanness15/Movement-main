// The basic movement part was done by Kevin
// The advanced movement vas done by Johannes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour {
    private Rigidbody rb;
   [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float JumpCooldown;
    public float airMultiplier;
    bool readyToJump;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    [Header]("Keybinds")
    public KeyCode jumpKey = KeyCode.Space;
    private float vertical;
    private float horizontal;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;

    
    void Start() {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
     
    private void update() {
        // ground check 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
         
        MyInput();
        SpeedControl();

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        
        

    }

    private void FixedUpdate() {
        
        MovePlayer();

    } 

    private void MyInput() {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        

        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded) {

            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCooldown);
        }
    }
    private void MovePlayer() {

      // calculate movement direction
      moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

      rb.AddForce(moveDirection.normalized * 10f, ForceMode.Force);

      // on ground
      if(grounded)
          rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
      // in air
      else if(!grounded)
          rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

   
    private void SpeedControl() { 
    
       Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       // limit velocity if needed
       if(flatVel.magnitude > moveSpeed)
       {

          Vector3 limitedVel = flatVel.normalized * moveSpeed;
          rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); 
        }




    }

    private void Jump() {

        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    
    private void ResetJump(){


       readyToJump = true;


    }







}



