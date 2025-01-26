using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameManager gameManager;

    [Header("Player Settings")]
    [SerializeField] Transform player; 

    [Header("Walls")]
    [SerializeField] Transform wallSection_start;
    [SerializeField] Transform[] wallSections;
    [SerializeField] float playerDistSpawnWallPart = 10f;

    [SerializeField] Transform bossSection;
    [SerializeField] float bossSectionHeightInterval = 100f;
    private float upcomingBossSectionHeight;
    private Transform bossSectionTransform;

    public Vector3 lastEndPosition;
    private Vector3 previousEndPosition;


    [Header("Platforms")]
    [SerializeField] Transform platform_start;
    [SerializeField] Transform[] platforms;
    [SerializeField] [Range(0, 10)] int platformsPerSection = 2;
    [SerializeField] [Range(0.0f, 10.0f)] float platformYSpacing = 8f;
    [SerializeField] [Range(0.0f, 10.0f)] float platformYRandomOffset = 10f;
    [SerializeField] [Range(-5.0f, 5.0f)] float platformXMin = -1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float platformXMax = 1.3f;

    [SerializeField] Transform lastPlatformTransform;
    private float platformYPos;

    [Header("Enemies")]
    [SerializeField] Transform enemy_start;
    [SerializeField] Transform[] enemies;
    [SerializeField] [Range(0, 10)] int enemiesPerSection = 1;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYSpacing = 10f;
    [SerializeField] [Range(0.0f, 10.0f)] float enemyYRandomOffset = 10f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMin = -1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float enemyXMax = 1.3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float spawnCheckRadius = 3f;
    [SerializeField] [Range(-5.0f, 5.0f)] float spawnPositionValidityBuffer = 0.5f;

    [SerializeField] Transform lastEnemyTransform;
    private float enemyYPos;


    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        lastEndPosition = wallSection_start.Find("End Position").position;
        lastPlatformTransform = platform_start;
        lastEnemyTransform = enemy_start;
        platformYPos = lastPlatformTransform.position.y;

        upcomingBossSectionHeight = bossSectionHeightInterval;
    }

    private void Update()
    {
        if (player != null)
        {
            // Spawn next wall part when player is less than a certain distance away from the last/upcoming end position
            if ((lastEndPosition.y - player.position.y) < playerDistSpawnWallPart)
            {
               
                // If current player height exceeds the height of the upcoming boss section
                if (gameManager.playerHeight > upcomingBossSectionHeight)
                {
                    //Increase the required height for the boss section
                    upcomingBossSectionHeight += bossSectionHeightInterval;

                    // Spawn the current boss section, communicate it to GameManager
                    bossSectionTransform = Instantiate(bossSection, lastEndPosition, Quaternion.identity);
                    gameManager.currentBossSectionTransform = bossSectionTransform;

                    // Set the last end position to that of the spawned boss section
                    lastEndPosition = bossSectionTransform.Find("End Position").position;

                    print("Boss section spawned.");
                    print("Upcoming boss section height: " + upcomingBossSectionHeight);

                    SpawnModules();
                }

                else
                { 
                    // Spawn a normal wall section, must spawn platforms before walls so that I can constrain the platforms
                    SpawnWallPart(); 
                    print("Normal wall section spawned.");
                   
                    SpawnModules();
                    
                    //lastPlatformTransform = SpawnModules(lastEndPosition, platformsPerSection, platforms, lastPlatformTransform, platformYSpacing, platformXMin, platformXMax, platformYRandomOffset);
                    //lastEnemyTransform = SpawnModules(lastEndPosition, enemiesPerSection, enemies, lastEnemyTransform, enemyYSpacing, enemyXMin, enemyXMax, enemyYRandomOffset);

                }

            }
        }
    }

    void SpawnModules()
    {
        SpawnPlatforms();
        if (gameManager.isGameStarted)
        {
            SpawnEnemies();
        }
    }

    // <------------------------------ WALLS ------------------------------> //

    public void SpawnWallPart()
    {
        Transform lastWallPartTransform = SpawnWallPart(lastEndPosition);
        lastEndPosition = lastWallPartTransform.Find("End Position").position;
    }

    Transform SpawnWallPart(Vector3 spawnPosition)
    {
        Transform randomWallSection = wallSections[Random.Range(0, wallSections.Length)];
        if (!gameManager.isGameStarted)
        {
            //Hard code the random wall 
            randomWallSection = wallSections[0];
        }
        Transform wallSectionTransform = Instantiate(randomWallSection, spawnPosition, Quaternion.identity);
        return wallSectionTransform;
    }

    // <------------------------------ PLATFORMS ------------------------------> //

    void SpawnPlatforms()
    {
        platformYPos = lastEndPosition.y;
        
        for (int i = 0; i < platformsPerSection; i++)
        {
            bool canSpawnHere = false;
            Vector3 newSpawnPosition = new Vector3(0, 0, 0);
            //Calculates the next semi-random position to spawn the module
            while (!canSpawnHere)
            {
                float newModuleYPos = platformYPos + platformYSpacing + Random.Range(0, platformYRandomOffset);
                float newModuleXPos = Random.Range(platformXMax, platformXMin);

                newSpawnPosition = new Vector3(newModuleXPos, newModuleYPos, 0);
                canSpawnHere = CheckSpawnPositionValidity(newSpawnPosition);
            }

            //Chooses a random module from the desired module array to spawn
            Transform randomModule = platforms[Random.Range(0, platforms.Length)];
            
            Transform newModuleTransform = Instantiate(randomModule, newSpawnPosition, Quaternion.identity);
            lastPlatformTransform = newModuleTransform;
            platformYPos = lastPlatformTransform.position.y;
        }
    }

    // <------------------------------ ENEMIES ------------------------------> //

    void SpawnEnemies()
    {
        enemyYPos = lastEndPosition.y;
        int n = 0;
        for (int i = 0; i < enemiesPerSection; i++)
        {
            bool canSpawnHere = false;
            Vector3 newSpawnPosition = new Vector3(0, 0, 0);
            //Calculates the next semi-random position to spawn the module
            while (!canSpawnHere && n>20)
            {
                float newModuleYPos = enemyYPos + enemyYSpacing + Random.Range(0, enemyYRandomOffset);
                float newModuleXPos = Random.Range(enemyXMin, enemyXMax);

                newSpawnPosition = new Vector3(newModuleXPos, newModuleYPos, 0);
                canSpawnHere = CheckSpawnPositionValidity(newSpawnPosition);
                n++;
            }

            //Chooses a random module from the desired module array to spawn
            Transform randomModule = enemies[Random.Range(0, enemies.Length)];


            Transform newModuleTransform = Instantiate(randomModule, newSpawnPosition, Quaternion.identity);
            lastEnemyTransform = newModuleTransform;
            enemyYPos = lastEnemyTransform.position.y;
        }
    }


    // <------------------------------ GENERIC MODULE SPAWNER ------------------------------> //
    // Note: cannot assign lastPlatformTransform directly through parameters, need to assign it outside this

    /*
    Transform SpawnModules(Vector3 lastEndPosition, int modulesPerSection, Transform[] moduleArray, Transform lastModuleTransform, float ModuleYSpacing, float moduleXMin, float moduleXMax, float moduleYRandomOffset)
    {
        float targetYPos = lastModuleTransform.position.y;

        if (lastModuleTransform.position.y < lastEndPosition.y)
        {
            targetYPos = lastEndPosition.y;
        }

        for (int i = 0; i < modulesPerSection; i++)
        { 
            bool canSpawnHere = false;
            Vector3 newSpawnPosition = new Vector3(0, 0, 0);

            //Calculates the next semi-random position to spawn the module

            while (!canSpawnHere)
            {
                
                float newModuleYPos = targetYPos + ModuleYSpacing + Random.Range(0, moduleYRandomOffset);
                float newModuleXPos = Random.Range(moduleXMin, moduleXMax);

                newSpawnPosition = new Vector3(newModuleXPos, newModuleYPos, 0);
                canSpawnHere = CheckSpawnPositionValidity(newSpawnPosition);
            }

            //Chooses a random module from the desired module array to spawn
            Transform randomModule = moduleArray[Random.Range(0, moduleArray.Length)];

            if (newSpawnPosition.y < lastEndPosition.y)
            {
            Transform newModuleTransform = Instantiate(randomModule, newSpawnPosition, Quaternion.identity);
            lastModuleTransform = newModuleTransform;
            }
        }
        return lastModuleTransform;
    }
    */
    bool CheckSpawnPositionValidity(Vector3 spawnPosition)
    {
        bool isSpawnPositionValid = true;
        Collider2D[] overlapColliders = Physics2D.OverlapCircleAll(spawnPosition, spawnCheckRadius);
        for (int i = 0; i < overlapColliders.Length; i++)
        {
            Vector3 centrepoint = overlapColliders[i].bounds.center;
            float xExtent = overlapColliders[i].bounds.extents.x;
            float yExtent = overlapColliders[i].bounds.extents.y;
           
            float leftExtent = centrepoint.x - (xExtent + spawnPositionValidityBuffer);
            float rightExtent = centrepoint.x + (xExtent + spawnPositionValidityBuffer);
            float topExtent = centrepoint.y + (yExtent + spawnPositionValidityBuffer);
            float bottomExtent = centrepoint.y - (yExtent + spawnPositionValidityBuffer);

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
    /*
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
    */

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
