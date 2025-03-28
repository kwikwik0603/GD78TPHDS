using System;
using UnityEngine;

public class S_PlayerMovement : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 moveDirection;
    private Rigidbody rb;
    [SerializeField] private float jumpForce;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveDirection * moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        float horInput = Input.GetAxis("Horizontal");
        float verInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            DoJump();
        }

        moveDirection = new Vector3(horInput, 0, verInput);

    }

    void DoJump()
    {
        Vector3 force = new Vector3(0, jumpForce, 0);
        rb.AddForce(force, ForceMode.Impulse);
    }
}
