using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public string horizontalAxis;   // Input axis for horiz movement
    public string verticalAxis;     // Input axis for vertical movement

    public Worm worm;               // Reference to Worm component of WormBody game object
    public GameObject otherEnd;
    public bool isMoving;
    public bool onTurtle;
    public bool anchoredToTurtle;
    public Joint2D joint;

    private Mover otherMover;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private Transform turtle;
    private Vector3 turtleOffset;

    // Start is called before the first frame update
    void Start()
    {
        otherMover = otherEnd.GetComponent<Mover>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //joint = GetComponent<Joint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!worm.isDead)
        {
            if (onTurtle)
            {
                if (turtle.GetComponent<Turtle>().moving)
                {
                    rb.position = turtle.position + turtleOffset;
                }
                //otherEnd.transform.position = turtle.position + turtleOffset;
                else
                {
                    Move();
                    turtleOffset = transform.position - turtle.position;
                }
            }
            else { // If not on turtle
                onTurtle = false;
                Move();
            }

        }

    }

    void Move()
    {
        float h = Input.GetAxisRaw(horizontalAxis);
        float v = Input.GetAxisRaw(verticalAxis);
        //Vector3 move = Vector3.ClampMagnitude(new Vector3(h, v), 1.0f);

        if (!otherMover.isMoving && (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0))
        {
            Vector3 move = Vector3.ClampMagnitude(new Vector3(h, v), 1.0f);
            isMoving = true;
            anim.Play("Walk");
            rb.mass = 1;
            Vector3 targetPosition = transform.position + move * worm.speed * Time.deltaTime;
            //if (Vector3.SqrMagnitude(targetPosition - otherEnd.transform.position) > worm.length * worm.length)
            //{
            //    targetPosition = otherEnd.transform.position + Vector3.ClampMagnitude(otherEnd.transform.position - targetPosition, worm.length);

            //}
            //transform.position = targetPosition;
            rb.velocity = move * worm.speed;
            //rb.MovePosition(targetPosition);
        }
        else
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
            if (!otherMover.anchoredToTurtle)
                rb.mass = 100;
            anim.Play("Idle");
        }
    }

    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    if (!worm.isDead && !isMoving && other.CompareTag("Water") )
    //    {
    //        worm.Die("You Have Drowned!", worm.colorWater);
    //    }
    //}

    private void LateUpdate()
    {
        transform.rotation = worm.body.rotation;
    }

    public void Die()
    {
        anim.Play("Sad");
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0.0f;
    }


    public void OnTurtle(Transform t)
    {
        if (!otherMover.onTurtle)
        {
            onTurtle = true;
            turtle = t;
            //turtleOffset = transform.position - turtle.position;
        }
    }

    public void OffTurtle()
    {
        onTurtle = false;
        turtle = null;
    }


    //public void AnchorToTurtle()
    //{
    //    //joint.enabled = true;
    //    //joint.connectedBody = turtleRB;
    //    anchoredToTurtle = true;
    //    turtleOffset = transform.position - turtle.position;
    //}

    //public void DeAnchor()
    //{
    //    //joint.enabled = false;
    //    //joint.connectedBody = null;
    //    onTurtle = false;
    //    turtle = null;
    //}

}