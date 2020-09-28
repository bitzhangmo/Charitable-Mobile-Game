using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Ball : MonoBehaviour
{
    [Header("小球属性")]
    public int Life = 10;
    public int attack = 3;
    public float minSpeed = 0.05f;
    public float speed = 0f;
    [Header("组件相关")]
    public GameManager gm;
    public bool isChosen = false;
    public string myName;
    private Rigidbody2D rb;
    public bool canMove = false;
    private Transform word;
    private TextMesh textMesh;
    private GameObject canvas;
    private GameObject bar;
    public bool isTargetBall = false;
    public bool reachLimit = false;
    public float rgb = 0;

    public Ball(int attack,int life,float bouns)
    {
        this.attack = attack;
        this.Life = life;
        // this.bouns = bouns;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        
        word = this.transform.GetChild(0);
        textMesh = word.gameObject.GetComponent<TextMesh>();
        textMesh.text = myName;

        canvas = this.transform.GetChild(1).gameObject;
        bar = canvas.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();
        CheckSpeed();
        if(canMove && !isTargetBall)
        {
            ChangeColor();
        }
        if(isTargetBall)
        {
            canvas.SetActive(true);
            bar.GetComponent<Image>().fillAmount = (float)this.Life / 4.0f;
        }
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


            // Debug.Log("hello");
            if(this.isChosen)
            {
                if(other.gameObject.GetComponent<Ball>().isTargetBall && gm.processIndex == 1)  // 当处于阶段2时才可以造成伤害
                {   
                    other.gameObject.SendMessage("TakeDamage",attack);
                }
            }
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
            gm.gameObject.SendMessage("UpdateTopText",myName);
            gm.gameObject.SendMessage("RemoveWordCount",myName);
            GameObject.Destroy(this.gameObject);
            // this.transform.gameObject.Destory();
        }
    }

    public void CheckSpeed()
    {
        // Debug.Log(rb.velocity.magnitude);
        if(rb.velocity.magnitude < minSpeed)
        {
            speed = rb.velocity.magnitude;
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
    }

    public void ChangeColor()
    {
        
        if(rgb > 1)
        {
            reachLimit = true;
        }
        else if(rgb < 0)
        {
            reachLimit = false;
        }

        if(reachLimit)
        {
            rgb -= Time.deltaTime;
        }
        else
        {
            rgb += Time.deltaTime;
        }
        
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(rgb,rgb,rgb,rgb);
    }
}
