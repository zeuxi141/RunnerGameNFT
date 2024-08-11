using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float gravity;
    public Vector2 velocity;
    public float maxVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float jumpVelocity = 20;
    public float groundHeight = 10;
    public bool isGrounded = false;


 
    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;


    public float jumpGroundThreshold = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);

        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        
        if(!isGrounded)
        {
            if(isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }


            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;                
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if(hit2D.collider != null)
            {
               Ground ground = hit2D.collider.GetComponent<Ground>();
                if(ground != null)
                {
                    pos.y = groundHeight;
                    isGrounded = true;
                }
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        }

        distance += velocity.x * Time.fixedDeltaTime;

        if(isGrounded)
        {
            float velocityRatio = velocity.x / maxVelocity;
            acceleration = maxAcceleration * ( 1 - velocityRatio );

            velocity.x = acceleration * Time.fixedDeltaTime;
            if(velocity.x >= maxVelocity)
            {
                velocity.x = maxVelocity;
            }


            Vector2 rayOrigin = new Vector2(pos.x - 0.5f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);
            if (hit2D.collider == null)
            {
                isGrounded = false;                                    
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }


        transform.position = pos;
    }
}
