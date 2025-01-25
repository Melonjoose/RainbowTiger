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

    [SerializeField] Transform lastPlatformTransform;


    [Header("Enemies")]
    [SerializeField] Transform enemy_start;
    [SerializeField] Transform[] enemies;
    [SerializeField] [Range(0, 10)] int enemiesPerSection = 3;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYSpacing = 10f;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYRandomOffset = 10f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMin = -1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMax = 1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float spawnCheckRadius = 3f;

    [SerializeField] Transform lastEnemyTransform;
 


    void Start()
    { 
        lastEndPosition = wallSection_start.Find("End Position").position;
        lastPlatformTransform = platform_start;
        lastEnemyTransform = enemy_start;
    }

    private void Update()
    {
        if (player != null)
        {
            // Spawn next wall part when player is less than a certain distance away from the last/upcoming end position
            if ((lastEndPosition.y - player.position.y) < playerDistSpawnWallPart)
            {
                SpawnWallPart();
                //SpawnPlatforms();
                lastPlatformTransform = SpawnModules(platformsPerSection, platforms, lastPlatformTransform, platformYSpacing, platformXMin, platformXMax, platformYRandomOffset);
                lastEnemyTransform = SpawnModules(enemiesPerSection, enemies, lastEnemyTransform, enemyYSpacing, enemyXMin, enemyXMax, enemyYRandomOffset);

            }
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

    // <------------------------------ GENERIC MODULE SPAWNER ------------------------------> //
    // Note: cannot assign lastPlatformTransform directly through parameters, need to assign it outside this

    Transform SpawnModules(int modulesPerSection, Transform[] moduleArray, Transform lastModuleTransform, float ModuleYSpacing, float moduleXMin, float moduleXMax, float moduleYRandomOffset)
    {
        for (int i = 0; i < modulesPerSection; i++)
        {
            bool canSpawnHere = false;
            Vector3 newSpawnPosition = new Vector3(0, 0, 0);

            //Calculates the next semi-random position to spawn the module

            while (!canSpawnHere)
            {
                float newModuleYPos = lastModuleTransform.position.y + ModuleYSpacing + Random.Range(0, moduleYRandomOffset);
                float newModuleXPos = Random.Range(moduleXMin, moduleXMax);

                newSpawnPosition = new Vector3(newModuleXPos, newModuleYPos, 0);
                canSpawnHere = CheckSpawnPositionValidity(newSpawnPosition);
            }

            //Chooses a random module from the desired module array to spawn
            Transform randomModule = moduleArray[Random.Range(0, moduleArray.Length)];


            Transform newModuleTransform = Instantiate(randomModule, newSpawnPosition, Quaternion.identity);
            lastModuleTransform = newModuleTransform;
        }
        return lastModuleTransform;
    }

    bool CheckSpawnPositionValidity(Vector3 spawnPosition)
    {
        bool isSpawnPositionValid = true;
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(spawnPosition, spawnCheckRadius);
        for (int i = 0; i < overlapColliders.Length; i++)
        {
            Vector3 centrepoint = overlapColliders[i].bounds.center;
            float xExtent = overlapColliders[i].bounds.extents.x;
            float yExtent = overlapColliders[i].bounds.extents.y;
            float buffer = 0.5f;

            float leftExtent = centrepoint.x - (xExtent + buffer);
            float rightExtent = centrepoint.x + (xExtent + buffer);
            float topExtent = centrepoint.y + (yExtent + buffer);
            float bottomExtent = centrepoint.y - (yExtent + buffer);

            if (spawnPosition.y >= bottomExtent && spawnPosition.y <= topExtent)
            {
                if (spawnPosition.x >= leftExtent && spawnPosition.x <= rightExtent)
                {
                    isSpawnPositionValid = false;
                }
            }

        }
        return isSpawnPositionValid;
    }

    // <------------------------------ PLATFORMS ------------------------------> //

    void SpawnPlatforms()
    {
        for (int i = 0; i < platformsPerSection; i++)
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
    /*
    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerSection; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        bool canSpawnHere = false;
        Vector3 newSpawnPosition = new Vector3(0, 0, 0);

        //Calculates the next semi-random position to spawn the enemy

        while (!canSpawnHere)
        {
            float newEnemyYPos = lastEnemyTransform.position.y + enemyYSpacing + Random.Range(0, enemyYRandomOffset);
            float newEnemyXPos = Random.Range(enemyXMin, enemyXMax);

            newSpawnPosition = new Vector3(newEnemyXPos, newEnemyYPos, 0);
            canSpawnHere = CheckSpawnPositionValidity(newSpawnPosition);
        }

        //Chooses a random enemy to spawn
        Transform randomEnemy = enemies[Random.Range(0, enemies.Length)];

        if (newSpawnPosition != null)
        lastEnemyTransform = Instantiate(randomEnemy, newSpawnPosition, Quaternion.identity);
        
    }

    bool CheckSpawnPositionValidity(Vector3 spawnPosition)
    {
        bool isSpawnPositionValid = true;
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(spawnPosition, spawnCheckRadius);
        for (int i = 0; i < overlapColliders.Length; i++)
        {
            Vector3 centrepoint = overlapColliders[i].bounds.center;
            float xExtent = overlapColliders[i].bounds.extents.x;
            float yExtent = overlapColliders[i].bounds.extents.y;

            float leftExtent = centrepoint.x - xExtent;
            float rightExtent = centrepoint.x + xExtent;
            float topExtent = centrepoint.y + yExtent;
            float bottomExtent = centrepoint.y - yExtent;

            if (spawnPosition.y >= bottomExtent && spawnPosition.y <= topExtent)
            {
                if (spawnPosition.x >= leftExtent && spawnPosition.x <= rightExtent)
                {
                    isSpawnPositionValid = false;
                }
            }
        
        } 
        return isSpawnPositionValid;
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
        Transform enemyTransform = Instantiate(randomEnemy, spawnPosition, Quaternion.identity);
        return enemyTransform;
    }
    */
}
