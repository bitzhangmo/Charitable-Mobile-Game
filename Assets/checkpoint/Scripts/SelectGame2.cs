using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectGame2 : MonoBehaviour
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


    public void OnSelectOne()
    {
        SceneManager.LoadScene("level0");
    }


    public void OnBack()
    {
        SceneManager.LoadScene("checkpoint");
    }


    public void ShowScore()
    {

    }

    public void ShowNull()
    {
        messageBox.setText("敬请期待！！");
        messageBox.show();
    }


}
