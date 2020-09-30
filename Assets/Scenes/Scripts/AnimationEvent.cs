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
    }

}
