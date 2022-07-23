using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using Sirenix.OdinInspector;
using TMPro;
using TigerForge;

public class Player : MonoBehaviour
{
    [Header("Input")]
    [HideInInspector]
    public PlayerInput playerInput;
    private Vector3 inputDirection;
    private bool currentInput = false;

    [Range(0, 50)]
    public float movespeed = 15f;
    [Range(0, 50)]
    public float jumpspeed = 10f;
    [Range(0, 20)]
    public float velocityLimit = 100f;
    private bool isGrounded = false;
    private bool usedDoubleJump = false;

    [Range(0, 1)]
    public float gravitySlideScale = 0.7f;
    [Range(0, 10)]
    public float slideBoostStrength = 6f;
    private float _basegravity;
    private bool isWallSliding = false;
    enum Direction { none, left, right };
    private Direction slideDir;

    [Title("Debug")]
    [SceneObjectsOnly]
    public TMP_Text debugjumps;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _basegravity = rb.gravityScale;
        debugjumps.gameObject.SetActive(Static.debugMode);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ground => grounded, reset double jump
        if (collision.gameObject.layer == 3 && collision.gameObject.tag == "TerrainGround")
        {
            isGrounded = true;
            usedDoubleJump = false;
        }
        //Side wall => start wall sliding, set direction of slide
        else if (collision.gameObject.layer == 3 && collision.gameObject.CompareTag("TerrainWall"))
        {
            if(Static.debugMode)
            {
                //Draw a blue ray on slide hit
                Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.blue, 1, false);
            }
            //Start sliding
            isWallSliding = true;
            usedDoubleJump = false;

            //Compare collision and own position to find direction of collision
            slideDir =  ((collision.transform.position - transform.position).normalized).x >= 0 ? Direction.right : Direction.left;

            //Set gravity to be lower when sliding
            rb.gravityScale = _basegravity * gravitySlideScale;
        }
    }

    private string calcJumpDebug(bool isGrounded, bool usedDoubleJump, Direction slidedir)
    {
        if(isGrounded)
        {
            return $"Jumps: Grounded(2)\n Sliding: {slidedir}";
        }
        else if (!usedDoubleJump)
        {
            return $"Jumps: Air(1)\n Sliding: {slidedir}";
        }
        return $"Jumps: Air(0)\n Sliding: {slidedir}";
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Leave ground
        if (collision.gameObject.layer == 3 && collision.gameObject.CompareTag("TerrainGround"))
        {
            isGrounded = false;
            
        }
        //Leave wall
        else if (collision.gameObject.layer == 3 && collision.gameObject.CompareTag("TerrainWall"))
        {
            isWallSliding = false;
            slideDir = Direction.none;
            rb.gravityScale = _basegravity;
        }
    }



    void Update()
    {
        CalculateMovementInput();
        
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -velocityLimit, velocityLimit), Mathf.Clamp(rb.velocity.y, -velocityLimit, velocityLimit));

        if(Static.debugMode)
        {
            debugjumps.text = calcJumpDebug(isGrounded, usedDoubleJump, slideDir);
        }
        
    }
    void FixedUpdate()
    {
        MoveThePlayer();
    }

    void CalculateMovementInput()
    {
        if (inputDirection == Vector3.zero)
        {
            currentInput = false;
        }
        else if (inputDirection != Vector3.zero)
        {

            currentInput = true;
        }
    }

    void MoveThePlayer()
    {
        if (currentInput == true)
        {
            Vector3 movement = inputDirection.normalized * movespeed * Time.deltaTime * 100;
            rb.AddForce(movement);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Jump();
        }
        
    }

    public void OnDebug(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            debugjumps.gameObject.SetActive(Static.debugMode);
        }

    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpspeed * 1.5f);
            drawDebugHere();
        }
        else if (isWallSliding)
        {
            if(slideDir == Direction.left)
            {
                rb.velocity = new Vector2(rb.velocity.x + slideBoostStrength, rb.velocity.y + (jumpspeed * 1.2f));
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x - slideBoostStrength, rb.velocity.y + (jumpspeed * 1.2f));
            }
            
            usedDoubleJump = false;
            drawDebugHere();
        }
        else if (!usedDoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + (jumpspeed * 1.8f));
            usedDoubleJump = true;
            drawDebugHere();
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -velocityLimit, velocityLimit), Mathf.Clamp(rb.velocity.y, -velocityLimit, velocityLimit));

    }

    private void drawDebugHere()
    {
        if(Static.debugMode)
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.green, 0.75f, false);
        }
    }
}
