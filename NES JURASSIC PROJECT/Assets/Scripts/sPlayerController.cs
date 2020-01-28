using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;



    private Rigidbody2D rb;
    private Animator animator;

    private float lastHMove = 0.0f;
    private float lastVMove = 0.0f;

    private Vector2 movement;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";



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

    }



    void FixedUpdate()
    {
        // movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }
    
    // animate
    void anim()
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);


        if (movement.x == 0 && movement.y == 0)
        {
            animator.SetBool("Moving", false);
            animator.SetFloat("Horizontal", lastHMove);
            animator.SetFloat("Vertical", lastVMove);

        }
        else {
            lastHMove = movement.x;
            lastVMove = movement.y;
            animator.SetBool("Moving", true);

        }
    }


    
}
