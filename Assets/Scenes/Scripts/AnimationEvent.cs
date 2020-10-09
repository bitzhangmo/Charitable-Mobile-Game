using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    
    public GameManager gameManager;

    void OnEnterStep1()
    {
        gameManager.processIndex = 1;
    }

    void OnEnterStep2()
    {
        gameManager.processIndex = 2;
        // AudioCallBack audioCallBack = Debug.Log("test");
        if(gameManager.isEduLevel)
        {
            gameManager.callBack = test3;
            test3();
            gameManager.PlayClipData(gameManager.clips[2],gameManager.callBack);
        }

    }

    public void test3()
    {
        gameManager.eduText[1].SetActive(false);
        gameManager.eduText[2].SetActive(true);
    }

}
