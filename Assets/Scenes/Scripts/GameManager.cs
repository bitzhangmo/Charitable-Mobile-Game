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
    public Vector2 MousePos;
    
    GameObject target;
    [Header("场景数据")]
    public int strsIndex = 0;
    public int strsRealIndex = 0;
    public int restPartIndex = 0;
    public int restPartCount = 0;
    // public int repeatCount = 0;
    public int chooseIndex = 0;
    public int levelIndex = 0;
    public float percent = 0.0f;
    public float maxDistance = 5.0f;
    public float initTime = 5.0f;
    public float force = 5.0f;

    public GameObject prefab;
    public GameObject p_Scroll;
    public GameObject scroll;
    public Transform shootPoint;
    // public LineRenderer lineRenderer;
    public Text topText;
    public GameObject win;
    public GameObject lose;
    
    [Header("音频相关")]
    public AudioClip ac;
    private AudioSource audioSource;

    [Header("关卡数据")]
    public Ball chosenBall;
    public string[] poem = {"花","落","知","多","少"};
    public string[] strs = {"矢","少","洛","小","化","夕","口","丿","十","火","一","丁","木","办","林","日","艹"};
    public string[] strsReal = {"化","艹","口","目","丿","少","夕"};
    public string[] levelPath = {"level0/","level1/","level2/"};
    public List<string> restPartStr = new List<string>();
    public Dictionary<string,int> wordCount = new Dictionary<string, int>(); 
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
    public Dictionary<string,List<string>> partRule = new Dictionary<string, List<string>>();
    public List<string> wordList = new List<string>();
    // public string[] levelPath;

    [Header("GM开关")]
    public bool isUseDisturb = true;
    
    [Header("预瞄线")]
    [Range(1,5)]
    public int _maxIterations = 3;
    public float _maxDistance = 10f;

    public int _count;
    public LineRenderer _line;

    [Header("游戏流程")]
    public float gameTimer = 0;
    public bool isInitPartStr = false;
    public int processIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        _line = GetComponent<LineRenderer>();
        // ReadLevelPathFile();
        ReadDoubleFile();
        // ReadFile();
        ReadFileByIndex(0);
        UpdateWord(wordList, true, "艹");
        UpdateWord(wordList, true, "矢");
        UpdateWord(wordList, true, "夕");
        UpdateWord(wordList, true, "洛");
        UpdateWord(wordList, true, "小");
        UpdateWord(wordList, true, "水");
    }

    // Update is called once per frame
    void Update()
    {
        
        // #if UNITY_ANDROID
        UserMobileInput();
        // #else
        // UserInput();
        // #endif
        InitBallByTime();
        if(wordCount.Count >= 5)
        {
            processIndex = 1;
        }
        CheckGameOver(2);
    }


    void UserInput()
    {
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
                    
                    if(chosenBall.canMove)
                    {
                        chosenBall.isChosen = true;
                    }
                }
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;

            endPos = Input.mousePosition;
            percent = Vector2.Distance(startPos,endPos)/maxDistance;

            direction = startPos - endPos;
            direction.Normalize();
            
            Vector3 line_end = (startPos + direction)*5;
            
            if(target != null && chosenBall.canMove)
            {
                PushTarget(target, direction, percent);
            }

            if(balls.Contains(chosenBall))
            {
                balls.Remove(chosenBall);
            }
            // chosenBall.canMove = false;
            chosenBall.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    public void UserMobileInput()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    if (chosenBall != null)
                    {
                        chosenBall.isChosen = false;
                    }
                    startPos = touch.position;
                    Ray ray2D = Camera.main.ScreenPointToRay(startPos);
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(ray2D.origin.x, ray2D.origin.y), Vector2.zero);

                    if(hit.collider)
                    {
                        if(hit.transform.gameObject.tag == "Ball")
                        {
                            target = hit.transform.gameObject;
                            chosenBall = target.GetComponent<Ball>();
                            if(chosenBall.canMove)
                            {
                                chosenBall.isChosen = true;
                            }
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    endPos = touch.position;
                    direction = startPos - endPos;
                    direction.Normalize();
                    percent = Vector2.Distance(startPos,endPos)/maxDistance;
                    break;
                case TouchPhase.Ended:
                    if(percent < 0.1)
                    {
                        break;
                    }

                    if(target != null && chosenBall.canMove)
                    {
                        PushTarget(target, direction, percent);
                        UpdateWord(wordList, true, chosenBall.myName);
                    }
                    
                    if(balls.Contains(chosenBall))
                    {
                        balls.Remove(chosenBall);
                    }
                    // chosenBall.canMove = false;
                    chosenBall.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
            }
        }
    }


    void PushTarget(GameObject target,Vector2 direction,float percent)
    {
        Rigidbody2D rb2D = target.GetComponent<Rigidbody2D>();
        rb2D.AddForce(direction*percent*force);
    }

    // 读取数据表并生成依据
    void ReadFile()
    {
        TextAsset txt = Resources.Load("level0/level0") as TextAsset;

        string[] str = txt.text.Split('\n');
        foreach(string strs in str)
        {
            string[] ss = strs.Split(',');
            string key = ss[0];
            List<string> result = new List<string>();
            for (int i = 1; i < ss.Length; i++)
            {
                result.Add(ss[i]);
            }
            levelRule.Add(key,result);
        }        
    }

    public void ReadFileByIndex(int index)
    {

        TextAsset txt = Resources.Load(levelPath[index] + "level") as TextAsset;
        // Debug.Log(txt);
        string[] strs = txt.text.Split('\n');

        foreach(string line in strs)
        {
            string[] items = line.Split(',');
            string key = items[0];

            List<string> result = new List<string>();
            for(int i = 1; i<items.Length; i++)
            {
                result.Add(items[i]);
            }
            levelRule.Add(key,result);
        }
        
        TextAsset partTxt = Resources.Load(levelPath[index] + "wordpart") as TextAsset;
        Debug.Log(partTxt);
        string[] partStrs = partTxt.text.Split('\n');

        foreach (var item in partStrs)
        {
            string[] parts = item.Split(',');
            string key = parts[0];

            List<string> partList = new List<string>();
            for(int i = 1; i < parts.Length - 1; i++)
            {
                partList.Add(parts[i]);
            }
            partRule.Add(key,partList);
        }
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

    public void ReadPartFileByIndex()
    {
        // TextAsset partFile = Resources.Load("") as TextAsset;
    }

    void InitBall()
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject initObject = GameObject.Instantiate(prefab, initPos[i].position, Quaternion.identity);
            Ball tempBall = initObject.GetComponent<Ball>();
            balls.Add(tempBall);
            tempBall.canMove = true;

            // 字的逻辑
            if(strsRealIndex >= strsReal.Length)
            {
                if(!isInitPartStr)
                {
                    foreach(var item in poem)
                    {
                        if(!wordCount.ContainsKey(item))
                        {
                            AddItemtoRestWordList(item);
                            isInitPartStr = true;
                        }
                    }


                }


                if(restPartIndex >= restPartStr.Count)
                {
                    restPartIndex = 0;
                    restPartCount++;
                }
                tempBall.myName = restPartStr[restPartIndex];
                restPartIndex++;
                // if(restPartCount > 3)
                // {
                //     Debug.Log("Game Over!");
                //     // lose.SetEnabled(true);
                //     lose.SetActive(true);
                // }
            }
            else
            {
                if(isUseDisturb)
                {
                    tempBall.myName = strs[strsIndex];
                    strsIndex++;
                }
                else
                {
                    tempBall.myName = strsReal[strsRealIndex];
                    strsRealIndex++;
                }
            }


            tempBall.gm = this;
        }
    }

    public void InitBallByTime()
    {
        if(balls.Count == 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                InitBall();
                timer = 2.0f;
            }
        }
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

    private void CheckGameOver(int index)
    {
        switch(index)
        {
            // 步数小于某值
            case 0:
                break;
            // 场景中有多少球
            case 1:
                break;
            // 时间限制
            case 2:
                gameTimer += Time.deltaTime;
                if(gameTimer > 120)
                {
                    Debug.Log("游戏结束！");
                }
                break;
            // 时间限制
            default:
                break;
        }
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
            UpdateWord(wordList, false, ball0.myName);
            UpdateWord(wordList, false, ball1.myName);
            UpdateWord(wordList, true, targetWord);

            ball0.Destroy();
            ball1.Destroy();
            GameObject newObject = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            Ball newball = newObject.GetComponent<Ball>();
            newball.myName = targetWord;
            newball.gm = this;
            audioSource.PlayOneShot(ac, 1F);

            if(ArrayStringContainWord(poem , targetWord))
            {
                newObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                newObject.GetComponent<SpriteRenderer>().color = Color.red;
                newball.isTargetBall = true;
                if(wordCount.ContainsKey(targetWord))
                {
                    wordCount[targetWord] += 1;
                }
                else
                {
                    wordCount.Add(targetWord,1);
                }
            }
            else
            {
                newball.canMove = true;
            }

        }
    }

    public void UpdateTopText(string name)
    {
        uiText += name;
        topText.text = uiText;
    }

    public bool ArrayStringContainWord(string[] array,string word)
    {
        foreach (var item in array)
        {
            if(item == word)
            {
                return true;
            }
        }

        return false;
    }

    public void AddItemtoRestWordList(string key)
    {
        foreach(var item in partRule[key])
        {
            if(item != " " && !wordList.Contains(item))
            {
                restPartStr.Add(item);
            }
            
        }
    }

    public void UpdateWord(List<string> wordlist, bool isInsert, string item)
    {
        if(isInsert)
        {
            if(wordList.Contains(item))
            {
                return;
            }
            wordList.Add(item);
        }
        else
        {
            wordList.Remove(item);
        }
    }
}
