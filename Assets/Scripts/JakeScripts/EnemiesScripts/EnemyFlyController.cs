using System.Collections;
using UnityEngine;

public class EnemyFlyController : MonoBehaviour
{
    #region REFERENCES
    [Header("REFERENCES")]
    [SerializeField] private GameObject sprite;
    public GameObject player;
    private float scale;
    #endregion

    #region PARAMETERS
    [Header("PARAMETERS")]
    public float movementSpeed;
    public float maxHealth;
    public float currentHealth;
    #endregion

    #region COLLISION CHECKS
    [Header("COLLISION CHECKS")]
    [SerializeField] private Transform hitboxCheck;
    [SerializeField] private Vector2 hitboxCheckSize;

    [SerializeField] private LayerMask playerLayer;
    #endregion

    private void OnEnable()
    {
        sprite = gameObject.transform.GetChild(0).gameObject;
        scale = sprite.transform.localScale.x;

        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = maxHealth;
    }

    private void Update()
    {
        //FOLLOW PLAYER
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(x, y, transform.position.z), movementSpeed * Time.deltaTime);

        //FLIP
        Vector3 direction = (player.transform.position - transform.position).normalized;
        if (direction.x < 0)
        {
            sprite.transform.localScale = new Vector3(scale, scale, scale);
        }

        else
        {
            sprite.transform.localScale = new Vector3(-scale, scale, scale);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerWeapons")
        {
            currentHealth--;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Fly has died.");
            Destroy(gameObject);
        }
    }

    #region EDITOR METHODS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitboxCheck.position, 1);
    }
    #endregion
}
