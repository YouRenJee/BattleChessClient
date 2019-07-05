using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;



public static class SaveReadFiles<T>
{
    public static string FilePath = Application.dataPath + "/BoardData/";

    public static void SaveFile(string fileName, T obj)
    {
        using (FileStream fStream = new FileStream(FilePath + fileName, FileMode.Create, FileAccess.ReadWrite))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fStream, obj);
        }
    }

    public static T ReadFile(string fileName)
    {
        try
        {
            using (FileStream fStream = new FileStream(FilePath + fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                return (T)bf.Deserialize(fStream);
            }
        }
        catch
        {

            return default(T);
        }

    }

}
