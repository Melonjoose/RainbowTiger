using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOtherObjectOnTriggerEnter : MonoBehaviour
{
    [SerializeField] Collider2D myCollider;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    { 
        if (other.gameObject != null)
        {
            if (other.gameObject.tag == "Player")
            {
                gameManager.GetComponent<GameManager>().TriggerPlayerDeath();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != null)
        {
            if (other.gameObject.tag == "Player")
            {
                gameManager.GetComponent<GameManager>().TriggerPlayerDeath();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }

}
