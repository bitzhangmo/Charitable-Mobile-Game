using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Button btnLogin;

    public InputField edUserName;
    public InputField edPwd;
    public GameObject panleRegister;
    public Toggle isSaveDefault;

    private User mUserDefault;
    private User mUser;
    private bool firstTime;
    private User[] users;


    public MessageBox messageBox;


    // Start is called before the first frame update
    void Start()
    {
        firstTime = false;
        string userJson = Utils.ReadJsonFile(Utils.FileNameDefault);



        if (userJson != null && userJson.Length > 0)
        {
            mUserDefault = new User();
            JsonUtility.FromJsonOverwrite(userJson, mUserDefault);
            firstTime = true;
        }

        string all = Utils.ReadJsonFile(Utils.FileNameUsers);
        print(all);

        if (userJson != null && all.Length > 0)
        {
            users = Utils.FromJson<User>(all);

        }


        mUser = new User();

    }

    private void FixedUpdate()
    {
        if (firstTime)
        {
            edUserName.text = mUserDefault.userName;
            edPwd.text = mUserDefault.pwd;
            firstTime = false;
        }

        mUser.userName = edUserName.text;
        mUser.pwd = edPwd.text;


        if (!mUser.IsEmpty())
        {
            btnLogin.interactable = true;
        }
        else
        {
            btnLogin.interactable = false;
        }




    }


    public void OnLogin()
    {

        if (checkUser(mUser))
        {

            if (isSaveDefault.isOn)
            {
                Utils.WriteJsonFile(Utils.FileNameDefault, JsonUtility.ToJson(mUser));
            }
            else
            {
                Utils.ClearFile(Utils.FileNameDefault);
            }

            SceneManager.LoadScene(1);
        }
        else
        {
            messageBox.setText("账号或密码错误！！");
            messageBox.show();
        }




    }


    public void OnRegister()
    {
        panleRegister.SetActive(true);
    }


    private bool checkUser(User user)
    {
        if (user != null && users != null)
        {
            foreach (User s in users)
            {
                if (s.Equal(user))
                {
                    return true;
                }
            }
        }
        return false;

    }

}
