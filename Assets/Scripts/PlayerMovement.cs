using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 25f;
    Vector2 moveInput;
    Animator myAnimator;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myCapsuleCollider;
    float gravityScaleAtStart;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        ClimbLadder();
        Run();
        FlipSprite();

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {

        LayerMask GroundLayer = LayerMask.GetMask("Ground");
        if (value.isPressed && myCapsuleCollider.IsTouchingLayers(GroundLayer))
        {
            myRigidbody.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void ClimbLadder()
    {
        LayerMask ClimbingLayer = LayerMask.GetMask("Climbing");
        bool hasVerticalSpeed = Math.Abs(myRigidbody.linearVelocity.y) > Mathf.Epsilon;
        if (myCapsuleCollider.IsTouchingLayers(ClimbingLayer))
        {
            myRigidbody.linearVelocity = new Vector2(myRigidbody.linearVelocity.x, runSpeed * moveInput.y);
            myRigidbody.gravityScale = 0f;
            myAnimator.SetBool("isClimbing", hasVerticalSpeed); //moveInput.y değerine göre de yaapbilirsin
        }
        else
        {
            myRigidbody.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.linearVelocity.y);
        myRigidbody.linearVelocity = playerVelocity;
        bool hasHorizontalSpeed = Math.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
        //veya şöyle yapabilirsin: myAnimator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Math.Abs(myRigidbody.linearVelocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.linearVelocity.x), 1f);

    }
}
