using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_SpiderBossMain : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;
    public int totalLegs = 8;
    public int currentLegs;
    public weien_DamagedColor damagedColorScript;
    private bool deathCalled = false;
    [SerializeField] private bool allLegsDestroyed = false;

    [Header("Movement Settings")]
    public float moveSpeed;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;

    [Header("Attack Settings")]
    public GameObject spiderBullet;
    public Transform shootPoint;
    public int numOfBullets;
    public float attackCooldown;
    private bool isAttacking = false;


    void Start()
    {
        randomSpot = Random.Range(0,moveSpots.Length);
        waitTime = startWaitTime;
        currentLegs = totalLegs;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allLegsDestroyed && currentLegs <= 0) {
            allLegsDestroyed=true;
        }

        if (currentHealth <= 0 && !deathCalled) {
            Death();
        }

        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0) 
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }

        }

        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(ShootAtPlayer(attackCooldown));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerWeapons" && allLegsDestroyed)
        {
            currentHealth--;
            damagedColorScript.StartCoroutine("HitColor");
        }
    }
    IEnumerator ShootAtPlayer(float seconds)
    {
        //Play Animation
        yield return new WaitForSeconds(seconds);
        for(int i = 0; i<numOfBullets; i++)
        {
            Instantiate(spiderBullet, shootPoint.position, Quaternion.identity);
        }
        isAttacking = false;
    }

    void Death()
    {
        deathCalled = true;
        Destroy(gameObject);
    }
}
