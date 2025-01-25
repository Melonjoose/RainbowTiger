using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //this is what the camera follows
    [SerializeField] float smoothTime = 0.05f; 
    [SerializeField] float offsetY;
    [SerializeField] float cameraZPos = -10f;

    private GameManager gameManager;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); 

    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            // Freezes the camera to the current boss fight room when the boss fight is active
            if (gameManager.isBossFightActive && gameManager.currentBossSectionTransform != null)
            {
                Vector3 bossRoomCameraPosition = gameManager.currentBossSectionTransform.Find("Camera Position").position;
                desiredPosition = new Vector3(0, bossRoomCameraPosition.y, cameraZPos);

                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);

                transform.position = smoothedPosition;
            }

            else if (target.position.y + offsetY > transform.position.y)
            {
                desiredPosition = new Vector3(0, target.position.y + offsetY, cameraZPos);

                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);

                transform.position = smoothedPosition;
            }

        }

    }
}
