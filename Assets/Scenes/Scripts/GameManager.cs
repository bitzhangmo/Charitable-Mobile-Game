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
    public int strsRealIndex = 0;
    public int restPartIndex = 0;
    public int restPartCount = 0;
    public int chooseIndex = 0;
    public int levelIndex = 0;
    public float percent = 0.0f;
    public float maxDistance = 5.0f;
    public float force = 5.0f;
    public float maxSpeed = 5.0f;
    public GameObject prefab;
    public Transform shootPoint;
    // public LineRenderer lineRenderer;
    [Header("UI组件")]
    public Text topText;
    public Text StepCount;
    public int step = 0;
    public GameObject win;
    public GameObject lose;
    
    [Header("音频相关")]
    public AudioClip ac;
    private AudioSource audioSource;

    [Header("关卡数据")]
    public Ball chosenBall;
    public string[] poem = {"花","落","知","多","少"};
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
    public Dictionary<string,string> doubleRule = new Dictionary<string, string>();
    public Dictionary<string,List<string>> partRule = new Dictionary<string, List<string>>();
    public List<string> wordList = new List<string>();
    // public string[] levelPath;

    [Header("预瞄线")]
    [Range(1,5)]
    public int _maxIterations = 3;
    public float _maxDistance = 10f;
    private int _count;
    private LineRenderer _line;

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
        ReadFileByIndex(levelIndex);
        if(levelIndex == 0)
        {
            UpdateWord(wordList, true, "艹");
            UpdateWord(wordList, true, "矢");
            UpdateWord(wordList, true, "夕");
            UpdateWord(wordList, true, "洛");
            UpdateWord(wordList, true, "小");
            UpdateWord(wordList, true, "水");
        }
        else if(levelIndex == 1)
        {
            UpdateWord(wordList,true,"草");
            UpdateWord(wordList,true,"又");
            UpdateWord(wordList,true,"木");
            UpdateWord(wordList,true,"一");
            UpdateWord(wordList,true,"日");
            UpdateWord(wordList,true,"阝");
        }

    }

    // Update is called once per frame
    void Update()
    {
        UserMobileInput();
        InitBallByTime();
        if(wordCount.Count >= 5)
        {
            processIndex = 1;
        }
        if(processIndex == 1 && wordCount.Count <= 0)
        {
            win.SetActive(true);
        }
        CheckGameOver(2);
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
                        if(chosenBall.isTargetBall)
                        {
                            return;
                        }
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
                            _line.SetVertexCount(1);
                            _line.SetPosition(0,target.transform.position);
                            _line.enabled = true;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    endPos = touch.position;
                    direction = startPos - endPos;
                    direction.Normalize();
                    RayCast(target.transform.position, -direction);
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
                        step++;
                        StepCount.text = step.ToString();
                        _line.SetVertexCount(0);
                        _count = 0;
                    }
                    
                    if(balls.Contains(chosenBall))
                    {
                        balls.Remove(chosenBall);
                    }
                    // chosenBall.canMove = false;
                    if(chosenBall != null)
                    {
                        chosenBall.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    break;
            }
        }
    }


    void PushTarget(GameObject target,Vector2 direction,float percent)
    {
        Rigidbody2D rb2D = target.GetComponent<Rigidbody2D>();
        // rb2D.AddForce(direction*percent*force);
        rb2D.velocity = direction*percent;
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
            initObject.SetActive(false);
            balls.Add(tempBall);
            tempBall.canMove = true;

            // 字的逻辑
            if(strsRealIndex >= strsReal.Length)
            {
                if(!isInitPartStr)
                {
                    foreach(var item in poem)
                    {
                        if(doubleRule.ContainsValue(item) || !wordCount.ContainsKey(item))
                        {
                            AddItemtoRestWordList(item);
                            isInitPartStr = true;
                        }
                    }
                }
                if(restPartStr.Count == 0)
                {
                    return;
                }
                if(restPartIndex >= restPartStr.Count)
                {
                    restPartIndex = 0;
                    // restPartCount++;
                }
                tempBall.myName = restPartStr[restPartIndex];
                restPartIndex++;
                Debug.Log("InitObject");
                initObject.SetActive(true);
            }
            else
            {
                tempBall.myName = strsReal[strsRealIndex];
                strsRealIndex++;
                initObject.SetActive(true);
            }


            tempBall.gm = this;
        }
    }

    public void InitBallByTime()
    {
        if(wordCount.Count >= 5 )
        {
            return;
        }
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
            if((myName == "水")&&(otherName == "车") || (myName == "车")&&(otherName == "水"))
                return "";
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
                if(processIndex == 1)
                {
                    // 
                }
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
            Handheld.Vibrate();
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

    public void OnClickRestartButton()
    {
        Application.LoadLevel(levelIndex+3);
    }

    public void RemoveWordCount(string item)
    {
        wordCount.Remove(item);
    }

    // 通过LineRenderer绘制轨迹线
    public bool RayCast(Vector2 position, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, dir, _maxDistance);
        if(hit && _count <= _maxIterations - 1)
        {
            _count++;
            var reflectAngle = Vector2.Reflect(dir, hit.normal);
            _line.SetVertexCount(_count + 1);
            _line.SetPosition(_count, hit.point);
            RayCast(hit.point + reflectAngle, reflectAngle);
            return true;
        }
        else
        {
            _count = 0;
        }

        if(hit == false)
        {
            _line.SetVertexCount(_count + 2);
            _line.SetPosition(_count + 1, position + dir*_maxDistance);
            _count = 0;
        }

        return false;
    }
}
