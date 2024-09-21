using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Collision col;
    [HideInInspector]
    private Rigidbody2D rb;
    private Animation anim;

    [Header("MovementInput")]
    private float xInput;
    private float yInput;

    [Header("PlayerStats")]
    public float walkSpeed = 10f;
    public float jumpPower = 5f;
    //public float slideSpeed = 5f;
    //public float wallJumpLerp = 10f;
    public float dashSpeed = 20f;

    [Header("Coyote-Time")]
    public float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    [Header("Boolean")]
    private bool isFacingRight = true;
    public bool Movable;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;
    private bool groundTouch;
    private bool hasDashed;
    //public int side = 1;

    void Start()
    {
        col = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(xInput, yInput);

        //Jump
        if (Input.GetButtonDown("Jump") && col.onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        //CoyoteTime
        if (col.onGround)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        Walk(direction);
        Flip();
    }

    //Walk
    private void Walk(Vector2 direction)
    {
        rb.velocity = (new Vector2(direction.x * walkSpeed, rb.velocity.y));
    }

    //Flip Character when changing direction
    private void Flip()
    {
        if (isFacingRight && xInput < 0f || !isFacingRight && xInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }



}
