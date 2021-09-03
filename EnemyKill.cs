using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    [SerializeField] float speedMovement = 2f;
    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxColliderFeet;
    PolygonCollider2D myBodyCollider;
    Animator animator;
    bool captured = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBoxColliderFeet = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        myBodyCollider = GetComponent<PolygonCollider2D>();

        RandomizeStartingAnimationFrame();
    }

    private void RandomizeStartingAnimationFrame()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0, 1.5f));
    }
    
    void Update()
    {
        if (!captured)
        {
            myRigidbody.velocity = new Vector2(speedMovement, 0);            
        }
        else
        {
            myRigidbody.simulated = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        speedMovement = speedMovement * -1;
        TurnAroundEnemy();
    }

    private bool IsFacingRight()
    {
        return myRigidbody.velocity.x > 0;
    }


    private void TurnAroundEnemy()
    {
        transform.localScale = new Vector2(-(Mathf.Sign(transform.localScale.x) * 0.85f), 0.85f);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    
}
