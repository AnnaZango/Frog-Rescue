using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    // Game config parameters
    [SerializeField] float speedMovement = 5f;
    [SerializeField] float speedMovementOnLadder = 2f;
    [SerializeField] float jumpHeight = 2f; 
    [SerializeField] float climbLadderSpeed = 3f;
    [SerializeField] Vector2 diekick = new Vector2(250, 250);
    [SerializeField] int currentHealth;
    [SerializeField] Vector2 bumpEnemyKill = new Vector2(0, 10f);
    [SerializeField] int numExtraJumpsTotal = 1;
    [SerializeField] float fallMultiplier = 2f;
    [SerializeField] HealthBar healthBar;

    int maxHealth = 3;

    // attack
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayerToDetect;
    [SerializeField] float radiusAttackPoint=0.5f;
    bool canCapture = true;
    int numCapturedAnimals = 0;

    // Audio:
    [SerializeField] AudioClip mySoundJump;
    [SerializeField] AudioClip mySoundHurt;
    [SerializeField] AudioClip mySoundNet;
    [SerializeField] AudioClip myDeadSound;
    AudioSource myAudioSource;
    
    // State
    bool isAlive = true;
    float originalSpeedMovement;
    bool falling = false;
    bool coroutineHitRunning = false;
    int numCurrentJumps;
    bool isJumping = false;

    
    Animator animator;
    Rigidbody2D myRigidbody;
    BoxCollider2D myColliderfeet;
    CapsuleCollider2D myColliderBody;
    LevelLoader levelLoader;
    GameSession myGameSession;
    float gravityScaleNormal;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();        
    }
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myColliderfeet = GetComponent<BoxCollider2D>();
        myColliderBody = GetComponent<CapsuleCollider2D>();
        levelLoader = FindObjectOfType<LevelLoader>();
        myGameSession = FindObjectOfType<GameSession>();
        numExtraJumpsTotal = myGameSession.GetNumExtraJumps();        
        gravityScaleNormal = myRigidbody.gravityScale;
        originalSpeedMovement = speedMovement;

        numExtraJumpsTotal = myGameSession.GetNumExtraJumps();
        numCapturedAnimals = 0;       

        currentHealth = maxHealth;
        healthBar.SetHealthBar(maxHealth);
    }

    void Update()
    {
        if(!isAlive)
        {            
            return;
        }
        
        Run();
        Jump();
        ClimbLadder();
        FlipSprite();
        InstantKill();
        Attack();        

        if (currentHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(myDeadSound, Camera.main.transform.position);
            Die();
        }
        
        if (myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity = myRigidbody.velocity + Vector2.up * Physics2D.gravity * fallMultiplier*Time.deltaTime;
        }        
       
        healthBar.SetHealthBar(currentHealth);        
    }

    private void Run()
    {    
        float runInput = CrossPlatformInputManager.GetAxis("Horizontal");         
        Vector2 playerVelocity = new Vector2(runInput * speedMovement, myRigidbody.velocity.y);         
        myRigidbody.velocity = playerVelocity;

        bool isMovingHorizontally = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", isMovingHorizontally);
    }

    private void Jump()
    {
        bool isTouchingGround = myColliderfeet.IsTouchingLayers(LayerMask.GetMask("Ground"));
        bool isTouchingLadder = myColliderfeet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
                
        if (isTouchingGround)
        {
            numCurrentJumps = numExtraJumpsTotal;            
        }        
        if (CrossPlatformInputManager.GetButtonDown("Jump") && numCurrentJumps > 0)
        {
            AudioSource.PlayClipAtPoint(mySoundJump, Camera.main.transform.position);
            myRigidbody.velocity = Vector2.up * jumpHeight;
            numCurrentJumps--;
        } 
    }

    private void ClimbLadder()
    {
        bool isInLadder = myColliderfeet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        float climbInput = CrossPlatformInputManager.GetAxis("Vertical");

        if (isInLadder)
        {
            speedMovement = speedMovementOnLadder;
        }
        else
        {
            speedMovement = originalSpeedMovement;
        }
        if (!isInLadder || Mathf.Abs(climbInput) < Mathf.Epsilon)
        {
            animator.SetBool("isClimbing", false);
            if (!isInLadder)
            {
                myRigidbody.gravityScale = gravityScaleNormal;
            }
            return;
        }

        if (isInLadder && Mathf.Abs(climbInput) > Mathf.Epsilon)
        {
            Vector2 climbLadder = new Vector2(myRigidbody.velocity.x, climbInput * climbLadderSpeed);
            myRigidbody.velocity = climbLadder;

            bool isClimbingNow = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
            animator.SetBool("isClimbing", isClimbingNow);
        }
    }
    
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            AudioSource.PlayClipAtPoint(mySoundNet, Camera.main.transform.position, 0.3f);
            animator.SetTrigger("isAttacking");            
        }
    }

    private void CaptureEnemyAttack()
    {
        Collider2D[] myEnemiesHit = Physics2D.OverlapCircleAll(attackPoint.position, radiusAttackPoint, enemyLayerToDetect);

        foreach (Collider2D enemy in myEnemiesHit)
        {
            if (canCapture)
            {                
                print(myEnemiesHit.Length);
                enemy.GetComponent<EnemyMovement>().CaptureEnemyAnimation();                               
            }            
        }
        if (myEnemiesHit.Length > 0)
        {
            numCapturedAnimals++;
        }        
    }

    public int GetNumCapturedAnimals()
    {
        return numCapturedAnimals;
    }    

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, radiusAttackPoint);
    }

    private void FlipSprite()
    {                     
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1);
        }
    }        

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            EnemyMovement enemy = collision.collider.GetComponent<EnemyMovement>();
            if (collision.gameObject.tag == "EnemyKill" || collision.gameObject.tag == "Enemy")
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    if (point.normal.y >= 0.7 && collision.gameObject.tag == "EnemyKill")
                    {
                        myRigidbody.velocity = myRigidbody.velocity + bumpEnemyKill;
                        enemy.DestroyEnemy();
                    }
                    else
                    {
                        if (!coroutineHitRunning)
                        {
                            var myGameSession = FindObjectOfType<GameSession>();
                            if (!myGameSession) { return; }
                            StartCoroutine(TakeHealth());
                        }
                    }
                }
            }
        }
    }
    
    IEnumerator TakeHealth()
    {
        if (currentHealth > 1)
        {
            coroutineHitRunning = true;
            AudioSource.PlayClipAtPoint(mySoundHurt, Camera.main.transform.position, 0.3f);
            animator.SetTrigger("isHurt");
            yield return new WaitForSeconds(0.5f);

            currentHealth = currentHealth - 1;

            coroutineHitRunning = false;
        }
        else
        {
            currentHealth = currentHealth - 1;
        }              
    }    
    
    private void InstantKill()
    {        
        if (myColliderfeet.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            currentHealth = 0;         
        }
    }

    public void Die()
    {
        canCapture = false;
        healthBar.SetHealthBar(currentHealth);        
        isAlive = false;
        myRigidbody.simulated = false;
        currentHealth = 0;
        animator.SetTrigger("isDead");
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
    
    public int CurrentHealth()
    {
        if (currentHealth > 0)
        {
            return currentHealth;
        }
        else
        {
            currentHealth = 0;
            return currentHealth;
        }
    }

}




