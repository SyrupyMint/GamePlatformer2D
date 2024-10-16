using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Collision col;
    [HideInInspector]
    public Rigidbody2D rb;
    private AnimationScript anim;

    [Header("PlayerStats")]
    public float walkSpeed = 10;
    public float jumpPower = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed = 20;

    [Header("Bools")]
    public bool _canMove;
    public bool _wallGrab;
    public bool _wallJumped;
    public bool _wallSlide;
    public bool _isDashing;

    [Header("Knockback")]
    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;

    public bool KnockFromRight;
    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    void Start()
    {
        col = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<AnimationScript>();
    }

    void Update()
    {
        var jumpInput = Input.GetButtonDown("Jump");
        var dashInput = Input.GetButtonDown("Dash");
        var wallGrabHold = Input.GetButton("Grab");
        var wallGrabRel = Input.GetButtonUp("Grab");

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);

        if (col.onWall && wallGrabHold && _canMove)
        {
            if (side != col.wallSide)
                anim.Flip(side * -1);
            _wallGrab = true;
            _wallSlide = false;
        }

        if (wallGrabRel || !col.onWall || !_canMove)
        {
            _wallGrab = false;
            _wallSlide = false;
        }

        if (col.onGround && !_isDashing)
        {
            _wallJumped = false;
            GetComponent<BetterJump>().enabled = true;
        }

        if (_wallGrab && !_isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float spdModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (walkSpeed * spdModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (col.onWall && !col.onGround)
        {
            if (x != 0 && !_wallGrab)
            {
                _wallSlide = true;
                WallSlide();
            }
        }

        if (!col.onWall || col.onGround)
            _wallSlide = false;

        if (jumpInput)
        {
            anim.SetTrigger("jump");

            if (col.onGround)
                Jump(Vector2.up);
            if (col.onWall && !col.onGround)
                WallJump();
        }


        if (dashInput && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
                Dash(xRaw, yRaw);
        }

        if (col.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!col.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if (KBCounter <= 0)
        {
            Walk(dir);
        }
        else
        {
            if (KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForce, KBForce);
            }
            if (KnockFromRight == false)
            {
                rb.velocity = new Vector2(KBForce, KBForce);
            }

            KBCounter -= Time.deltaTime;
        }

        if (_wallGrab || _wallSlide || !_canMove)
            return;

        if (x > 0)
        {
            side = 1;
            anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            anim.Flip(side);
        }

    }

    private void GroundTouch()
    {
        hasDashed = false;
        _isDashing = false;

        side = anim.sr.flipX ? -1 : 1;
    }

    private void Dash(float x, float y)
    {
        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        rb.gravityScale = 0;
        GetComponent<BetterJump>().enabled = false;
        _wallJumped = true;
        _isDashing = true;

        yield return new WaitForSeconds(.3f);

        rb.gravityScale = 3;
        GetComponent<BetterJump>().enabled = true;
        _wallJumped = false;
        _isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (col.onGround)
            hasDashed = false;
    }

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpPower;
    }

    private void WallJump()
    {
        if ((side == 1 && col.onRightWall) || side == -1 && !col.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = col.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f));

        _wallJumped = true;
    }

    private void WallSlide()
    {
        if (col.wallSide != side)
            anim.Flip(side * -1);

        if (!_canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && col.onRightWall) || (rb.velocity.x < 0 && col.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Walk(Vector2 dir)
    {
        if (!_canMove)
            return;

        if (_wallGrab)
            return;

        if (!_wallJumped)
        {
            rb.velocity = new Vector2(dir.x * walkSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * walkSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }
}
