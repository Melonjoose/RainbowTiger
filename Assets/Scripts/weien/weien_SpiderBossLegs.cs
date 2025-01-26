using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_SpiderBossLegs : MonoBehaviour
{
    private bool deathCalled = false;
    public float maxHealth;
    public float currentHealth;
    public weien_DamagedColor damagedColorScript;
    [SerializeField] private weien_SpiderBossMain spiderBossMain;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth <= 0 && !deathCalled)
        {
            deathCalled = true;
            spiderBossMain.currentLegs -= 1;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerWeapons")
        {
            AudioManager.Instance.PlaySFX("SFX_Spider_Movement02");
            currentHealth--;
            damagedColorScript.StartCoroutine("HitColor");
        }
    }
}
