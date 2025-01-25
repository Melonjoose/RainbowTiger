using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weien_StickDirectionCheck : MonoBehaviour
{
    public float rayDistance = 2f;
    public Transform pivotTransform;
    public LayerMask wallLayer;

    //1=StickDown, 2=StickUp, 3=StickLeft, 4=StickRight
    private int stickDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D upHit = Physics2D.Raycast(
            origin: transform.position,
            direction: pivotTransform.up,
            distance: rayDistance,
            layerMask: wallLayer
            );

        switch (stickDirection)
        {
            case 1:
                Debug.Log("Stick Down");
                break;
            case 2:
                Debug.Log("Stick Up");
                break;
            case 3:
                Debug.Log("Stick Left");
                break;
            case 4:
                Debug.Log("Stick Right");
                break;
            default:
                Debug.Log("Stick Down");
                break;
        }
    }
}
