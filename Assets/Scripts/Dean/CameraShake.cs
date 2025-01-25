using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool returnToOriginalPositionTrue;

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalCameraPosition = new Vector3 (0, transform.position.y, transform.position.z);
        float elapsed = 0f; //stores elapsed time


        while (elapsed < duration)
        {
            if (Time.timeScale > 0)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
                elapsed += Time.deltaTime;
            }
            yield return null;
        }

        if (returnToOriginalPositionTrue == true)
        {
            transform.position = originalCameraPosition; //returns the camera to its original position after the shake
        }
    }
}
