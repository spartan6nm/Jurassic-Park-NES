using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private sShooting ShootingScript;


    private Rigidbody2D rb;
    private Animator animator;

    private float lastHMove = 0.0f;
    private float lastVMove = 0.0f;

    private Vector2 movement;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string moving = "Moving";


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }



    void Update()
    {
        // input
        movement.x = Input.GetAxisRaw(horizontal);
        movement.y = Input.GetAxisRaw(vertical);

        anim();

        //Shoot
        ShootingScript.ShootUpdate();

    }



    void FixedUpdate()
    {
        // movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
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
