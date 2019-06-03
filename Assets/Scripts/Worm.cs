using System.Collections;
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
    public bool onTurtle;

    // Wormy UI
    public Text note;
    public Color colorGood;
    public Color colorNeutral;
    public Color colorBad;
    public Color colorWater;
    public GameObject panel;

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

    // Start is called before the first frame update
    void Start()
    {
        offset = Camera.main.transform.position - body.position;
        bodySR = body.gameObject.GetComponent<SpriteRenderer>();
        headSR = head.gameObject.GetComponent<SpriteRenderer>();
        buttSR = butt.gameObject.GetComponent<SpriteRenderer>();

        note.color = colorGood;
        note.text = "Twinchworm, Let's Go!";
        StartCoroutine(TextDelay());
        panel.SetActive(true);
        //line = GetComponent<LineRenderer>();
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
            // Flip Over
            flipped = true;
            body.localScale = new Vector3(bodyScale, -1.0f, 1.0f);
            head.localScale = new Vector3(1.0f, -1.0f, 1.0f);
            butt.localScale = new Vector3(1.0f, -1.0f, 1.0f);

            //bodySR.flipY = true;
            //headSR.flipY = true;
            //buttSR.flipY = true;
        }
        else if (flipped && butt.position.x < head.position.x)
        {
            // Flip back to normal
            flipped = false;
            body.localScale = new Vector3(bodyScale, 1.0f, 1.0f);
            head.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            butt.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            //bodySR.flipY = false;
            //headSR.flipY = false;
            //buttSR.flipY = false;
        }


        //Debug.Log("Worm Length: " + length + " | Worm Sprite: " + bodySR.sprite.name);

    }

    public void Die(string message, Color messageColor)
    {
        isDead = true;

        head.GetComponent<Mover>().Die();
        butt.GetComponent<Mover>().Die();

        // Tint Worm sprites
        bodySR.color = messageColor;
        headSR.color = messageColor;
        buttSR.color = messageColor;

        // Display UI
        note.color = messageColor;
        note.text = message;

        // Respawn with delay
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator TextDelay()
    {
        yield return new WaitForSeconds(3);
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

    public void RideTurtle(Transform turtle)
    {
        onTurtle = true;
        //body.parent = turtle;
        //head.parent = turtle;
        //butt.parent = turtle;

    }
}
