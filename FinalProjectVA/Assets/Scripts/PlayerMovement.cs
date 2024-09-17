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
    private float walkSpeed = 10f;
    private float jumpPower = 5f;

    [Header("Boolean")]
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(xInput, yInput);

        //if(Input.GetButtonDown("Jump") && col.onGround)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        //}

        //if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        //{
        //    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        //}

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
