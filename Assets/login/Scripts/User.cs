

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


    public bool IsEmpty()
    {
        return userName.Length == 0 || pwd.Length == 0;
    }


    public bool Equal(User other)
    {
        return other != null && userName.Equals(other.userName) && pwd.Equals(other.pwd);
    }

}
