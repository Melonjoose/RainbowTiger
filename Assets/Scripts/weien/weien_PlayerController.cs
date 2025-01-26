using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class weien_PlayerController : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Animator")]
    public Animator animator;
    public bool onWall = false;

    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;
    public GameObject mainCamHolder;
    public weien_DamagedColor damagedColorScript;
    private bool deathCalled = false;

    [Header("Grapple Settings")]
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    public Vector3 grapplePoint;
    private DistanceJoint2D joint;
    [SerializeField] weien_GrappleRope ropeObject;
    public float timeUntilGrapple;
    public float grappleCooldown;
    private float grappleCooldownTimer;

    [Header("BubbleBullet Settings")]
    public Rigidbody2D rb;
    public Transform pivotTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    [Header("BubbleFloat Settings")]
    public float bfAcceleration = 2f;
    public float bfMaxSpeed = 10f;
    public float bfSpeed = 2f;
    public float leftRightStrength = 0.4f;
    public float timeBeforeStart = 1f;
    private float holdTimer;
    private Vector2 floatDirection;
    private bool isFloating = false;
    [SerializeField] private bool floatActivated = false;
    private bool floating = false;

    [Header("Grounded Check")]
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.8f;
    private bool isGrounded;

    [Header("GetRandomPositionNearPlayer Settings")]
    public float randomPosRadius;

    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        floatDirection.y = 1;
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, grappleLayer);

        //Shoot BubbleBullet
        if (Input.GetMouseButtonDown(0))
        {
            BubbleBullet();
        }

        //Mini-Jump when right-click
        if (Input.GetMouseButtonDown(1) && isGrounded && grappleCooldownTimer <= 0)
        {
            if (!onWall)
            {
                animator.SetTrigger("JumpGround");
            }
            else
            {
                animator.SetTrigger("JumpWall");
            }
            
            rb.AddForce(pivotTransform.up * 8, ForceMode2D.Impulse);
            ropeObject.line.enabled = false;
            joint.enabled = false;
        }

        //Grapple and/or stop BubbleFloat when right-click is released
        if (Input.GetMouseButtonUp(1) && grappleCooldownTimer <= 0)
        {
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D grappleHit = Physics2D.Raycast(
            origin: transform.position,
            direction: pivotTransform.up,
            distance: Vector2.Distance(mousePosition, playerPosition),
            layerMask: grappleLayer
            );

            if (grappleHit.collider != null)
            {
                AudioManager.Instance.PlaySFX("SFX_Gum_Grapple"); 

                grapplePoint = grappleHit.point;
                grapplePoint.z = 0;
                ropeObject.line.enabled = true;
                StartCoroutine(ropeObject.AnimateRope(grapplePoint));
                joint.connectedAnchor = grapplePoint;
                joint.distance = grappleLength;
                StartCoroutine(StartGrapple(timeUntilGrapple));

                UI_Manager uiManager = FindObjectOfType<UI_Manager>();

                if (uiManager != null)
                {
                    uiManager.GameStart();
                    Debug.Log("GameStart method triggered!");
                }
            }

            if (isFloating)
            {
                isFloating = false;
                holdTimer = 0;
                
            }
            
            floatActivated = false;
            floating = false;
            grappleCooldownTimer = grappleCooldown;
            animator.SetTrigger("FloatActivated");
        }

        //BubbleFloat when right-click is held for an amount of time
        if (Input.GetMouseButton(1) && grappleCooldownTimer <= 0)
        {
            if (!floatActivated) 
            {
                Debug.Log("test");
                animator.SetTrigger("FloatActivated");
                AudioManager.Instance.PlaySFX("SFX_Gum_Inflate");
                floatActivated = true;
            }
            holdTimer += Time.deltaTime;
            if (holdTimer >= timeBeforeStart)
            {
                isFloating = true;
            }
        }
        if (isFloating)
        {
            if (!floating)
            {
                animator.SetTrigger("Floating");
                floating = true;
            }
            BubbleFloat();
        }

        //Grappling cooldown
        if (grappleCooldownTimer > 0)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }

        if (currentHealth <= 0 && !deathCalled)
        {
            Death();
        }

        //Check sticking direction

    }


    private void FixedUpdate()
    {
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        pivotTransform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    IEnumerator StartGrapple(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        joint.enabled = true;
        ropeObject.line.positionCount = 0;
        ropeObject.line.SetPosition(1, grapplePoint);
    }
    void BubbleBullet()
    {
        AudioManager.Instance.PlaySFX("SFX_Gum_Shoot");
        animator.SetTrigger("Shoot");
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().AddForce(pivotTransform.up * bulletSpeed, ForceMode2D.Impulse);
    }

    void BubbleFloat()
    {
        
        if ((mousePosition.x < transform.position.x))
        {
            floatDirection.x = -leftRightStrength;
        }
        else
        {
            floatDirection.x = leftRightStrength;
        }
        rb.velocity = new Vector2(floatDirection.x, floatDirection.y) * bfSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            damagedColorScript.StartCoroutine("HitColor");
            mainCamHolder.GetComponent<Animator>().SetTrigger("PlayerHit");
            joint.enabled = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(pivotTransform.up * -6, ForceMode2D.Impulse);
            currentHealth--;
        }
    }

    private void Death()
    {
        if (!deathCalled)
        {
            deathCalled = true;

            if (gameManager != null)
            {
                gameManager.TriggerPlayerDeath();
                Debug.Log("TriggerPlayerDeath method triggered!");
            }
            else
            {
                Debug.LogError("GameManager reference is null. Ensure it's assigned in the Inspector.");
            }

            Debug.Log("Player has died.");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }

    public Vector2 GetRandomPositionNearPlayer()
    {
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        float xOffset = Mathf.Cos(randomAngle) * randomPosRadius;
        float yOffset = Mathf.Sin(randomAngle) * randomPosRadius;
        Vector2 offsetPosition = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);

        return offsetPosition;
    }
}

