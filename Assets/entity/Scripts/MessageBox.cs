using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    // Start is called before the first frame update

    public Text tvText;
    public GameObject self;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setText(string msg)
    {
        tvText.text = msg;
       
    }

    public void OnDismiss()
    {
        self.SetActive(false);
    }


    public void show()
    {
        self.SetActive(true);
    }

}
