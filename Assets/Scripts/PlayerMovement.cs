using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 15;
    private Vector3 move;

    public float gravity = -10;
    public float jumpHeight = 8;
    private Vector3 velocity;

    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float runSpeed = 0;
        if (Input.GetKey(KeyCode.LeftShift))
            runSpeed += 10;

        animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(z));

        move = transform.right * x + transform.forward * z;

        controller.Move(move * (speed + runSpeed) * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, groundLayer);

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
                Jump();
        }
        else
            velocity.y += gravity * Time.deltaTime * 2;

        controller.Move(velocity * Time.deltaTime);

    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * -gravity);
    }
}
