using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public GameManager gm;
    public bool isChosen = false;
    public int Life = 10;
    public int attack = 3;
    public string myName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Ball")
        {
            Debug.Log("hello");
            other.gameObject.SendMessage("TakeDamage",attack);
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
}
