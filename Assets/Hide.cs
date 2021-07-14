using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{

    public GameObject Player;

    void setCollision(bool flag) {
        BoxCollider bc = GetComponent<BoxCollider>(); 
        if (bc != null) { bc.enabled = flag; }
    }

    // Update is called once per frame
    void Update()
    {
        Transform transform = GetComponent<Transform>();
        if (transform.position[1] >= Player.transform.position[1] + 1) {
            setCollision(false);
            GetComponent<Renderer>().enabled = false;
        } else {
            setCollision(true);
            GetComponent<Renderer>().enabled = true;
        }

    }
}
