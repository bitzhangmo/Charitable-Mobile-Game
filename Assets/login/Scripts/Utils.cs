
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


    public static void WriteJsonFileAppend(string fileName, string txt)
    {
        string path = Application.dataPath + fileName;

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            File.WriteAllText(path, txt, System.Text.Encoding.UTF8);
            return;
        }

        File.AppendAllText(path, txt, System.Text.Encoding.UTF8);
       
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


}



