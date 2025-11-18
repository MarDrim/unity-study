using System;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 velocity;
    Vector2 horizontal;
    float vertical;
    float walkSpeed = 5f; // Player speed
    float jumpHeight = 5f; // Player jump height

    Rigidbody2D rb; // Player rigidbody
    bool facingRight = true; // Player orientation

    bool onGround;
    bool onWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector2.zero;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called once per 50 frames
    void FixedUpdate()
    {
        // Move horizontally
        velocity.x = horizontal.x * walkSpeed;

        // Apply velocity
        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocityY);

        // Jump
        if (vertical > 0 && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical);
            onGround = false; // prevent double jumps
        }
    }

    public void Walk(InputAction.CallbackContext walk)
    {
        horizontal = walk.ReadValue<Vector2>();
        horizontal.Normalize();
        horizontal.y = 0;

        // Flip sprite
        if (horizontal.x > 0 && !facingRight) Flip();
        if (horizontal.x < 0 && facingRight) Flip();
    }

    public void Jump(InputAction.CallbackContext jump)
    {
        if (jump.performed)
        {
            vertical = 8;
        }
        else if (jump.canceled)
        {
            vertical = 0;
        }
    }

    public void Attack(InputAction.CallbackContext attack)
    {

    }

    // Flip is a helper method for move that flips the sprite depending on orientation.
    public void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        else onGround = false;

        if (collision.gameObject.CompareTag("Wall"))
        {
            onWall = true;
        }
        else onWall = false;
    }
}
