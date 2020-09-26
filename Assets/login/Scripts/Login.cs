using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Button btnLogin;
   
    public InputField edUserName;
    public InputField edPwd;
    public GameObject panleRegister;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnLogin()
    {
        print("OnLogin");
    }


    public void OnRegister()
    {
        print("OnRegister");
        panleRegister.SetActive(true);

    }

}
