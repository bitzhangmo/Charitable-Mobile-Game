﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public GameManager gm;
    public bool isChosen = false;
    public int Life = 10;
    public int attack = 3;
    public float minSpeed = 0.05f;
    public string myName;
    private Rigidbody2D rb;
    public bool canMove = false;
    private Transform word;
    private TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        word = this.transform.GetChild(0);
        textMesh = word.gameObject.GetComponent<TextMesh>();
        textMesh.text = myName;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();
        CheckSpeed();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ball")
        {
            if(this.isChosen)
            {
                GameObject[] balls = new GameObject[2];
                balls[0] = this.gameObject;
                balls[1] = other.gameObject;
                gm.gameObject.SendMessage("mixWord",balls);
            }


            // // Debug.Log("hello");
            // if(this.isChosen)
            // {
            //     other.gameObject.SendMessage("TakeDamage",attack);
            // }
        }
    }

    public void TakeDamage(int attack)
    {
        if(this.isChosen == false)
        {
            this.Life -= attack;
        }
    }

    public void CheckLife()
    {
        if(this.Life <= 0)
        {
            GameObject.Destroy(this.gameObject);
            // this.transform.gameObject.Destory();
        }
    }

    public void CheckSpeed()
    {
        if(rb.velocity.magnitude < minSpeed)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
}
