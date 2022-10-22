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
    enum Direction { none, left, right, top, bottom };
    private Direction slideDir;
    private Direction contactdir;


    [Title("Debug")]
    [SceneObjectsOnly]
    public TMP_Text debugjumps;

    private Rigidbody2D rb;

    public GameObject SparkObj;
    private Vector2 lastsparkloc;
    public GameObject statichelper;

    float distToGround;
    public LayerMask lm;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _basegravity = rb.gravityScale;
        debugjumps.gameObject.SetActive(Static.debugMode);
        EventManager.StartListening("KILL", onKilled);
        EventManager.StartListening("SPARK", Spark);
    }

    private void Start()
    {
        distToGround = GetComponent<Collider2D>().bounds.extents.y;
    }

    private void setYVelocity(float value)
    {
        rb.velocity = new Vector2(rb.velocity.x, value);
        return;
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        contactdir = calcCollisionAngle(transform, collision.contacts[0].point, isGrounded);
        //Side wall => start wall sliding, set direction of slide
        if (collision.gameObject.layer == 3 && (contactdir == Direction.right || contactdir == Direction.left))
        {
            if (Static.debugMode)
            {
                //Draw a blue ray on slide hit
                Debug.DrawRay(collision.GetContact(0).point, collision.GetContact(0).normal, Color.blue, 1, false);
            }
            //Start sliding
            isWallSliding = true;
            usedDoubleJump = false;

            //Compare collision and own position to find direction of collision
            slideDir = ((collision.transform.position - transform.position).normalized).x >= 0 ? Direction.right : Direction.left;


            //Set gravity to be lower when sliding
            rb.gravityScale = _basegravity * gravitySlideScale;
        }
        //ground => grounded, reset double jump
        else if (collision.gameObject.layer == 3 && contactdir == Direction.top)
        {
          //  isGrounded = true;
            usedDoubleJump = false;
        }
        
    }
    
    private string calcJumpDebug(bool isGrounded, bool usedDoubleJump, Direction slidedir, Direction contactDir)
    {
        if(isGrounded)
        {
            return $"Jumps: Grounded(2)\n Sliding: {slidedir} Contact: {contactDir}\n Level(current/max): {Static.currentSelectedlevel} {Static.maxBeatenLevel} ";
        }
        else if (!usedDoubleJump)
        {
            return $"Jumps: Air(1)\n Sliding: {slidedir} Contact: {contactDir}\n Level(current/max): {Static.currentSelectedlevel} {Static.maxBeatenLevel}";
        }
        return $"Jumps: Air(0)\n Sliding: {slidedir} Contact: {contactDir}\n Level(current/max): {Static.currentSelectedlevel} {Static.maxBeatenLevel}";
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Leave wall
        if (collision.gameObject.layer == 3 && (contactdir == Direction.right || contactdir == Direction.left))
        {
            isWallSliding = false;
            slideDir = Direction.none;
            rb.gravityScale = _basegravity;
        }
        //Leave ground
        else if (collision.gameObject.layer == 3 && contactdir == Direction.top)
        {
           // isGrounded = false;
            contactdir = Direction.none;
            
        }
        

    }



    void Update()
    {
        CalculateMovementInput();
        
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -velocityLimit, velocityLimit), Mathf.Clamp(rb.velocity.y, -velocityLimit, velocityLimit));

        if(isGrounded)
        {
            isWallSliding = false;
        }

        if(Static.debugMode)
        {
            debugjumps.text = calcJumpDebug(isGrounded, usedDoubleJump, slideDir, contactdir);
        }
        
    }
    void FixedUpdate()
    {
        MoveThePlayer();
        isGrounded = CheckIfGrounded();
        if(Static.debugMode)
        {
            Debug.DrawRay(transform.position, -Vector2.up * 0.3f, Color.blue, 1, false);
        }
        
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
        if (Static.disablePlayerMovement)
        {
            return;
        }
        if (currentInput == true)
        {
            //Vector3 movement = inputDirection.normalized * movespeed * Time.deltaTime * 100;
            //rb.AddForce(movement);
            accelerateTowardsDirection(inputDirection.normalized);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>().normalized;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started && !Static.gamePaused)
        {
            Jump();
        }
        
    }

    public void OnSpark(InputAction.CallbackContext context)
    {
        if (context.started && !Static.gamePaused)
        {
            onKilled();
            
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
        if(Static.disablePlayerMovement)
        {
            return;
        }
        if (isWallSliding)
        {
            if(contactdir == Direction.left)
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
        else if(isGrounded)
        {
            setYVelocity(jumpspeed * 1.5f);
            drawDebugHere();
        }
        else if (!usedDoubleJump)
        {
            setYVelocity(jumpspeed * 2f);
            usedDoubleJump = true;
            drawDebugHere();
        }
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -velocityLimit, velocityLimit), Mathf.Clamp(rb.velocity.y, -velocityLimit, velocityLimit));

    }

    void accelerateTowardsDirection(Vector2 direction)
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, direction.x * movespeed * 8, 0.35f * Time.deltaTime), rb.velocity.y);
    }
    private void drawDebugHere()
    {
        if(Static.debugMode)
        {
            Debug.DrawRay(transform.position, Vector2.up, Color.green, 0.75f, false);
        }
    }


    private void onKilled()
    {
        lastsparkloc = transform.position;
        if(lastsparkloc.y < -5)
        {
            lastsparkloc.y += 4;
        }
        transform.position = Static.CurrentLevelLevelTemplate.startingPosition;

        EventManager.EmitEvent("RESPAWN");
        Spark();
    }

    private void Spark()
    {
        if(Static.disablePlayerMovement)
        {
            return;
        }

        GameObject sp = Instantiate(SparkObj, new Vector2(lastsparkloc.x, lastsparkloc.y), transform.rotation);
        sp.GetComponent<SparkCT>().sparkid = Static.sparkid;
        Static.sparkid += 1;
        
        statichelper.GetComponent<StaticHelper>().checkSparks();

    }


    private Direction calcCollisionAngle(Transform col1, Vector2 col2, bool isGrounded)
    {
        Vector2 vector = col2 - (Vector2)col1.position;
        //vector = vector.normalized;
        float Angle = Mathf.Atan2(vector.y, vector.x);
        float collisionAngle = 360 - (Angle * Mathf.Rad2Deg) - 180;

        Direction cang;
        cang = calcCastedDir(collisionAngle, isGrounded);

        if(Static.debugMode)
        {
            Debug.Log(cang + "  " + collisionAngle + "  transform" + (Vector2)col1.position + "   " + col2 + "   " + vector + "   " + isGrounded);
        }
        
        return cang;
    }

    private Direction calcCastedDir(float collisionAngle, bool isGrounded)
    {
        if(!isGrounded)
        {
            if ((collisionAngle > 315 && collisionAngle <= 360) || (collisionAngle > 0 && collisionAngle <= 45))
            {
                return Direction.left;
            }
            else if (collisionAngle > 45 && collisionAngle <= 135)
            {
                return Direction.bottom;
            }
            else if (collisionAngle > 135 && collisionAngle <= 225)
            {
                return Direction.right;
            }
            else if (collisionAngle > 225 && collisionAngle <= 315)
            {
                return Direction.top;
            }
        }
        else
        {
            if (collisionAngle > 180 && collisionAngle <= 360)
            {
                return Direction.top;
            }
            else if (collisionAngle > 0 && collisionAngle <= 180)
            {
                return Direction.bottom;
            }

        }
       

        return Direction.none;
    }

    private bool CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f, lm);
        if(hit.collider != null)
        {
 
            return true;

        }
        return false;
        
    }


}
