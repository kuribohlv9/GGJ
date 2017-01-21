using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Vector2 direction = Vector2.zero;
    private Vector2 WavedashDirection = Vector2.zero;
    public Transform GroundCheck;
    private bool grounded = false;
    private float WavedashTimer = 0;
    private bool EnableWavedash = false;
    private bool CanWavedash = false;
    private bool EnableShortHopCheck = false;
    private float gravity;
    private bool canWallJump = true;
    private Collider2D walljumpcollider;
    private float shorthopcheck = 0;

    public float speed = 1;
    public float jumpheight = 1;
    public float wavedashspeed = 1;
    public float wavedashlenght = 1;
    public float shorthopcheckvalue = 0.1f;
    public float shorthopcut = 0.5f;

    public float extraspeedfrominitialgap = 1;
    public float extraspeedfrominitial = 1;

    public bool EnableDebug = false;
    // Use this for initialization
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        gravity = rigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EnableWavedash)
        {
            HandleInput();

            CheckGrounded();
            HandleJumping();
            HandleShortHop();

            HandleMovement();

            HandleWavedash();
            FakeAnimation();
        }
        else
        {
            WavedashTimer -= Time.deltaTime;
            if (WavedashTimer < 0)
            {
                EnableWavedash = false;
                rigidbody.gravityScale = gravity;
            }

            rigidbody.velocity = WavedashDirection.normalized * Time.deltaTime * wavedashspeed;
        }
    }
    private void CheckGrounded()
    {
        grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.15f, 1 << LayerMask.NameToLayer("Ground"));
        if (grounded)
        {
            CanWavedash = true;
        }
    }

    private void HandleInput()
    {
        direction.x = Input.GetAxis("Horizontal");
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            rigidbody.AddForce(new Vector2(0, jumpheight));
            transform.localScale = new Vector3(0.5f, 1, 1);
            EnableShortHopCheck = true;
        }
        else if (canWallJump)
        {

        }
    }

    private void HandleShortHop()
    {
        if(EnableShortHopCheck)
        {
            shorthopcheck -= Time.deltaTime;
            if(shorthopcheck < 0)
            {
                if(!Input.GetButton("Jump"))
                {
                    Vector2 tempvel = rigidbody.velocity;
                    tempvel.y *= shorthopcut;
                    rigidbody.velocity = tempvel;
                }
                shorthopcheck = shorthopcheckvalue;
                EnableShortHopCheck = false;
            }
        }
    }

    private void HandleMovement()
    {
        if (direction != Vector2.zero)
        {
            rigidbody.AddForce(direction.normalized * Time.deltaTime * speed);
            if (rigidbody.velocity.x < extraspeedfrominitialgap && direction.x > 0 || rigidbody.velocity.x > -extraspeedfrominitialgap && direction.x < 0)
                rigidbody.AddForce(direction.normalized * Time.deltaTime * extraspeedfrominitial);
        }
    }

    private void HandleWavedash()
    {
        if (CanWavedash && Input.GetButtonDown("Fire1"))
        {
            WavedashDirection.x = Input.GetAxis("Horizontal");
            WavedashDirection.y = Input.GetAxis("Vertical");

            EnableWavedash = true;
            WavedashTimer = wavedashlenght;
            rigidbody.gravityScale = 0;
            transform.localScale = new Vector3(0.5f, 1, 1);
            CanWavedash = false;

        }
    }

    private void FakeAnimation()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * 2);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (EnableWavedash && col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                Vector2 angledirection = WavedashDirection.normalized - contact.normal * Vector2.Dot(WavedashDirection.normalized, contact.normal);
                rigidbody.velocity = angledirection.normalized * Time.deltaTime * wavedashspeed;
            }

            EnableWavedash = false;
            rigidbody.gravityScale = gravity;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (canWallJump && !grounded && Input.GetButtonDown("Jump"))
        {
            Vector2 normal = Vector2.zero;
            foreach (ContactPoint2D contact in col.contacts)
            {
                normal = contact.normal;
            }
            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce((normal + Vector2.up).normalized * jumpheight);
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
        }
    }
    void OnGUI()
    {
        if (EnableDebug)
        {
            Rect derp = Rect.zero;
            derp.width = 100;
            derp.height = 100;

            GUI.TextArea(derp, "Velocity \n" + rigidbody.velocity.ToString());
        }
    }
}
