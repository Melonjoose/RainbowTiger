using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = rb.velocity * movementInput;

        if (movementInput != Vector2.zero)
        {
            rb.AddForce(movementInput * 10, ForceMode.Force);
            if (rb.velocity.magnitude > 10)
            {
                rb.velocity = rb.velocity.normalized * 10;
            }
        }
        else
        {
            rb.AddForce(rb.velocity * -1, ForceMode.Force);
        }
    }
}
