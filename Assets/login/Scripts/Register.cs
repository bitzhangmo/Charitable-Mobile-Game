using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{

    public Button btnRegister;
    public InputField edUserName;
    public InputField edPwd;
    public InputField edPwdRepeat;
    public GameObject panleRegister;
    public Toggle saveUser;

    public MessageBox messageBox;

    private string mUserName;
    private string mPwd;
    private string mPwdRepeat;


    void Start()
    {


    }


    private void FixedUpdate()
    {
        mUserName = edUserName.text;
        mPwd = edPwd.text;
        mPwdRepeat = edPwdRepeat.text;


        if (mUserName.Length > 0 && mPwd.Length > 0 && mPwdRepeat.Length > 0)
        {
            btnRegister.interactable = true;
        }
        else
        {
            btnRegister.interactable = false;
        }


    }

    public void OnBack()
    {
        panleRegister.SetActive(false);
    }


    public void OnRegister()
    {
        User user = new User(mUserName, mPwd);

        if (hashkUserName(mUserName))
        {
            messageBox.setText("该账号已经存在了！！");
            messageBox.show();
            return;
        }

        if (mPwd.Equals(mPwdRepeat))
        {
            if (saveUser.isOn)
            {
                Utils.WriteJsonFile(Utils.FileNameDefault, JsonUtility.ToJson(user));
            }
            else
            {
                Utils.ClearFile(Utils.FileNameDefault);
            }

            Utils.WriteJsonFileAppend(Utils.FileNameUsers, JsonUtility.ToJson(user));
            SceneManager.LoadScene(1);

        }
        else
        {
            messageBox.setText("两次密码不一致！！");
            messageBox.show();
        }


    }


    private bool hashkUserName(string userName)
    {

        User user = new User();
        JsonUtility.FromJsonOverwrite(Utils.ReadJsonFile(Utils.FileNameUsers), user);
        if (user.userName.Equals(userName))
        {
            return true;
        }
        return false;

    }


}
