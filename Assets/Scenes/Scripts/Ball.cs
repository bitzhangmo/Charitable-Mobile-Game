using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    [Header("小球属性")]
    [Tooltip("等级")]
    public int level = 1;
    [Tooltip("生命值")]
    public float Life = 10;
    [Tooltip("攻击力")]
    public float attack = 3;
    [Tooltip("最小速度:低于该速度停止")]
    public float minSpeed = 0.05f;
    [Tooltip("当前速度")]
    public float speed = 0f;
    [Tooltip("可移动次数")]
    public int moveCount = 0;       // 1级球2次，2级球4次，3级球6次。

    [Tooltip("是否被选中")]
    public bool isChosen = false;
    [Tooltip("是否可以移动")]
    public bool canMove = false;
    [Tooltip("字符")]
    public string myName;
    [Tooltip("是否是诗里的字")]
    public bool isTargetBall = false;
    private bool reachLimit = false;
    public bool hasSendMessage = false;
    [Header("组件相关")]
    public GameManager gm;
    private Rigidbody2D rb;
    private Transform word;
    private TextMesh textMesh;
    private GameObject canvas;
    private GameObject bar;
    public Sprite sprite0;
    public Sprite sprite1;
    public Sprite sprite2;
    private ShakeCamera shakeCamera;
    private float rgb = 0;
    
    public float m_Radius = 1; // 圆环的半径
    public float m_Theta = 0.1f; // 值越低圆环越平滑
    public Color m_Color = Color.green; // 线框颜色

    public AudioClip[] clips;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        word = this.transform.GetChild(0);
        textMesh = word.gameObject.GetComponent<TextMesh>();
        textMesh.text = myName;
        canvas = this.transform.GetChild(1).gameObject;
        bar = canvas.transform.GetChild(1).gameObject;
        GameObject obj = GameObject.Find("MainCamera");
        shakeCamera = obj.GetComponent<ShakeCamera>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
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
            isChosen = false;
            canvas.SetActive(true);
            bar.GetComponent<Image>().fillAmount = this.Life / 4.0f;
        }

    }

    public void SetBall()
    {
        Start();
        switch(this.level)
        {
            case 1:
            {
                this.moveCount = 2;
                this.rb.drag = 0.2f;
                this.attack = 1;
                // this.gameObject.GetComponent<SpriteRenderer>().sprite = ;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite0;
                break;
            }
            case 2:
            {
                this.moveCount = 4;
                this.rb.drag = 0.15f;
                this.attack = 1.5f;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
                break;
            }
            case 3:
            {
                this.moveCount = 6;
                this.rb.drag = 0.1f;
                this.attack = 3;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag != "shootpoint")
        {
            shakeCamera.enabled = true;
        }
        
        if (other.gameObject.tag == "Ball")
        {
            if(this.isChosen)
            {
                GameObject[] balls = new GameObject[2];
                balls[0] = this.gameObject;
                balls[1] = other.gameObject;
                if(!other.gameObject.GetComponent<Ball>().isTargetBall)
                {
                    // Debug.Log(gm.gameObject);
                    // gm.gameObject.SendMessage("mixWord",balls);
                    SendMessageToOtherBall(other.gameObject, balls);
                }
            }


            // Debug.Log("hello");
            if(this.isChosen)
            {
                if(other.gameObject.GetComponent<Ball>().isTargetBall && gm.processIndex == 2)  // 当处于阶段2时才可以造成伤害
                {   
                    OnPlayAudio();
                    other.gameObject.SendMessage("TakeDamage",attack);
                }
            }
        }
    }

    public void SendMessageToOtherBall(GameObject otherObj, GameObject[] balls)
    {
        if(!hasSendMessage)
        {
            otherObj.SendMessage("PostEvent", balls);
            otherObj.GetComponent<Ball>().hasSendMessage = true;
        }
    }

    public void PostEvent(GameObject[] balls)
    {
        gm.gameObject.SendMessage("mixWord", balls);
    }


    public void OnPlayAudio()
    {
        audioSource.clip = clips[level - 1];
        audioSource.Play(0);
    }

    public void TakeDamage(int attack)
    {
        Debug.Log("TakeDamage");
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
            gm.AliveBalls.Remove(this.gameObject);
            GameObject.Destroy(this.gameObject);
            // this.transform.gameObject.Destory();
        }
    }

    public void CheckSpeed()
    {
        // Debug.Log(rb.velocity.magnitude);
        if(rb.velocity.magnitude > 0)
        {
            if(rb.velocity.magnitude < minSpeed)
            {
                if(isChosen)
                {
                    isChosen = false;
                    hasSendMessage = false;
                }
                speed = rb.velocity.magnitude;
                rb.velocity = new Vector2(0, 0);
            }
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
        
        // this.gameObject.GetComponent<SpriteRenderer>().color = new Color(rgb,rgb,rgb,rgb);
        // this.gameObject.GetComponent<SpriteRenderer>().color.a = rgb;
        
        if(isChosen)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255,255,255,rgb);
        }
    }

}
