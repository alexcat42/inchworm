using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BluejayState
{
    patrolling,
    attacking,
    flying
}

public class BlueJay : MonoBehaviour
{
    public BluejayState state;
    private GameObject worm;
    private Worm wormScript;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //worm = GameObject.FindWithTag("Player");
        //wormScript = worm.GetComponent<Worm>();
        anim = GetComponent<Animator>();
        state = BluejayState.patrolling;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == BluejayState.patrolling)
            Patrol();

        else if (state == BluejayState.attacking)
            Attack();

        // Fly
    }

    void Patrol()
    {
        anim.Play("Walk");
    }

    public void Attack()
    {

        anim.Play("Attack");
    }


}
