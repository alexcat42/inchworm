using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocoon : MonoBehaviour
{

    // Thanks to Blackthorn Productions for checkpoint script
    private static Cocoon cocoonSpawner;

    // Start is called before the first frame update
    void Awake()
    {
        if (cocoonSpawner == null)
        {
            cocoonSpawner = this;
            DontDestroyOnLoad(cocoonSpawner);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
