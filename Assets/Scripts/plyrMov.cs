using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyrMov : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Rigidbody")]
    public Rigidbody2D theRB;
    public Rigidbody2D theRB2;

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
    public Transform playerSprite;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Line Stuff")]
    public bool canSlide;
    public LayerMask iceLayer;
    public LineRenderer lineRenderer;
    public int currentPointIndex = 0;
    public int iceSpeed;
    public float iceCheckRadius;
    public float iceAccelRate, iceAccel;

    void Start()
    {
        theRB.freezeRotation = true;
        iceAccel = 0;
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
        canSlide = Physics2D.OverlapCircle(playerSprite.position, iceCheckRadius, iceLayer);

        if (canSlide)
        {
            iceAccel = iceAccelRate;
        }
        if (!canSlide)
        {
            iceAccel = 0;
        }

        if (horizontalInput > 0 && plyrAccel < accelMax)
        {
            plyrAccel += (accelRate + iceAccel) * Time.deltaTime;
        }
        if (horizontalInput <= 0 && plyrAccel >= 1f)
        {
            plyrAccel -= (accelRate + iceAccel) * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || canSlide))
        {
            Debug.Log("This is working");
            Jump();
        }

        if (verticalInput < 0)
        {
            FastFall();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ice"))
        {
            Debug.Log("You're on Ice!");
            lineRenderer = other.GetComponent<LineRenderer>();
            currentPointIndex = GetClosestIndex();

            if (lineRenderer != null)
            {
                Debug.Log("Found a LineRenderer on" + other.name);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Ice") && lineRenderer != null)
        {
            canSlide = true;
            iceAccel = iceAccelRate;

            if (currentPointIndex >= lineRenderer.positionCount)
            {
                return;
            }
            Vector2 targetPosition = lineRenderer.GetPosition(currentPointIndex);
            Debug.Log("Target position is " + targetPosition);
            Vector2 direction = (targetPosition - theRB.position).normalized;
            Debug.Log("Direction is " + direction);
            theRB2.AddForce((direction * iceSpeed) * Time.deltaTime, ForceMode2D.Force);
            float distance = Vector2.Distance(theRB.position, lineRenderer.GetPosition(lineRenderer.positionCount - 1));
            if (distance < 0.1f)
            {
                currentPointIndex++;
                if (currentPointIndex >= lineRenderer.positionCount)
                {
                    currentPointIndex = lineRenderer.positionCount - 1; // Stay on last point if exceeded
                }
            }


        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ice"))
        {
            canSlide = false; // Reset sliding state
            lineRenderer = null; // Clear reference to LineRenderer
            currentPointIndex = 0; // Optionally reset index
            Debug.Log("Exited Ice");
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

    public void IceJump()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y + (jumpForce * plyrAccel));
    }

    public int GetClosestIndex()
    {
        int closestIndex = -1;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector2 pointPosition = lineRenderer.GetPosition(i);
            float distance = Vector2.Distance(playerSprite.position, pointPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        Debug.Log("Closest index is " + closestIndex);

        return closestIndex;
    }

}