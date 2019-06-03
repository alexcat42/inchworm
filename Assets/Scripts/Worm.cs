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

    // Wormy UI
    public Text note;
    public Color colorGood;
    public Color colorNeutral;
    public Color colorBad;
    public Color colorWater;

    [HideInInspector]
    public float length;

    //private LineRenderer line;
    private Vector3 offset;
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
        note.text = "TWINCHWORM, Let's Go!";
        StartCoroutine(TextDelay());
        //line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        body.position = Vector3.Lerp(butt.position, head.position, 0.5f);
        body.rotation = Quaternion.FromToRotation(Vector3.right, head.position - body.position);

        length = Vector3.Magnitude(butt.position - head.position);
        float bodyScale = Mathf.Clamp(length - 0.5f, 0.7f, 3.0f);
        body.localScale = new Vector3(bodyScale, 1.0f, 1.0f);

        if (length > 2.35f) bodySR.sprite = bodySprites[0];
        else if (length > 2.2f) bodySR.sprite = bodySprites[1];
        else if (length > 2.05f) bodySR.sprite = bodySprites[2];
        else if (length > 1.9f) bodySR.sprite = bodySprites[3];
        else if (length > 1.75f) bodySR.sprite = bodySprites[4];
        else if (length > 1.5f) bodySR.sprite = bodySprites[5];
        else bodySR.sprite = bodySprites[6];

        if (butt.position.x > head.position.x)
        {
            bodySR.flipY = true;
            headSR.flipY = true;
            buttSR.flipY = true;
        }
        else
        {
            bodySR.flipY = false;
            headSR.flipY = false;
            buttSR.flipY = false;
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
        Camera.main.transform.position = body.position + offset;
    }
}
