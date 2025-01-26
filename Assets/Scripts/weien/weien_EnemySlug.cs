using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class weien_EnemySlug : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;
    public weien_DamagedColor damagedColorScript;
    private bool deathCalled = false;

    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float moveDistance = 5f;
    public float timeBeforeMove = 2.5f;
    [SerializeField] private float moveTimer;
    [SerializeField] private bool moveUp = true;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float raycastDistance;
    private float scale;

    [Header("Attack Settings")]
    public Transform shootPoint;
    public GameObject projectile;
    public float projectileSpeed;
    public float shootDelay;
    private GameObject player;
    private bool isAttacking;
    private bool shootCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        scale = transform.localScale.y;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 && !deathCalled)
        {
            Death();
        }

        if (moveTimer > 0 && !isAttacking)
        {
            moveTimer -= Time.deltaTime;
        }

        if (moveTimer <= 0)
        {
            isAttacking = true;
            RaycastHit2D obstacleUp = Physics2D.Raycast(
            origin: transform.position,
            direction: transform.up,
            distance: raycastDistance,
            layerMask: wallLayer
            );
            RaycastHit2D obstacleDown = Physics2D.Raycast(
            origin: transform.position,
            direction: transform.up * -1,
            distance: raycastDistance,
            layerMask: wallLayer
            );
            if (moveUp) 
            {   
                if(obstacleUp.collider == null)
                {
                    Vector2 newPos = new Vector2(transform.position.x, transform.position.y + moveDistance);
                    transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
                }
                
                if (!shootCalled)
                {
                    transform.localScale = new Vector3(scale, scale, scale);
                    StartCoroutine(ShootAtPlayer(shootDelay));
                    shootCalled = true;
                }
            }
            else
            {
                if(obstacleDown.collider == null)
                {
                    Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveDistance);
                    transform.position = Vector2.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
                }
                
                if (!shootCalled) 
                {
                    transform.localScale = new Vector3(scale, -scale, scale);
                    StartCoroutine(ShootAtPlayer(shootDelay)); 
                    shootCalled = true;
                }
                
            }
        }
    }

    IEnumerator ShootAtPlayer(float seconds)
    {
        //Play Animation
        yield return new WaitForSeconds(seconds);
        GameObject slugBullet = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        Vector2 direction = player.transform.position - shootPoint.position;
        slugBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * projectileSpeed;
        isAttacking = false;
        if (moveUp) { moveUp = false; } else { moveUp = true; }
        moveTimer = timeBeforeMove;
        shootCalled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerWeapons")
        {
            currentHealth--;
            damagedColorScript.StartCoroutine("HitColor");
        }
    }

    void Death()
    {
        deathCalled = true;
        Destroy(gameObject);
    }
}
