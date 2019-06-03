using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBlock : MonoBehaviour
{
    public string message;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
