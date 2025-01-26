using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public UI_Manager uiManager;

    [Header("Player Settings")]
    [SerializeField] Transform player; 
    public float playerHeight; //absolute height of player, used in level generator
    public float playerScore; //displayed score or highest height of player
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI finalHeightText;

    [Header("Boss Fight Settings")]
    public bool isBossFightActive;

    public Transform currentBossSectionTransform;

    private void Start()
    {
        uiManager = FindObjectOfType<UI_Manager>();
        playerScore = 0;
        isBossFightActive = false;
    }

    private void Update()
    {
        if (player != null)
        {
            playerHeight = player.position.y;
            
            if (playerHeight > playerScore)
            {
                playerScore = playerHeight;
            }
            
            UpdateHeightCounterUI();
        }
    }

    void UpdateHeightCounterUI()
    {
        heightText.text = Mathf.Round(playerScore).ToString();
    }

    public void WinBossFight()
    {
        //Open the exit door
        currentBossSectionTransform.Find("Exit Door").gameObject.SetActive(false);
        isBossFightActive = false;
    }

    public void TriggerPlayerDeath()
    {
        StartCoroutine(HandlePlayerDeath());
    }

    private IEnumerator HandlePlayerDeath()
    {
        Debug.Log("Player death sequence started...");
        AudioManager.Instance.StopOST();
        AudioManager.Instance.PlaySFX("SFX_Gum_Death");
        yield return new WaitForSeconds(2.0f);

        finalHeightText.text = Mathf.Round(playerScore).ToString();

        if (uiManager != null)
        {
            uiManager.GameOver();
            Time.timeScale = 0;
        }
        else
        {
            Debug.LogError("UI_Manager instance is missing.");
        }

        Debug.Log("Player death sequence completed.");
    }

}
