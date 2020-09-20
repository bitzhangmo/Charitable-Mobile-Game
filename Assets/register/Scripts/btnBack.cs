using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void back () {
        SceneManager.LoadScene(0);//跳转到指定的Level，也就是第一步中的右侧标号
	}
    

    
}
