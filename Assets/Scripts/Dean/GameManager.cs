using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] Transform player; 
    public float playerHeight;
    public bool isBossFightActive;
    public Transform currentBossSectionTransform;

    private void Start()
    {
    }

    private void Update()
    {
        if (player != null)
        {
            playerHeight = player.position.y;
        }
    }

    

}
