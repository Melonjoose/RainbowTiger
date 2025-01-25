using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] Transform player; 
    public float playerHeight; //absolute height of player, used in level generator
    public float playerScore; //displayed score or highest height of player
    public TextMeshProUGUI heightText;

    [Header("Boss Fight Settings")]
    public bool isBossFightActive;
    public Transform currentBossSectionTransform;
     

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

    void WinBossFight()
    {
        //Open the exit door
        currentBossSectionTransform.Find("Exit Door").gameObject.SetActive(false);
        isBossFightActive = false;
    }

}
