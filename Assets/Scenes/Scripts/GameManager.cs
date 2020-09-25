using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("输入相关")]
    public bool isMouseDown = false;
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 direction;

    
    GameObject target;
    [Header("场景数据")]
    public int strsIndex = 0;
    public int strsRealIndex = 0;
    public int repeatCount = 0;
    public int chooseIndex = 0;
    public float percent = 0.0f;
    public float maxDistance = 5.0f;
    public float initTime = 5.0f;
    public float force = 5.0f;

    public GameObject prefab;
    public GameObject p_Scroll;
    public GameObject scroll;
    public Transform shootPoint;
    public LineRenderer lineRenderer;
    public Text topText;
    
    [Header("音频相关")]
    public AudioClip ac;
    private AudioSource audioSource;

    [Header("关卡数据")]
    public Ball chosenBall;
    public string poem = "花落知多少";
    public string[] strs = {"矢","少","洛","小","化","夕","口","丿","十","火","一","丁","木","办","林","日","艹"};
    public string[] strsReal = {"矢","洛","化","夕","口","艹"};
    
    // private List<string> uiText = new List<string>();
    private string uiText = "";
    public Transform[] initPos;
    // public Ball[] balls;
    public List<Ball> balls;
    public float timer = 2.0f;
    // private string l = "落";
    [SerializeField]
    public Dictionary<string,List<string>> levelRule = new Dictionary<string, List<string>>();
    // public Dictionary<string,string[]> levelRule = new Dictionary<string, string[]>();
    public Dictionary<string,string> doubleRule = new Dictionary<string, string>();
    public string[] levelPath;

    [Header("GM开关")]
    public bool isUseDisturb = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        ReadLevelPathFile();
        ReadDoubleFile();
        ReadFile();
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
        if(balls.Count == 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                InitBall();
                timer = 2.0f;
            }
        }
        if(repeatCount > 2)
        {
            Time.timeScale = 0;
        }
        if(balls.Count > 0)
        {
            balls[0].canMove = true;
        }
        
    }


    void UserInput()
    {
        // balls[0].canMove = true;
        if(Input.GetMouseButtonDown(0))
        {
            if(!isMouseDown)
            {
                startPos = Input.mousePosition;
            }
            isMouseDown = true;
            

            if (chosenBall != null)
            {
                chosenBall.isChosen = false;
            }

            Ray ray2D = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray2D.origin.x, ray2D.origin.y), Vector2.zero);

            if(hit.collider)
            {
                if(hit.transform.gameObject.tag == "Ball")
                {
                    target = hit.transform.gameObject;
                    chosenBall = target.GetComponent<Ball>();

                    lineRenderer = target.GetComponent<LineRenderer>();
                    lineRenderer.SetVertexCount(2);
                    
                    
                    if(chosenBall.canMove)
                    {
                        chosenBall.isChosen = true;
                    }
                }
            }


            // initScroll(new Vector3(ray2D.origin.x,ray2D.origin.y,0));
        }
        if(Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;

            endPos = Input.mousePosition;
            percent = Vector2.Distance(startPos,endPos)/maxDistance;

            direction = startPos - endPos;
            direction.Normalize();
            
            Vector3 line_end = (startPos + direction)*5;
            lineRenderer.SetPosition(0, new Vector3(startPos.x,startPos.y,-0.3f));
            lineRenderer.SetPosition(1,line_end);
            
            if(target != null && chosenBall.canMove)
            {
                PushTarget(target, direction, percent);
            }

            if(balls.Contains(chosenBall))
            {
                balls.Remove(chosenBall);
            }
            chosenBall.canMove = false;
            chosenBall.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            // ReleaseScroll();
        }

        // if(isMouseDown)
        // {
        //     percent = Vector2.Distance(startPos,endPos);
        // }
    }

    void PushTarget(GameObject target,Vector2 direction,float percent)
    {
        Rigidbody2D rb2D = target.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction*percent);
    }

    // 读取数据表并生成依据
    void ReadFile()
    {
        TextAsset txt = Resources.Load("level0") as TextAsset;
        //Debug.Log(txt);

        string[] str = txt.text.Split('\n');
        Debug.Log(str);
        foreach(string strs in str)
        {
            string[] ss = strs.Split(',');
            string key = ss[0];
            List<string> result = new List<string>();
            // string[] result = new string[ss.Length - 1];
            for (int i = 1; i < ss.Length; i++)
            {
                // result.Insert(i - 1,ss[i]);
                result.Add(ss[i]);
                // result[i - 1] = ss[i];
            }
            levelRule.Add(key,result);
        }        
    }

    public void ReadFileByIndex(int index)
    {
        
    }

    // 关卡规则文件名
    public void ReadLevelPathFile()
    {
        TextAsset pathFile = Resources.Load("levelPath") as TextAsset;

        levelPath = pathFile.text.Split('\n');
    }

    // 同字合成表
    public void ReadDoubleFile()
    {
        TextAsset doubleFile = Resources.Load("double") as TextAsset;

        string[] strs = doubleFile.text.Split('\n');
        foreach(string str in strs)
        {
            string[] rule = str.Split(',');
            string key = rule[0];
            string target = rule[1];
            doubleRule.Add(key,target);
        }
    }

    void InitBall()
    {
        for(int i = 0; i < 3; i++)
        {
            // int randomIndex = UnityEngine.Random.Range(0,strs.Length);
            GameObject initObject = GameObject.Instantiate(prefab, initPos[i].position, Quaternion.identity);
            Ball tempBall = initObject.GetComponent<Ball>();
            balls.Add(tempBall);
            // tempBall.canMove = true;
            if(isUseDisturb)
            {
                tempBall.myName = strs[strsIndex];
                strsIndex++;
                if(strsIndex == strs.Length)
                {
                    strsIndex = 0;
                    repeatCount++;
                }
            }
            else
            {
                tempBall.myName = strsReal[strsRealIndex];
                strsRealIndex++;
                if(strsRealIndex == strsReal.Length)
                {
                    strsRealIndex = 0;
                    repeatCount++;
                }
            }

            tempBall.gm = this;
        }
    }

    public void ChooseBall()
    {

    }

    private string CheckBall(string myName, string otherName)
    {
        if(levelRule.ContainsKey(myName) && levelRule.ContainsKey(otherName))
        {
            List<string> list1 = levelRule[myName];
            List<string> list2 = levelRule[otherName];
            // string[] list1 = levelRule[myName];
            // string[] list2 = levelRule[otherName];

            // 检查同字情况
            if(myName == otherName)
            {
                return CheckBallInDoubleFile(myName);
            }

            foreach(string item in list1)
            {
                if(list2.Contains(item))
                {
                    return item;
                }
            }
        }

        return "";
    }

    private string CheckBallInDoubleFile(string key)
    {
        if(doubleRule.ContainsKey(key))
        {
            return doubleRule[key];
        }
        return "";
    }

    public void mixWord(GameObject[] balls)
    {
        // Debug.Log("mix");
        Ball ball0 = balls[0].GetComponent<Ball>();
        Ball ball1 = balls[1].GetComponent<Ball>();

        string targetWord = CheckBall(ball0.myName, ball1.myName);
        Vector3 pos = ball1.transform.position;
        if(!targetWord.Equals(""))
        {
            ball0.Destroy();
            ball1.Destroy();
            GameObject newObject = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            Ball newball = newObject.GetComponent<Ball>();
            newball.myName = targetWord;
            newball.gm = this;
            audioSource.PlayOneShot(ac, 1F);
            if(poem.Contains(targetWord))
            {
                newObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                newObject.GetComponent<SpriteRenderer>().color = Color.red;
                newball.isTargetBall = true;
            }
            else
            {
                newball.canMove = true;
            }

        }
    }

    public void UpdateTopText(string name)
    {
        // int index = checkWordIndex(name);
        // // uiText[index*2] = name;
        // uiText.Insert(index * 2,name);
        // uiText.Insert(index + 1," ");
        // string txt = uiText.ToString();
        uiText += name;
        topText.text = uiText;
    }

    private int checkWordIndex(string name)
    {
        string[] poemArray = poem.Split();
        for(int i = 0; i < poemArray.Length; i++)
        {
            if(name == poemArray[i])
            {
                return i;
            }
        }
        return -1;
    }
    private void initScroll(Vector3 clickPos)
    {
        scroll = GameObject.Instantiate(p_Scroll, clickPos, Quaternion.identity);
    }

    private void ReleaseScroll()
    {
        GameObject.Destroy(scroll);
    }
}
