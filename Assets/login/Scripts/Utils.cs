
using System.IO;
using UnityEngine;



public class Utils
{

    public static string FileNameUsers = "UserInfos.txt";
    public static string FileNameDefault = "DefaultInfos.txt";

    public static void WriteJsonFile(string fileName, string txt)
    {
        string path = Application.dataPath + fileName;

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            File.WriteAllText(path, txt, System.Text.Encoding.UTF8);
            return;
        }

        //File.AppendAllText(path, txt, System.Text.Encoding.UTF8);
        File.WriteAllText(path, txt, System.Text.Encoding.UTF8);
    }


 


    public static void ClearFile(string fileName)
    {
        string path = Application.dataPath + fileName;

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            File.WriteAllText(path, "", System.Text.Encoding.UTF8);
            return;
        }

        File.WriteAllText(path, "", System.Text.Encoding.UTF8);
    }


    public static string ReadJsonFile(string fileName)
    {
        string path = Application.dataPath + fileName;
        return File.ReadAllText(path, System.Text.Encoding.UTF8);
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



