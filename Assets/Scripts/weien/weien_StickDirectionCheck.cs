using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class weien_StickDirectionCheck : MonoBehaviour
{
    public Animator animator;
    public float rayDistance = 2f;
    public Transform playerTransform;
    public LayerMask wallLayer;

    //1=StickDown, 2=StickUp, 3=StickLeft, 4=StickRight, 5=MidAir
    private int stickDirection = 1;

    private bool stickingDown = false;
    private bool stickingUp = false;
    private bool stickingLeft = false;
    private bool stickingRight = false;
    private bool stickingMidAir = false;

    private float scale;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        RaycastHit2D upHit = Physics2D.Raycast(
            origin: transform.position,
            direction: playerTransform.up,
            distance: rayDistance,
            layerMask: wallLayer
            );

        RaycastHit2D downHit = Physics2D.Raycast(
            origin: transform.position,
            direction: playerTransform.up * -1,
            distance: rayDistance,
            layerMask: wallLayer
            );

        RaycastHit2D leftHit = Physics2D.Raycast(
            origin: transform.position,
            direction: playerTransform.right * -1,
            distance: rayDistance,
            layerMask: wallLayer
            );

        RaycastHit2D rightHit = Physics2D.Raycast(
            origin: transform.position,
            direction: playerTransform.right,
            distance: rayDistance,
            layerMask: wallLayer
            );

        if (upHit.collider != null)
        {
            if (stateInfo.IsName("AM_Gum_FloatTransition") || stateInfo.IsName("AM_Gum_Shoot"))
            {
                animator.SetTrigger("IdleGround");
            }
            stickDirection = 2;
        }
        else if (downHit.collider != null)
        {
            if (stateInfo.IsName("AM_Gum_FloatTransition") || stateInfo.IsName("AM_Gum_Shoot"))
            {
                animator.SetTrigger("IdleGround");
            }
            stickDirection = 1;
        }
        else if (leftHit.collider != null)
        {
            if (stateInfo.IsName("AM_Gum_FloatTransition") || stateInfo.IsName("AM_Gum_Shoot"))
            {
                animator.SetTrigger("IdleWall");
            }
            stickDirection = 3;
        }
        else if (rightHit.collider != null)
        {
            if (stateInfo.IsName("AM_Gum_FloatTransition") || stateInfo.IsName("AM_Gum_Shoot"))
            {
                animator.SetTrigger("IdleWall");
            }
            stickDirection = 4;
        }
        else if (upHit.collider == null && downHit.collider == null && leftHit.collider == null && rightHit.collider == null)
        {
            stickDirection = 5;
        }

        switch (stickDirection)
        {
            case 1:
                if (stickingDown) { break; }
                //Debug.Log("Stick Down");
                
                ResetBooleans(1);
                SetAnimState(1);
                stickingDown = true;
                break;
            case 2:
                if (stickingUp) { break; }
                //Debug.Log("Stick Up");
                ResetBooleans(2);
                SetAnimState(2);
                stickingUp = true;
                break;
            case 3:
                if (stickingLeft) { break; }
                //Debug.Log("Stick Left");
                ResetBooleans(3);
                SetAnimState(3);
                stickingLeft = true;
                break;
            case 4:
                if (stickingRight) { break; }
                //Debug.Log("Stick Right");
                ResetBooleans(4);
                SetAnimState(4);
                stickingRight = true;
                break;
            case 5:
                if (stickingMidAir) { break; }
                //Debug.Log("Mid air");
                ResetBooleans(5);
                SetAnimState(5);
                stickingMidAir = true;
                break;
        }
    }

    void ResetBooleans(int excludeDirection)
    {
        switch (excludeDirection)
        {
            case 1:
                stickingUp = false;
                stickingLeft = false;
                stickingRight = false;
                stickingMidAir = false;
                break;
            case 2:
                stickingDown = false;
                stickingLeft = false;
                stickingRight = false;
                stickingMidAir = false;
                break;
            case 3:
                stickingUp = false;
                stickingDown = false;
                stickingRight = false;
                stickingMidAir = false;
                break;
            case 4:
                stickingUp = false;
                stickingLeft = false;
                stickingDown = false;
                stickingMidAir = false;
                break;
            case 5:
                stickingDown = false;
                stickingUp = false;
                stickingLeft = false;
                stickingRight = false;
                break;
        }
    }

    void SetAnimState(int direction)
    {
        switch (direction)
        {
            case 1:
                animator.SetTrigger("IdleGround");
                GetComponent<weien_PlayerController>().onWall = false;
                transform.localScale = new Vector3(scale, scale, scale);
                break;
            case 2:
                animator.SetTrigger("IdleGround");
                GetComponent<weien_PlayerController>().onWall = false;
                transform.localScale = new Vector3(scale, -scale, scale); 
                break;
            case 3:
                animator.SetTrigger("IdleWall");
                GetComponent<weien_PlayerController>().onWall = true;
                transform.localScale = new Vector3(-scale, scale, scale);
                Debug.Log("left");
                break;
            case 4:
                animator.SetTrigger("IdleWall");
                GetComponent<weien_PlayerController>().onWall = true;
                transform.localScale = new Vector3(scale, scale, scale);
                break;
            case 5:
                Vector3 resetY = transform.localScale;
                resetY.y = scale;
                transform.localScale = resetY;
                break;
        }
    }
}
