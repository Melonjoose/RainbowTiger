using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherObjectOnTriggerEnter : MonoBehaviour
{

    [SerializeField] Collider2D myCollider; 
     

    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject != null)
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            Destroy(other.gameObject);
        }
    }

}
