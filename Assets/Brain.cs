using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Brain : MonoBehaviour {

    private int DNAlength = 5;
    public DNA dna;
    public GameObject eyes;
    bool seeDownWall = false;
    bool seeUpWall = false;
    bool seeBottomWall = false;
    bool seeTopWall = false;
    public float spawnSpread = 1.5f;

    Vector3 startPosition;
    public float timeAlive = 0;
    public float distanceTraveled = 0;
    public int crash = 0;
    bool alive = true;
    Rigidbody2D rb;

    private float timeStarted;

    public void Init()
    {
        dna = new DNA(DNAlength, 200);
        this.transform.Translate(Random.Range(-spawnSpread, spawnSpread), Random.Range(-spawnSpread, spawnSpread), 0);
        startPosition = this.transform.position;
        rb = this.GetComponent<Rigidbody2D>();
        timeStarted = Time.time;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "dead")
        {
            alive = false;
            timeAlive = Time.time - timeStarted;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "topWall" ||
            collision.gameObject.tag == "bottomWall" ||
            collision.gameObject.tag == "top" ||
            collision.gameObject.tag == "bottom")
        {
            crash++;
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("finish line"))
        {
            Debug.Log(this.name);
            timeStarted--;
        }
    }

    private void Update()
    {
        if (!alive) return;

        seeUpWall = seeTopWall = seeBottomWall = seeDownWall = false;

        RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.forward, 3.0f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("topWall"))
            {
                seeUpWall = true;
            }
            else if (hit.collider.gameObject.CompareTag("bottomWall"))
            {
                seeDownWall = true;
            }
        }

        hit = Physics2D.Raycast(eyes.transform.position, eyes.transform.up, 1.0f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("top"))
            {
                seeTopWall = true;
            }
        }

        hit = Physics2D.Raycast(eyes.transform.position, -eyes.transform.up, 1.0f);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("bottom"))
            {
                seeBottomWall = true;
            }
        }

        //timeAlive = PopulationManager.elapsed; 
    }

    void FixedUpdate()
    {
        if (!alive) return;

        float upforce = 0;
        float forwardForce = 1.0f;

        if (seeUpWall)
        {
            upforce = dna.GetGene(0);
        }
        else if (seeDownWall)
        {
            upforce = dna.GetGene(1);
        }
        else if (seeTopWall)
        {
            upforce = dna.GetGene(2);
        }
        else if (seeBottomWall)
        {
            upforce = dna.GetGene(3);
        }
        else
        {
            upforce = dna.GetGene(4);
        }
        rb.AddForce(this.transform.right * forwardForce);
        rb.AddForce(this.transform.up * upforce * 0.1f);
        distanceTraveled = Vector3.Distance(startPosition, this.transform.position);
    }
}
