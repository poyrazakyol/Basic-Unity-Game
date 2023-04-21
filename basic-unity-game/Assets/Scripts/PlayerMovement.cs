using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sr;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0;
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float jumpForce = 14;

    private enum MovementState { idle, running, jumping, falling};

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
        if(Input.GetButtonDown("Jump") && IsGrounded() )
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpForce);
        }

        AnimatorUpdate();
    }

    void AnimatorUpdate()
    {
        MovementState state;

        if(dirX>0)
        {
            state = MovementState.running;
            sr.flipX = false;
        }

        else if(dirX<0)
        {
            state = MovementState.running;
            sr.flipX = true;
        }

        else
        {
           state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }

        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state",(int)state);
    }

    bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

    }
}
