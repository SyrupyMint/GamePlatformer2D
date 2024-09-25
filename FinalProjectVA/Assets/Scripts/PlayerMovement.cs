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
    private TrailRenderer tr;
    private float xInput;
    private float yInput;

    [Header("PlayerStats")]
    public float walkSpeed = 10f;
    public float jumpPower = 5f;
    public float slideSpeed = 5f;
    public float wallJumpLerp = 10f;

    [Header("Coyote-Time")]
    public float _coyoteTime = 0.2f;
    public float _coyoteTimeCounter;

    [Header("Bool")]
    [SerializeField] private bool isFacingRight = true;
    public bool _canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public int side = 1;

    [Header("Dashing")]
    [SerializeField] private float _dashingVel = 14f;
    [SerializeField] private float _dashingDelay = 0.5f;
    private Vector2 _dashingDir;
    private bool _isDashing;
    private bool _canDash = true;

    void Start()
    {
        col = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    void Update()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var jumpInputRel = Input.GetButtonUp("Jump");
        var dashInput = Input.GetButtonDown("Dash");
        var wallGrabHold = Input.GetButton("Grab");
        var wallGrabRel = Input.GetButtonUp("Grab");

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(xInput, yInput);

        if (col.onWall && wallGrabHold) { wallGrab = true; wallSlide = false; }

        if(!col.onWall || wallGrabRel) { wallGrab = false; wallSlide = false; }
        
        if (wallGrab)
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            float spdModifier = yInput > 0 ? 0.5f : 1;
            rb.velocity = new Vector2(rb.velocity.x, y * (walkSpeed * spdModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if(col.onWall && !col.onGround)
        {
            if (!wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (col.onWall && col.onGround)
        {
            wallSlide = false;
        }

        if (col.onGround) { wallJumped = false; }

        if (jumpInput)
        {
            if (_coyoteTimeCounter > 0f)
            {
                Jump(Vector2.up);
                _coyoteTimeCounter = 0f;
            }
            if (col.onWall && !col.onGround)
                WallJump();

        }

        if (dashInput && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            tr.emitting = true;
            _dashingDir = new Vector2(xInput, yInput);
            if (_dashingDir == Vector2.zero)
            {
                _dashingDir = new Vector2(transform.localScale.x, 0);
            }
            //Dash Delay
            StartCoroutine(StopDashing());
        }

        if(_isDashing)
        {
            rb.velocity = _dashingDir.normalized * _dashingVel;
            return;
        }

        //CoyoteTime
        if (col.onGround)
        {
            _coyoteTimeCounter = _coyoteTime;
            _canDash = true; //reset the dash once the character is on ground after jump
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        if (wallGrab) { return; }

        Walk(direction);
        Flip();
    }

    private void WallJump()
    {
        StartCoroutine(DisableMovement(0.1f));
        Vector2 wallDir = col.onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + wallDir);
        wallJumped = true;
    }

    private void WallSlide()
    {
        bool pushingWall = false;
        if ((rb.velocity.x > 0 && col.onRightWall) || (rb.velocity.x < 0 && col.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashingDelay);
        tr.emitting = false;
        _isDashing = false;
    }

    //Walk
    private void Walk(Vector2 direction)
    {
        if (!_canMove) { return; }
        if (wallGrab) { return; }
        if(!wallJumped)
        {
            rb.velocity = (new Vector2(direction.x * walkSpeed, rb.velocity.y));
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(direction.x * walkSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    //Jump
    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;
    }

    IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }

    //Flip Character when changing direction
    private void Flip()
    {
        if (isFacingRight && xInput < 0f || !isFacingRight && xInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }



}
