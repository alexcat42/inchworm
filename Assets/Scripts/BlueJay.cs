using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueJay : MonoBehaviour
{
    public float speed;
    //public float flySpeed;
    public bool faceRight;

    private Worm worm;
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool attacking;

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
        // Patrol unless in range of player

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!attacking)
        {
            if (other.CompareTag("TallGrass"))
            {
                speed *= -1;
                rb.velocity = new Vector2(speed, 0.0f);
                //sr.flipX = !sr.flipX;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            if (other.CompareTag("Player"))
            {
                worm = other.GetComponentInParent<Worm>();
                if (worm.active)
                {
                    attacking = true;
                    anim.Play("Attack");
                    rb.velocity = Vector3.zero;
                    if (worm != null)
                        worm.Die("Oh no! A Blue Jay ate Twinchworm!", worm.colorBad);
                    StartCoroutine(Fly());
                }
            }
        }
    }

    IEnumerator Fly()
    {
        yield return new WaitForSeconds(1);
        worm.CarriedAway(transform);
        GetComponent<PolygonCollider2D>().enabled = false;
        rb.velocity = new Vector2(speed * 2, 0.0f);
        anim.Play("Flap");

    }
}
