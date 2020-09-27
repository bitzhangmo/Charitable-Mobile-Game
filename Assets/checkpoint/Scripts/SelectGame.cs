using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectGame : MonoBehaviour
{


    public MessageBox messageBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectOne()
    {
        SceneManager.LoadScene(2);
    }


    public void selectTwo()
    {
        messageBox.setText("敬请期待！！");
        messageBox.show();
    }


    public void selectThree()
    {
        messageBox.setText("敬请期待！！");
        messageBox.show();
    }
}
