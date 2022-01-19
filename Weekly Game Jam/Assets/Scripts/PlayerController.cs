using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0,10)] float jumpForce;
    [SerializeField, Range(0, 10)] float airSpeed;
    [SerializeField, Range(0, 10)] float groundSpeed;
    [SerializeField, Range(0, 20)] float maxVelocity;
    [SerializeField, Range(0, 10)] float maxAirVelocity;

    public float fallMultiplier;
    public float lowJumpMultiplier;
    public bool onGround;
    
    public LayerMask platformLayerMask;
    
    private float speed;
    private float moveVertical;
    public float moveHorizontal;
    
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider2D;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        capsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        if (IsGrounded())
            speed = groundSpeed;
        else
            speed = airSpeed;
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxVelocity && IsGrounded())
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //rb.MovePosition(transform.forward * speed);
            //rb.velocity = new Vector2(speed * moveHorizontal, 0f);
        }
        else if (Mathf.Abs(moveHorizontal) > 0.1f && Mathf.Abs(rb.velocity.x) < maxAirVelocity)
        {
            rb.AddForce(new Vector2(moveHorizontal * speed, 0f), ForceMode2D.Impulse);
            //rb.MovePosition(transform.forward * speed);
            //rb.velocity = new Vector2(speed * moveHorizontal, 0f);
        }


        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }

        else if (rb.velocity.x > 0 && moveHorizontal == 0 && IsGrounded())
            rb.velocity = new Vector2(0f,0f);
        else if (rb.velocity.x < -0 && moveHorizontal == 0 && IsGrounded())
            rb.velocity = new Vector2(0f, 0f);
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(capsuleCollider2D.bounds.center, capsuleCollider2D.bounds.size, 0f, Vector2.down, .5f, platformLayerMask);
        Debug.Log(raycastHit2D.collider);
        Debug.DrawRay(transform.position, Vector2.down*5f);
        onGround = raycastHit2D.collider;
        return raycastHit2D.collider != null;
    }
}
