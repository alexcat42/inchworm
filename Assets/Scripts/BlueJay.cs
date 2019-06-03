using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueJay : MonoBehaviour
{
    public float speed;
    public float flySpeed;
    public bool faceRight;

    private Worm worm;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool attacking;

    void Start()
    {
        worm = GameObject.FindWithTag("Player").GetComponent<Worm>();
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
        // Patrol unless in range of player

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TallGrass"))
        {
            speed *= -1;
            rb.velocity = new Vector2(speed, 0.0f);
            sr.flipX = !sr.flipX;
        }

        if (other.CompareTag("Player") && !attacking)
        {
            attacking = true;
            anim.Play("Attack");
            rb.velocity = Vector3.zero;
            worm.Die("A Blue Jay Ate You!", worm.colorBad);
            StartCoroutine(Fly());
        }
    }

    IEnumerator Fly()
    {
        yield return new WaitForSeconds(1);

        GameObject.Find("WormBody").transform.parent = transform;
        GameObject.Find("WormHead").transform.parent = transform;
        GameObject.Find("WormButt").transform.parent = transform;

        if (!sr.flipX) flySpeed *= -1;
        rb.velocity = new Vector2(flySpeed, 0.0f);

        anim.Play("Flap");

    }
}
