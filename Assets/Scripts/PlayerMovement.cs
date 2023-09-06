using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private bool isGrounded;
    private Vector3 velocity;
    private Vector3 move;

    public float speed = 15;
    public float gravity = -10;
    public float jumpHeight = 8;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Debug.Log($"x: {x}; y: {z};");

        float runSpeed = 0;
        if (Input.GetKey(KeyCode.LeftShift))
            runSpeed += 10;

        animator.SetFloat("speed", Mathf.Abs(x) + Mathf.Abs(z));

        move = transform.right * x + transform.forward * z;

        controller.Move((speed + runSpeed) * Time.deltaTime * move);

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
