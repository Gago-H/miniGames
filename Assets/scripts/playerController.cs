using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float backwardsSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpHeight;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;


    private Vector3 moveDirection;
    private Vector3 velocity;


    private CharacterController controller;
    private Animator anim;


    void Start()
    {
        controller = GetComponentInChildren<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();

    }
    private void Move()
    {
        Vector3 sphereCenter = transform.position + Vector3.down * (groundCheckDistance / 2);
        isGrounded = Physics.CheckSphere(sphereCenter, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        moveDirection = new Vector3(0, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);

        if (isGrounded)
        {

            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                //walk backwards
                walkBackwords();
            }

            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S))
            {
                //walk
                Walk();

            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.S) )
            {
                //run
                Run();
            }

            else if (moveDirection == Vector3.zero )
            {
                //idle
                idle();
            }

            if (Input.GetKeyDown(KeyCode.R) )
            {
                //emote
                defeatedEmote();

            }
            moveDirection *= moveSpeed;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
            }
        }

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        //Gizmos.DrawSphere(transform.position, groundCheckDistance);
    }
    private void idle()
    {
        moveDirection = Vector3.zero;
        anim.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("Speed", 0.5F, 0.1f, Time.deltaTime);

    }

    private void walkBackwords()
    {
        moveSpeed = backwardsSpeed;
        anim.SetFloat("Speed", -1f, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }
    private void jump()
    {
        anim.SetTrigger("jump");
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private void defeatedEmote()
    {
        anim.SetTrigger("defeated");
    }
}

