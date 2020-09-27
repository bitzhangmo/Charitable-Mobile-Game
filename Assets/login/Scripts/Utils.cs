
using System;
using System.IO;
using System.Text;
using UnityEngine;



public class Utils
{

    public static string FileNameUsers = "UserInfos.txt";
    public static string FileNameDefault = "DefaultInfos.txt";





    public static void WriteJsonFile(string fileName, string data)
    {
        string path = "";

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            UnityEngine.Application.productName) + fileName;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.temporaryCachePath, UnityEngine.Application.productName) + fileName;
        }
        else
        {
            path = Application.persistentDataPath + fileName;
        }


        Debug.Log(path);

        File.WriteAllText(path, data, Encoding.UTF8);

    }





    public static void ClearFile(string fileName)
    {
        string path = "";


        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            UnityEngine.Application.productName) + fileName;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.temporaryCachePath, UnityEngine.Application.productName) + fileName;
        }
        else
        {
            path = Application.persistentDataPath + fileName;
        }



        File.WriteAllText(path, "", System.Text.Encoding.UTF8);
    }


    public static string ReadJsonFile(string fileName)
    {
        string path = "";


        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            UnityEngine.Application.productName) + fileName;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.temporaryCachePath, UnityEngine.Application.productName) + fileName;
        }
        else
        {
            path = Application.persistentDataPath + fileName;
        }

        if (File.Exists(path))
        {
            return File.ReadAllText(path, System.Text.Encoding.UTF8);

        }
        return null;


    }






    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }


    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        //Wrapper 在下面 是，已经 序列化的 类， 里面有个 公共 泛型的 Items 物体
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }


    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }



}



