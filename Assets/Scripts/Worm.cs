﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Worm : MonoBehaviour
{
    public float maxLength;
    public float speed;
    public float health;
    public Transform body;
    public Transform head;
    public Transform butt;
    public Sprite[] bodySprites;
    public bool isDead;
    public bool active;
    public bool onTurtle;
    //public int mysticFlowerPieces;

    // Wormy UI
    public Text note;
    public Color colorGood;
    public Color colorNeutral;
    public Color colorBad;
    public Color colorWater;
    public GameObject[] panels;
    public Text mysticFlowerText;
    public GameObject butterfly;

    [HideInInspector]
    public float length;

    //private LineRenderer line;
    // Offset for Camera
    private Vector3 offset;
    private bool flipped;

    // Sprite Renderers
    private SpriteRenderer bodySR;
    private SpriteRenderer headSR;
    private SpriteRenderer buttSR;

    // Movers
    private Mover headMover;
    private Mover buttMover;

    // Turtle Riding Stuff
    private Transform turtle;
    private Vector3 bodyOffset;
    private Vector3 headOffset;
    private Vector3 buttOffset;

    // Cocoon Spawner
    private Checkpoints checkpoints;
    private Transform cocoon;

    // Start is called before the first frame update
    void Start()
    {
        active = true;
        offset = Camera.main.transform.position - body.position;
        GoToCheckpoint();
        bodySR = body.gameObject.GetComponent<SpriteRenderer>();
        headSR = head.gameObject.GetComponent<SpriteRenderer>();
        buttSR = butt.gameObject.GetComponent<SpriteRenderer>();

        headMover = head.gameObject.GetComponent<Mover>();
        buttMover = butt.gameObject.GetComponent<Mover>();

        foreach(GameObject panel in panels) 
            panel.SetActive(true);

        //DisplayMessage("Twinchworm, Let's Go!", colorGood);
        UpdateFlowerText();
        //line = GetComponent<LineRenderer>();

        butterfly.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        body.position = Vector3.Lerp(butt.position, head.position, 0.5f);
        body.rotation = Quaternion.FromToRotation(Vector3.right, head.position - body.position);

        length = Vector3.Magnitude(butt.position - head.position);
        float bodyScale = Mathf.Clamp(length - 0.5f, 0.7f, 3.0f);
        body.localScale = new Vector3(bodyScale, body.localScale.y, 1.0f);

        if (length > 2.35f) bodySR.sprite = bodySprites[0];
        else if (length > 2.175f) bodySR.sprite = bodySprites[1];
        else if (length > 2.0f) bodySR.sprite = bodySprites[2];
        else if (length > 1.825f) bodySR.sprite = bodySprites[3];
        else if (length > 1.65f) bodySR.sprite = bodySprites[4];
        else if (length > 1.475f) bodySR.sprite = bodySprites[5];
        else if (length > 1.3f) bodySR.sprite = bodySprites[6];
        else bodySR.sprite = bodySprites[7];

        // Attempt to do this automatically
        //float scaledLength = (bodyScale - 1.2f) * (2.4f - 1.2f) * 8.0f;
        //Debug.Log("ScaledLength: " + scaledLength);
        //if (scaledLength > 7) bodySR.sprite = bodySprites[0];
        //else if (scaledLength > 6) bodySR.sprite = bodySprites[1];
        //else if (scaledLength > 5) bodySR.sprite = bodySprites[2];
        //else if (scaledLength > 4) bodySR.sprite = bodySprites[3];
        //else if (scaledLength > 3) bodySR.sprite = bodySprites[4];
        //else if (scaledLength > 2) bodySR.sprite = bodySprites[5];
        //else if (scaledLength > 1) bodySR.sprite = bodySprites[6];
        //else bodySR.sprite = bodySprites[7];

        if (!flipped && butt.position.x > head.position.x)
        {
            flipped = true;
            body.localScale = new Vector3(bodyScale, -1.0f, 1.0f);
            head.localScale = new Vector3(1.0f, -1.0f, 1.0f);
            butt.localScale = new Vector3(1.0f, -1.0f, 1.0f);
        }
        else if (flipped && butt.position.x < head.position.x)
        {
            flipped = false;
            body.localScale = new Vector3(bodyScale, 1.0f, 1.0f);
            head.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            butt.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //if (onTurtle)
        //{
        //    RideTurtle();
        //}
        //Debug.Log("Worm Length: " + length + " | Worm Sprite: " + bodySR.sprite.name);

    }

    public void Die(string message, Color messageColor)
    {
        isDead = true;
        active = false;

        head.GetComponent<Mover>().Die();
        butt.GetComponent<Mover>().Die();

        // Tint Worm sprites
        bodySR.color = messageColor;
        headSR.color = messageColor;
        buttSR.color = messageColor;

        DisplayMessage(message, messageColor);

        // Respawn with delay
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DisplayMessage(string message, Color messageColor)
    {
        StopCoroutine("TextDelay");
        note.color = messageColor;
        note.text = message;
        StartCoroutine("TextDelay");
    }

    private IEnumerator TextDelay()
    {
        yield return new WaitForSeconds(5);
        note.text = "";
    }

    void LateUpdate()
    {
        if (!isDead)
        {
            Camera.main.transform.position = body.position + offset;
        }
    }

    public void CarriedAway(Transform bird)
    {
        body.parent = bird;
        head.parent = bird;
        butt.parent = bird;

        //body.GetComponent<Rigidbody2D>().isKinematic = true;
        head.GetComponent<Rigidbody2D>().isKinematic = true;
        butt.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void GetOnTurtle(Transform turtleTransform)
    {
        onTurtle = true;
    }

    public void GetOffTurtle()
    {
        if (!headMover.onTurtle && !buttMover.onTurtle)
        {
            onTurtle = false;
        }
    }

    void RideTurtle()
    {
        //if (turtle != null && turtle.GetComponent<Turtle>().moving)
        //{
        //    body.position = turtle.position + bodyOffset;
        //    head.position = turtle.position + headOffset;
        //    butt.position = turtle.position + buttOffset;
        //}
    }

    public void CollectFlower(Transform flowerTransform)
    {
        checkpoints.mysticFlowerPieces++;
        cocoon.transform.position = flowerTransform.position;
        UpdateFlowerText();
        if (checkpoints.mysticFlowerPieces >= checkpoints.mysticFlowersTotal)
        {
            Butterfly();
        }
    }

    void UpdateFlowerText()
    {
        string flowersCurrent = checkpoints.mysticFlowerPieces.ToString();
        string flowersTotal = checkpoints.mysticFlowersTotal.ToString();
        mysticFlowerText.text = "Mystical Flower Pieces: " + flowersCurrent + "/" + flowersTotal;
    }
     
    void Butterfly()
    {
        DisplayMessage(checkpoints.victoryMessage, colorGood);
        bodySR.enabled = false;
        headSR.enabled = false;
        buttSR.enabled = false;
        active = false;
        butt.gameObject.SetActive(false);
        head.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        butterfly.transform.position = body.position;
        butterfly.SetActive(true);
        butterfly.GetComponent<Rigidbody2D>().velocity = Vector2.up / 1.5f;
        StartCoroutine(RestartWithDelay());
    }

    IEnumerator RestartWithDelay()
    {
        int nextScene = checkpoints.nextScene;
        yield return new WaitForSeconds(8);
        Destroy(checkpoints.gameObject);
        SceneManager.LoadScene(nextScene);
    }

    void GoToCheckpoint()
    {
        checkpoints = GameObject.FindWithTag("Checkpoints").GetComponent<Checkpoints>();
        cocoon = GameObject.FindWithTag("Cocoon").transform;
        if (cocoon != null)
        {
            body.position += cocoon.position;
            head.position += cocoon.position;
            butt.position += cocoon.position;
        }
    }
}
