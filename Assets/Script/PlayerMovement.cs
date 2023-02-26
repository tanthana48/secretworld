using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 2.0f;
    [SerializeField] float jumpSpeed = 4.0f;
    [SerializeField] float climbSpeed = 2.0f;
    [SerializeField] Vector2 deathKick = new Vector2(0, 2f);

    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;
    
    Vector2 moveInput;
    Rigidbody2D rgbd2D;

    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    private CircleCollider2D myCircleCollider;
    float gravityScaleAtStart;

    [SerializeField] private AudioClip shootingSFX;
    [SerializeField] private AudioClip walkSFX;

    bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myCircleCollider = GetComponent<CircleCollider2D>();
        gravityScaleAtStart = rgbd2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) {return; }
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) {return; }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    public float walkingSoundInterval = 0.5f; // Time interval between walking sound effects.
    private float lastWalkingSoundTime = 0f; // Time when the last walking sound effect was played.

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rgbd2D.velocity.y);
        rgbd2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);

            // Play the walking sound effect at a fixed time interval.
            if (Time.time - lastWalkingSoundTime > walkingSoundInterval && myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) )
            {
                AudioSource.PlayClipAtPoint(walkSFX, Camera.main.transform.position, 0.2f);
                lastWalkingSoundTime = Time.time;
            }
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }


    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rgbd2D.velocity.x), 1f);
        }
    }

    [SerializeField] AudioClip jumpSFX; // Jump sound effect.

    void OnJump(InputValue value)
    {
        if (!isAlive) {return; }
        if (!myCircleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
            return;
        }

        AudioSource audioSource = GetComponent<AudioSource>();

        if (value.isPressed)
        {
            rgbd2D.velocity += new Vector2(0f, jumpSpeed);

            // Play the jump sound effect.
            audioSource.clip = jumpSFX;
            audioSource.volume = 0.5f;
            audioSource.Play();
        }

    }
    public float climbingSoundInterval = 0.5f; // Time interval between walking sound effects.
    private float lastClimbingSoundTime = 0f; // Time when the last walking sound effect was played.

    [SerializeField] AudioClip climbSFX;
    void ClimbLadder()
    {
        if(!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        { 
            rgbd2D.gravityScale = gravityScaleAtStart;
            myAnimator.SetBool("isClimbing", false);
            return; 
        }
        Vector2 climbVelocity = new (rgbd2D.velocity.x, moveInput.y*climbSpeed);
        rgbd2D.velocity = climbVelocity;
        rgbd2D.gravityScale = 0f;

        //check vertical speed
        bool playerVerticalSpeed = Mathf.Abs(rgbd2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerVerticalSpeed);
        if (Time.time - lastClimbingSoundTime > climbingSoundInterval && playerVerticalSpeed)
        {
            AudioSource.PlayClipAtPoint(climbSFX, Camera.main.transform.position, 0.3f);
            lastClimbingSoundTime = Time.time;
        }
    }
    [SerializeField] AudioClip hurtSFX;
    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")) || transform.position.y < -5f)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = hurtSFX;
            audioSource.Play();
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            rgbd2D.velocity = deathKick;
            
            rgbd2D.gravityScale = 3f;

            StartCoroutine(informGameSession());
        }
    }

    IEnumerator informGameSession()
    {
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    void OnFire(InputValue value)
    {
        if(!isAlive) {return;}

        Instantiate(bullet, gun.position, transform.rotation);
        myAnimator.SetTrigger("Shooting");
        AudioSource.PlayClipAtPoint(shootingSFX, Camera.main.transform.position, 0.1f);
    }
}
