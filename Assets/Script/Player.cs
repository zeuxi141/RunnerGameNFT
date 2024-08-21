using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Animator anim;
    [SerializeField] private int facingDir = 1;
    [SerializeField] private bool facingRight = true;

    [Header("Run speed info")]
    [SerializeField] private float acceleration = 10f;
    private float maxAcceleration = 10f;
    [SerializeField] public float currentSpeed;
    private float maxSpeed = 100;
    public float moveSpeed;
    [SerializeField] public float distance = 0;

    [Header("Jump Info")]
    [SerializeField] public float jumpForce;
    [SerializeField] private bool isHoldingJump = false;
    [SerializeField] public float maxholdJumpTime = 0.4f;
    [SerializeField] private float holdJumpTimer = 0.0f;

    [Header("Gameplay Info")]
    public bool isStart = false;
    public bool isDead = false;



    private float xInput = 0.1f;
    private bool isGrounded;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] LayerMask deadthArea;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart && isDead == false)
        {

            Movement();

            CheckInput();

            CollisionCheck();

            AnimatorControllers();

            FlipController();
        }
        else
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isGrounded", true);
        }
    }

    private void FixedUpdate()
    {
        if (isHoldingJump && holdJumpTimer < maxholdJumpTime)
        {
            rb.velocity = new Vector2(rb.velocity.x, 9);
            holdJumpTimer += Time.fixedDeltaTime;
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isHoldingJump = true;
        holdJumpTimer = 0f; 
    }


    private void CollisionCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isDead = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, deadthArea);
    }

    private void CheckInput()
    {
        //xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();     
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    private void Movement()
    {
        if (isGrounded)
        {
            holdJumpTimer = 0;
        }
        if (xInput != 0)
        {
            float speedRatio = currentSpeed / maxSpeed;
            acceleration = maxAcceleration * ( 1- speedRatio );

            currentSpeed += acceleration * Time.fixedDeltaTime; 
            if(currentSpeed >= maxSpeed)
            {
                currentSpeed = maxSpeed;
            
            }
            distance += currentSpeed * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = moveSpeed; 
        }


        //rb.velocity = new Vector2(xInput * currentSpeed, rb.velocity.y);
    }


    private void AnimatorControllers()
    {
        //bool isMoving = rb.velocity.x != 0;
        bool isMoving = true;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if(rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }else if (rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
