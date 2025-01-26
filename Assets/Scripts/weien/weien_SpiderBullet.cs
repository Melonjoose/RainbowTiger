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
        Vector2 direction = Random.insideUnitCircle.normalized;
        rb.velocity = direction * moveSpeed;
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
