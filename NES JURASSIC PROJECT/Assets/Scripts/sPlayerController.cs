﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sPlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    private float lastHMove = 0.0f;
    private float lastVMove = 0.0f;

    Vector2 movement;

    private const string HORIZONTAL = "Horizontal";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        // input
        movement.x = Input.GetAxisRaw(HORIZONTAL);
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        //animator.SetBool("Moving", movement.x == 0 && movement.y == 0);

        if (movement.x == 0 && movement.y == 0) {
            animator.SetBool("Moving", false);

        } else {
            animator.SetBool("Moving", true);

        }
    }
    void FixedUpdate()
    {
        // movement
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }
}
