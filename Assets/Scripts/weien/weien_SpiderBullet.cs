using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_SpiderBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 direction = GameObject.FindGameObjectWithTag("Player").GetComponent<weien_PlayerController>().GetRandomPositionNearPlayer();
        rb.velocity = new Vector2(direction.x, direction.y).normalized * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
