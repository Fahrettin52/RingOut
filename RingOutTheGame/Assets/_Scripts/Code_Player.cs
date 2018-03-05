using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Code_Player : MonoBehaviour {

    public int movementSpeed;
    public int rotationSpeed;

    public Vector3 input;

    public Rigidbody rigidbod;

    public Vector3 targetRotation;

    private void Start()
    {
        rigidbod = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        Movement();
	}

    /// <summary>
    /// Handles the movement and otation of the player
    /// </summary>
    private void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        if (move != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(move * Time.deltaTime);
        }

        transform.Translate(move * movementSpeed * Time.deltaTime, Space.World);
    }
}
