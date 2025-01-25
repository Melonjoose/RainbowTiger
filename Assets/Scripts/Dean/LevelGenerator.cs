using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform wallSection;
    [SerializeField] Transform wallSection_start;

    private Vector3 lastEndPosition;
    [SerializeField] float playerDistSpawnWallPart = 10;
    [SerializeField] GameObject player;
     

    void Start()
    { 
        lastEndPosition = wallSection_start.Find("End Position").position;
       
    }

    private void Update()
    {
        // Spawn next wall part when player is less than a certain distance away from the last/upcoming end position
        if ((lastEndPosition.y - player.transform.position.y) < playerDistSpawnWallPart)
        {
            SpawnWallPart();
        }
    }

    void SpawnWallPart()
    {
        Transform lastWallPartTransform = SpawnWallPart(lastEndPosition);
        lastEndPosition = lastWallPartTransform.Find("End Position").position;
    }

    Transform SpawnWallPart(Vector3 spawnPosition)
    {
        Transform wallSectionTransform = Instantiate(wallSection, spawnPosition, Quaternion.identity);
        return wallSectionTransform;
    }
}
