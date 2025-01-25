using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //this is what the camera follows
     
    [SerializeField] float smoothTime = 0.05f; 
    [SerializeField] float offsetY;


    private void FixedUpdate()
    {
        if (target != null)
        {
            if (target.position.y + offsetY > transform.position.y)
            {
                Vector3 desiredPosition = new Vector3(0, target.position.y + offsetY, -10);
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothTime);

                transform.position = smoothedPosition;


            }
        }
    }
}
