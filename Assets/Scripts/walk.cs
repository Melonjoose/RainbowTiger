using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Transform rootObject; // Root object to link target objects' positions
    public Transform[] groupOneObjects; // First group of objects
    public Transform[] groupTwoObjects; // Second group of objects with offset
    public float radius = 5f; // Radius of the circular movement
    public float rotationSpeed = 50f; // Speed of movement around the circle
    public bool reverseGroupOne = false; // Toggle to switch rotation direction for group one (clockwise when unchecked)
    public bool reverseGroupTwo = true; // Toggle to switch rotation direction for group two (anticlockwise when unchecked)
    public float offsetTime = 1f; // Offset time delay for the second group

    private float angle = 0f;
    private Vector3[] initialPositionsOne;
    private Vector3[] initialPositionsTwo;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        
        if (groupOneObjects != null && groupOneObjects.Length > 0)
        {
            initialPositionsOne = new Vector3[groupOneObjects.Length];
            for (int i = 0; i < groupOneObjects.Length; i++)
            {
                if (groupOneObjects[i] != null)
                {
                    initialPositionsOne[i] = rootObject.InverseTransformPoint(groupOneObjects[i].position);
                }
            }
        }

        if (groupTwoObjects != null && groupTwoObjects.Length > 0)
        {
            initialPositionsTwo = new Vector3[groupTwoObjects.Length];
            for (int i = 0; i < groupTwoObjects.Length; i++)
            {
                if (groupTwoObjects[i] != null)
                {
                    initialPositionsTwo[i] = rootObject.InverseTransformPoint(groupTwoObjects[i].position);
                }
            }
        }
    }

    void Update()
    {
        if ((groupOneObjects != null && groupOneObjects.Length > 0) || (groupTwoObjects != null && groupTwoObjects.Length > 0))
        {
            // Determine direction based on toggles (clockwise default for group one, anticlockwise default for group two)
            float directionOne = reverseGroupOne ? 1f : -1f;
            float directionTwo = reverseGroupTwo ? -1f : 1f;
            
            // Increment the angle over time
            float currentAngleOne = (Time.time - startTime) * directionOne * rotationSpeed;
            float currentAngleTwo = (Time.time - startTime - offsetTime) * directionTwo * rotationSpeed;
            
            for (int i = 0; i < groupOneObjects.Length; i++)
            {
                if (groupOneObjects[i] != null)
                {
                    float x = Mathf.Cos(currentAngleOne * Mathf.Deg2Rad) * radius;
                    float y = Mathf.Sin(currentAngleOne * Mathf.Deg2Rad) * radius;
                    groupOneObjects[i].position = rootObject.TransformPoint(initialPositionsOne[i] + new Vector3(x, y, 0));
                }
            }
            
            for (int i = 0; i < groupTwoObjects.Length; i++)
            {
                if (groupTwoObjects[i] != null)
                {
                    float x = Mathf.Cos(currentAngleTwo * Mathf.Deg2Rad) * radius;
                    float y = Mathf.Sin(currentAngleTwo * Mathf.Deg2Rad) * radius;
                    groupTwoObjects[i].position = rootObject.TransformPoint(initialPositionsTwo[i] + new Vector3(x, y, 0));
                }
            }
        }
    }
}

