using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormFace : MonoBehaviour
{
    private Worm worm;
    private Mover mover;

    void Start()
    {
        worm = GetComponentInParent<Worm>();
        mover = GetComponentInParent<Mover>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Turtle"))
        {
            mover.Anchor(other.GetComponent<Rigidbody2D>());
            worm.GetOnTurtle(other.transform);
        }

        if (other.CompareTag("MysticFlower"))
        {
            worm.CollectFlower(other.transform);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Message"))
        {
            MessageBlock mb = other.GetComponent<MessageBlock>();
            if (!mb.muted)
            {
                worm.DisplayMessage(mb.message, worm.colorGood);
                mb.Mute();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Turtle"))
        {
            mover.DeAnchor();
            worm.GetOffTurtle();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (!worm.isDead && !worm.onTurtle && !mover.isMoving)
                worm.Die("You Have Drowned!", worm.colorWater);
        }
    }

}
