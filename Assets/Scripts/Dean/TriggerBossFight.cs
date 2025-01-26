using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBossFight : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] Collider2D myCollider;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager.isBossFightActive = true;
            AudioManager.Instance.StopOST();
            AudioManager.Instance.PlaySFX("SFX_Spider_Spawn");
            AudioManager.Instance.PlayOST("OST_Boss");
        }
    }
}
