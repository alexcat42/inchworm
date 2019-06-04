using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    public int nextScene;
    public string victoryMessage;

    [HideInInspector]
    public int mysticFlowerPieces;
    [HideInInspector]
    public int mysticFlowersTotal;
    // Thanks to Blackthorn Productions for checkpoint script
    private static Checkpoints instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            mysticFlowersTotal = GameObject.FindGameObjectsWithTag("MysticFlower").Length;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
