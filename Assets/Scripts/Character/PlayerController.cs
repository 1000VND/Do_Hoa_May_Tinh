using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //code di chuyển, nhảy
    private CharacterController characterController;
    private Vector3 playerVelocity;
    public float speed = 8f;

    private bool isGrounded;
    public float gravity = -25f;
    public float jumForce = 1.0f;

    private bool crouching = false;
    private float crouchTimer = 1;
    private bool lerpCrouch = false;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //check xem có trên mặt đất
        isGrounded = characterController.isGrounded;

        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p += p;
            if(crouching)
            {
                characterController.height = Mathf.Lerp(characterController.height, 1, p);
            }
            else
            {
                characterController.height = Mathf.Lerp(characterController.height, 2, p);
            }

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        Jump();
        Sprint();
        Crouch();
}

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;  
        characterController.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumForce * -2.0f * gravity);
        }
    }

    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 16.0f;
        }
        else
        {
            speed = 8.0f;
        }
    }

    public void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouching = true;
            crouchTimer = 0;
            lerpCrouch = true;
        }
        else
        {
            crouching = false;
            crouchTimer = 0;
            lerpCrouch = true;
        }
    }
}
