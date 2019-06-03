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
            mover.onTurtle = true;
            worm.GetOnTurtle(other.transform);
        }

        if (other.CompareTag("MysticFlower"))
        {
            worm.CollectFlower(other.transform);
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Message"))
        {
            string message = other.GetComponent<MessageBlock>().message;
            worm.DisplayMessage(message, worm.colorGood);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Turtle"))
        {
            mover.onTurtle = false;
            worm.GetOffTurtle();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            if (!worm.isDead && !mover.onTurtle && !mover.isMoving)
                worm.Die("You Have Drowned!", worm.colorWater);
        }
    }

}
