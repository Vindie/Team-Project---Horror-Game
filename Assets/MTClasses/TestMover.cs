using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMover : MonoBehaviour {

    Rigidbody rb;
    public int moveSpeed = 10;
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = moveSpeed * Time.deltaTime * transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = moveSpeed * Time.deltaTime * transform.forward * -1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.left * moveSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right * moveSpeed);
        }

    }
}
