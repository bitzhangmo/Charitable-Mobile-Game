using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectGame2 : MonoBehaviour
{

  

    public ScorePanel scorePanel;

    public Image mImageOne;

    public Sprite mSpriteOne;

    public Image mImageTwo;

    public Sprite mSpriteTwo;

    // Start is called before the first frame update

    void Start()
    {
        if (Utils.ReadPassFlage(1))
        {
            mImageOne.sprite = mSpriteOne;
        }

        if (Utils.ReadPassFlage(2))
        {
            mImageTwo.sprite = mSpriteTwo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnSelectOne()
    {
        SceneManager.LoadScene("level0");
    }

    public void OnSelectTwo()
    {
        SceneManager.LoadScene("level1_test");
    }

    public void OnBack()
    {
        SceneManager.LoadScene("checkpoint");
    }


    public void ShowScore()
    {
        scorePanel.show();
    }

    public void ShowNull()
    {

    }


}
