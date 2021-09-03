using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBLTP : MonoBehaviour
{
    [SerializeField] float speedMovement = 2f;
    [SerializeField] float jumpForce = 10f;
    
    private float moveInput;
    private Rigidbody2D myRigidBody;
    bool isFacingRight = true;

    bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float radiusSize;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] int numExtraJumps = 2;
    int currentNumJumps;


    void Start()
    {
        currentNumJumps = numExtraJumps;
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusSize, whatIsGround);

        if (isGrounded)
        {
            currentNumJumps = numExtraJumps;
        }

        if(Input.GetKeyDown(KeyCode.Space) && currentNumJumps > 0)
        {
            myRigidBody.velocity = Vector2.up * jumpForce;
            currentNumJumps--;
        } else if(Input.GetKeyDown(KeyCode.Space) && currentNumJumps==0 && isGrounded)
        {
            myRigidBody.velocity = Vector2.up * jumpForce;
        }
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("Horizontal");
        myRigidBody.velocity = new Vector2(moveInput*speedMovement, myRigidBody.velocity.y);
        Flip();
    }

    private void Flip()
    {        
        bool playerIsMoving = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1);            
        }
        if (myRigidBody.velocity.x >= 0)
        {
            isFacingRight = true;
        }
        else
        {
            isFacingRight = false;
        }
    }
}
