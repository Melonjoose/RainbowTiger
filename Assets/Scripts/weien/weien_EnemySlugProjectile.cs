using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_EnemySlugProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
