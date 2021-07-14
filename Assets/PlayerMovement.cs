using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float Move;

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if ( Input.GetKey("w") ) {
            rb.AddForce(0, 0, Move);
        }
        if ( Input.GetKey("a") ) {
            rb.AddForce(-Move, 0, 0);
        }
        if ( Input.GetKey("s") ) {
            rb.AddForce(0, 0, -Move);
        }
        if ( Input.GetKey("d") ) {
            rb.AddForce(Move, 0, 0);
        }
        if ( Input.GetKey(KeyCode.Space) ) {
            rb.AddForce(0, 5*Move, 0);
        }
    }
}
