using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 15f;
    private bool isGrounded = false;
    private bool facintRight = true;


    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Run()
    {
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
    }
    private void Flip()
    {
      facintRight = !facintRight;
      Vector3 Scaler = transform.localScale;
      Scaler.x *=-1;
      transform.localScale = Scaler;
    }
    private void Update()
    {
        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }
    private void FixedUpdate()
    {
        if(facintRight==false && (Input.GetButton("Horizontal")))
        {
          Flip();
        }
        if(Input.GetAxis("Horizontal") == 0)
        {
           anim.SetBool("isRunning",false);
        }
        else
        {
           anim.SetBool("isRunning",true);
        }
        CheckGround();
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
    }
}
