using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float speed;
    public float delayAtBank;
    public bool faceRight;

    private Worm worm;
    //private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private bool carryingWorm;
    private bool moving;
    private bool waiting;

    void Start()
    {
        //anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (faceRight)
        {
            sr.flipX = true;
            speed = Mathf.Abs(speed);
        }
        else
        {
            sr.flipX = false;
            speed = -Mathf.Abs(speed);
        }

        rb.velocity = new Vector2(speed, 0.0f);
        moving = true;
    }

    void Update()
    {
        // Patrol from bank to bank
        // moving = true -> move from bank to bank
        // when it arrives at grass, flip around and wait for delayAtBank seconds (moving = false)

        // If player head/butt enters, then carryingWorm = true
        // If other player segment enters, then moving = true, and keep going
        // Cancel that! Much easier to just have him patrol!


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (moving && !other.CompareTag("Water"))
        {
            StartCoroutine(WaitAtBank());
        }

        //else if (waiting && other.CompareTag("Player"))
        //{
        //    carryingWorm = true;
        //    worm = other.GetComponentInParent<Worm>();
        //    worm.RideTurtle(transform);
        //    //anim.Play("Attack");
        //    rb.velocity = Vector3.zero;
        //    //worm.Die("A Blue Jay Ate You!", worm.colorBad);
        //    //StartCoroutine(Fly());
        //}

    }

    IEnumerator WaitAtBank()
    {
        waiting = true;
        moving = false;

        // Stop Turtle Movement
        Vector2 vel = rb.velocity;
        rb.velocity = Vector2.zero;

        // Wait for half the time
        yield return new WaitForSeconds(delayAtBank / 2);

        // Flip Turtle Around
        sr.flipX = !sr.flipX;
        vel.x *= -1;

        // Wait again
        yield return new WaitForSeconds(delayAtBank / 2);

        // Send the turtle on its way
        rb.velocity = vel;
        waiting = false;
        moving = true;
    }
}
