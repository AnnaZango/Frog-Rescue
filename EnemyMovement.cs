using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float speedMovement = 2f;
    [SerializeField] GameObject particlesDead;
    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxColliderFeet;
    PolygonCollider2D myBodyCollider;
    Animator animator;
    bool captured = false;

    [SerializeField] AudioClip mySoundSkeleton;
    [SerializeField] AudioClip mySoundDie;
    AudioSource myAudioSource;

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
            InstantKill();
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
        transform.localScale = new Vector2(-(Mathf.Sign(transform.localScale.x)), 1f);
    }

    public void DestroyEnemy()
    {
        AudioSource.PlayClipAtPoint(mySoundSkeleton, Camera.main.transform.position, 1.5f);
        AudioSource.PlayClipAtPoint(mySoundDie, Camera.main.transform.position, 0.2f);
        GameObject particlesDeath = Instantiate(particlesDead, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        Destroy(particlesDeath, 0.7f);
    }

    public void CaptureEnemyAnimation()
    {
        if (captured) { return; }
        AudioSource.PlayClipAtPoint(mySoundSkeleton, Camera.main.transform.position);
        captured = true;
        StartCoroutine(CaptureDelay());
    }

    IEnumerator CaptureDelay()
    {        
        myBodyCollider.enabled = false;
        myBoxColliderFeet.enabled = false;
        animator.SetTrigger("isCaptured");
        yield return new WaitForSeconds(1f);
        
        Destroy(this.gameObject);
    }

    private void InstantKill()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            GameObject particlesDeath = Instantiate(particlesDead, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject);
            Destroy(particlesDeath, 0.7f);
        }
    }

}
