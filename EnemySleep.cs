using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySleep : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float movementSpeed;
    [SerializeField] float distanceToDetect = 5f;
    Rigidbody2D rb;
    PolygonCollider2D myFeetCollider;

    float distanceToTarget;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myFeetCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.position);
       
        if (distanceToTarget < distanceToDetect)
        {
            ChaseTarget();            
        }
        else
        {
            StopChasing();            
        }
    }

    private void ChaseTarget()
    {
        if(transform.position.x < target.position.x)
        {
            rb.velocity = new Vector2(movementSpeed, 0);
            transform.localScale = new Vector2(2, 2);
        }
        else if(transform.position.x > target.position.x)
        {
            rb.velocity = new Vector2(-movementSpeed, 0);
            transform.localScale = new Vector2(-2, 2);
        }
        else if(transform.position.x == target.position.x)
        {
            rb.velocity = new Vector2(0,0);
            transform.localScale = new Vector2(2, 2);
        }
    }

    private void StopChasing()
    {
        rb.velocity = new Vector2(0, 0);
    }

}
