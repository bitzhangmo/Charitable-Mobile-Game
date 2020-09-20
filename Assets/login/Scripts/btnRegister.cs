using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btnRegister : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void register () {
        SceneManager.LoadScene(2);//跳转到指定的Level，也就是第一步中的右侧标号
	}


  


}
