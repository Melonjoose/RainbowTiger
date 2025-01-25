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
    private bool deathCalled = false;
    [SerializeField] private bool allLegsDestroyed = false;

    [Header("Movement Settings")]
    public float moveSpeed;
    private int randomSpot;
    private float waitTime;
    public float startWaitTime;
    public Transform[] moveSpots;

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerWeapons" && allLegsDestroyed)
        {
            currentHealth--;
        }
    }

    void Death()
    {
        deathCalled = true;
        Destroy(gameObject);
    }
}
