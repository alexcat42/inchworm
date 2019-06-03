using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour
{
    public float speed;
    public float delayAtBank;
    public bool faceRight;

    private Worm worm;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private bool carryingWorm;
    private bool moving;

    void Start()
    {
        anim = GetComponent<Animator>();
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
        anim.Play("Walk");
    }

    void Update()
    {
        // Patrol from bank to bank
        // moving = true -> move from bank to bank
        // when it arrives at grass, flip around and wait for delayAtBank seconds (moving = false)

        // If player head/butt enters, then carryingWorm = true
        // If other player segment enters, then moving = true, and keep going


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            speed *= -1;
            rb.velocity = new Vector2(speed, 0.0f);
            sr.flipX = !sr.flipX;
        }

        if (other.CompareTag("Player"))
        {
            carryingWorm = true;
            worm = other.GetComponentInParent<Worm>();
            anim.Play("Attack");
            rb.velocity = Vector3.zero;
            //worm.Die("A Blue Jay Ate You!", worm.colorBad);
           //StartCoroutine(Fly());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        worm.CarriedAway(transform);
        rb.velocity = new Vector2(speed * 2, 0.0f);
        anim.Play("Flap");

    }
}
