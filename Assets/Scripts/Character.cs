using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : Unit
{
    public float charSpeed = 3.0f;
    public int charLives = 5;
    public float charJumpForce = 15.0f;
    // Start is called before the first frame update
    new private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded = false;
    private Bullet bullet;
    public int Lives {
        get {
            return charLives;
        }
        set {
            if(value < 5) charLives  = value;
            livesBar.Refresh();
        }
    }
    private LivesBar livesBar;
    private CharState State {
        get { return (CharState)animator.GetInteger("State");}
        set { animator.SetInteger("State", (int) value);}
    }
    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
        livesBar = FindObjectOfType<LivesBar>();
    }
    private void FixedUpdate()
    {
        checkGround();
    }

    // Update is called once per frame
    void Update()
    {   
        if(isGrounded) State = CharState.Idle;
        if(Input.GetButton("Horizontal")) Run();
        if(isGrounded && Input.GetButtonDown("Jump")) Jump();
        if(Input.GetButtonDown("Fire1")) Shoot();
    }
    private void Run() {

        if(isGrounded) State = CharState.Walk;
        Vector3 direction = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, charSpeed * Time.deltaTime);
        spriteRenderer.flipX = direction.x < 0.0F;
        
    }
    private void Jump() {

        State = CharState.Jump;
        rigidbody.AddForce(transform.up * charJumpForce, ForceMode2D.Impulse);
        
    }

    private void checkGround() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1F);
        isGrounded = colliders.Length > 1;
    }
    private void Shoot() {
        Vector3 position = transform.position;
        Bullet newBullet =  Instantiate(bullet, position, bullet.transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (spriteRenderer.flipX ? -1.0F : 1.0F );
    }
    
    public override void ReceivedDamage() {
        Lives--;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 9.0F, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.gameObject.GetComponent<Unit>();
        if(unit) ReceivedDamage();
    }

}

public enum CharState {
    Idle,
    Walk,
    Jump
}

