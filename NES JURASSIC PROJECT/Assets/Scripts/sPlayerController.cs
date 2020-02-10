using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private sShooting ShootingScript;



    private Rigidbody2D rb;
    private Animator animator;

    private float lastHMove = 0.0f;
    private float lastVMove = 0.0f;

    private Vector2 movement;
    private bool grounded = true;
    private bool wannaJump = false;


    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string moving = "Moving";
    private const string jump = "Jump";

    private Vector3 jumpHeight = new Vector3(0f, 20f , 0f);
    private Vector2 landPos = new Vector2(0f, 0f);





    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        

    }



    void Update()
    {


        //move input
        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);
        //jump input
        if(Input.GetKeyUp(KeyCode.X) && grounded)
        {
            wannaJump = true;
        }

        anim();

        //Shoot
        ShootingScript.ShootUpdate();

    }



    void FixedUpdate()
    {
        Movement();
        Jump();
        Invoke("Grounder", 0.1f);

    }
    


    void Movement()
    {
        // movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }

    void Jump()
    {
        
        if (wannaJump)
        {
            landPos = transform.position;
            wannaJump = false;
            grounded = false;

            rb.velocity = jumpHeight * jumpSpeed * Time.fixedDeltaTime;
            rb.gravityScale = 5;   

        }
        
    }


    void Grounder()
    {
        if(movement.y > 0)
        {
            rb.gravityScale = 0;
            grounded = true;
        }
        else if (transform.position.y <= landPos.y)
        {
            rb.gravityScale = 0;
            grounded = true;
        }
    }

    // animate
    void anim()
    {
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);


        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool(moving, false);
            animator.SetFloat(horizontal, lastHMove);
            animator.SetFloat(vertical, lastVMove);

        }
        else {
            lastHMove = movement.x;
            lastVMove = movement.y;
            animator.SetBool(moving, true);

        }
    }


    
}
