using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBlock : MonoBehaviour
{
    public string message;

    [HideInInspector]
    public bool muted;

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Mute()
    {
        if (!muted)
            StartCoroutine(DelayToUnmute());
    }

    IEnumerator DelayToUnmute()
    {
        muted = true;
        yield return new WaitForSeconds(10);
        muted = false;
    }
}
