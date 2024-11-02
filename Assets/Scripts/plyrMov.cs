using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyrMov : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Rigidbody")]
    public Rigidbody2D theRB;

    [Header("Editable-Player-Stuff")]
    public float plyrMovSpd;
    public float accelRate;
    public float plyrAccel;
    public float accelMax;
    public float jumpForce;
    public float fallForce;
    public float distanceToWall;
    float horizontalInput;
    float verticalInput;
    public int atBase;

    [Header("Grounded-Stuff")]
    public bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Line Stuff")]
    public LineRenderer lineRenderer

    void Start()
    {
        theRB.freezeRotation = true;
        plyrAccel = 1f;
        atBase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Setup Stuff.
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        theRB.velocity = new Vector2(horizontalInput * (plyrMovSpd * plyrAccel), theRB.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (horizontalInput > 0 && plyrAccel < accelMax)
        {
            plyrAccel += accelRate * Time.deltaTime;
        }
        if (horizontalInput <=0 && plyrAccel >= 1f)
        {
            plyrAccel -= accelRate * Time.deltaTime;
        }

        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            Debug.Log("This is working");
            Jump();
        }   

        if (verticalInput < 0)
        {
            FastFall();
        }
    }

    public void FastFall()
    {
        Debug.Log("Fall!");
        theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y - fallForce);
    }
    public void Jump()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y + jumpForce);
    }

    public int GetClosestPoint()
    {
        
    }

}
