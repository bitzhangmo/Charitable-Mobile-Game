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
    private User[] users;


    void Start()
    {
        string all = Utils.ReadJsonFile(Utils.FileNameUsers);
        print(all);
        if (all!=null && all.Length > 0)
        {
            users = Utils.FromJson<User>(all);

        }

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
        SceneManager.LoadScene("login");
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

        if (mUserName.Length < 4 ||  mUserName. Length > 12)
        {
            messageBox.setText("账号长度至少4位，至多12位！！");
            messageBox.show();
            return;
        }

        if(mPwd.Length<6 || mPwd.Length > 16)
        {
            messageBox.setText("密码长度至少6位，至多16位！！");
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

            writeToJson(user);
            SceneManager.LoadScene("guides");

        }
        else
        {
            messageBox.setText("两次密码不一致！！");
            messageBox.show();
        }


    }


    private void writeToJson(User user)
    {

        if (users != null)
        {
            int index = users.Length;
            User[] tmp = new User[index + 1];
            users.CopyTo(tmp, 0);
            tmp[index] = user;
            users = tmp;

        }
        else
        {
            users = new User[1];
            users[0] = user;
        }

        Utils.WriteJsonFile(Utils.FileNameUsers, Utils.ToJson<User>(users));
    }


    private bool hashkUserName(string userName)
    {
        if (users != null)
        {
            foreach (User s in users)
            {
                if (s.userName.Equals(userName))
                {
                    return true;
                }
            }
        }

        

        return false;

    }


}
