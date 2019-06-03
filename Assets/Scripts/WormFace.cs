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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water") && !worm.isDead && !mover.isMoving)
        {
            worm.Die("You Have Drowned!", worm.colorWater);
        }
    }
}
