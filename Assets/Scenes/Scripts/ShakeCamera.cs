using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{

    public float shakeLevel = 3f;
    public float setShakeTime = 0.5f;
    public float shakeFPS = 45f;

    private bool isShakeCamera = false;
    private float fps;
    private float shakeTime = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    private Camera selfCamera;

    private void OnEnable() {
        isShakeCamera = true;
        selfCamera = gameObject.GetComponent<Camera>();
        shakeTime = setShakeTime;
        fps = shakeFPS;
        frameTime = 0.03f;
        shakeDelta = 0.005f;   
    }

    private void OnDisable() {
        selfCamera.rect = new Rect(0.0f,0.0f,1.0f,1.0f);
        isShakeCamera = false;    
    }

    // Update is called once per frame
    void Update()
    {
        if(isShakeCamera)
        {
            if(shakeTime > 0)
            {
                shakeTime -= Time.deltaTime;
                if(shakeTime <= 0)
                {
                    enabled = false;
                }
                else
                {
                    frameTime += Time.deltaTime;

                    if(frameTime > 1.0/fps)
                    {
                        frameTime = 0;
                        selfCamera.rect = new Rect(shakeDelta * (-1.0f + shakeLevel * Random.value),shakeDelta * (-1.0f + shakeLevel * Random.value),1.0f,1.0f);
                    }
                }
            }
        }        
    }
}
