using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] Transform player; 

    [Header("Walls")]
    [SerializeField] Transform wallSection_start;
    [SerializeField] Transform[] wallSections;
    [SerializeField] float playerDistSpawnWallPart = 10f;

    private Vector3 lastEndPosition;


    [Header("Platforms")]
    [SerializeField] Transform platform_start;
    [SerializeField] Transform[] platforms;
    [SerializeField] [Range(0, 10)] int platformsPerSection = 5;
    [SerializeField] [Range(0.0f, 10.0f)] float platformYSpacing = 8f;
    [SerializeField] [Range(0.0f, 10.0f)] float platformYRandomOffset = 10f;
    [SerializeField] [Range(-5.0f, 5.0f)] float platformXMin = -1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float platformXMax = 1.3f;

    private Transform lastPlatformTransform;


    [Header("Enemies")] 
    [SerializeField] Transform[] enemies;
    [SerializeField] [Range(0, 10)] int enemiesPerSection = 3;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYSpacing = 10f;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYRandomOffset = 10f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMin = -1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMax = 1.3f;

    private Transform lastEnemyTransform;


    void Start()
    { 
        lastEndPosition = wallSection_start.Find("End Position").position;
        lastPlatformTransform = platform_start;
    }

    private void Update()
    {
        // Spawn next wall part when player is less than a certain distance away from the last/upcoming end position
        if ((lastEndPosition.y - player.position.y) < playerDistSpawnWallPart)
        {
            SpawnWallPart();
            SpawnPlatforms();
            SpawnEnemies();
        }
    }

    // <------------------------------ WALLS ------------------------------> //

    void SpawnWallPart()
    {
        Transform lastWallPartTransform = SpawnWallPart(lastEndPosition);
        lastEndPosition = lastWallPartTransform.Find("End Position").position;
    }

    Transform SpawnWallPart(Vector3 spawnPosition)
    {
        Transform randomWallSection = wallSections[Random.Range(0, wallSections.Length)];
        Transform wallSectionTransform = Instantiate(randomWallSection, spawnPosition, Quaternion.identity);
        return wallSectionTransform;
    }

    // <------------------------------ PLATFORMS ------------------------------> //

    void SpawnPlatforms()
    {
        for(int i=0; i < platformsPerSection; i++)
        {
            SpawnPlatform();
        }
    }

    void SpawnPlatform()
    {
        float newPlatformYPos = lastPlatformTransform.position.y + platformYSpacing + Random.Range(0, platformYRandomOffset);
        float newPlatformXPos = Random.Range(platformXMin, platformXMax);
        Vector3 newSpawnPosition = new Vector3(newPlatformXPos, newPlatformYPos, 0);
        lastPlatformTransform = SpawnPlatform(newSpawnPosition);
    }

    Transform SpawnPlatform(Vector3 spawnPosition)
    {
        Transform randomPlatform = platforms[Random.Range(0, platforms.Length)];
        Transform platformTransform = Instantiate(randomPlatform, spawnPosition, Quaternion.identity);
        return platformTransform;
    }

    // <------------------------------ ENEMIES ------------------------------> //

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerSection; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        float newEnemyYPos = lastEnemyTransform.position.y + enemyYSpacing + Random.Range(0, enemyYRandomOffset);
        float newEnemyXPos = Random.Range(enemyXMin, enemyXMax);
        Vector3 newSpawnPosition = new Vector3(newEnemyXPos, newEnemyYPos, 0);
        lastEnemyTransform = SpawnEnemy(newSpawnPosition);
    }

    Transform SpawnEnemy(Vector3 spawnPosition)
    {
        Transform randomEnemy = enemies[Random.Range(0, enemies.Length)];
        Transform enemyTransform = Instantiate(randomEnemy);
        return enemyTransform;
    }
}
