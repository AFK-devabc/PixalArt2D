using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D collider2D;
    private Rigidbody2D body;

    private Animator animator;
    float HorizontalInput;

    [SerializeField] private float mouseDelay;
    private float countMouseDelay;

    private bool isleft = false;
    private bool isMove = true;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
    }
    private void FixedUpdate()
    {
        if (!isMove)
        {
            body.velocity = new Vector2(0,0);
        }
        else
        {
            HorizontalInput = Input.GetAxis("Horizontal");
            if (HorizontalInput > 0.0001f)
            {
                transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (isleft == true)
                    animator.SetTrigger("turn");
                isleft = false;
            }
            else if (HorizontalInput < -0.0001f)
            {
                transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (isleft == false)
                    animator.SetTrigger("turn");
                isleft = true;

            }
            body.velocity = new Vector2(HorizontalInput * Speed, body.velocity.y);

        }
        animator.SetBool("run", math.abs(HorizontalInput) > 0.0001f);
        animator.SetBool("grounded", isGrounded());



        
    }
    private void Update()
    {
        countMouseDelay -= Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            Jump();
        if (Input.GetMouseButton(0))

            if (countMouseDelay <= 0)
            {
                MeleeAttack();
                countMouseDelay = mouseDelay;
            }
    }



    private Vector3 GetLocalScale()
    {
        return transform.localScale;
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            animator.SetTrigger("jump");
        }
    }
    private void MeleeAttack()
    {
        animator.SetTrigger("meleeattack");
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(collider2D.bounds.center, collider2D.bounds.size, 0, Vector2.down * Time.deltaTime, 0.1f, groundLayer);
        return raycastHit2D.collider != null;

    }
    public void LockMove()
    {
        isMove = false;
    }
    public void UnlockMove()
    {
        isMove = true;
    }
}
