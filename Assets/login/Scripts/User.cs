

using System;

[Serializable]
public class User
{
    public string userName;

    public string pwd;

    


    public User()
    {

    }

    public User(string userName, string pwd)
    {
        this.userName = userName;
        this.pwd = pwd;
       
    }

}
