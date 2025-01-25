using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_enemyTest : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0)
        {
            Death();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapons")
        {
            currentHealth--;
        }
    }

    private void Death()
    {
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}
