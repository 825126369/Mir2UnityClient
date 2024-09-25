
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


class LocalStorageTool
{
    public static string SaveData<T>(T t)
    {
        string key = typeof(T).Name;
        string data = JsonTool.ToJson(t);
        return PlayerPrefs.GetString(key, data);
    }
    
    public static T GetData<T>()
    {
        string key = typeof(T).Name;
        string jsonValue = PlayerPrefs.GetString(key);
        return JsonTool.FromJson<T>(jsonValue);
    }
    
    public static void SetInt(string key, int value){
        PlayerPrefs.SetInt(key,value);
    }
    public static void SetString(string key, string value){
        PlayerPrefs.SetString(key,value);
    }
    public static void SetFloat(string key, float value){
        PlayerPrefs.SetFloat(key,value);
    }

    public static int GetInt(string key, int defaultValue){
        return PlayerPrefs.GetInt(key,defaultValue);
    }

    public static string GetString(string key, string defaultValue){
        return PlayerPrefs.GetString(key,defaultValue);
    }

    public static float GetFloat(string key, float defaultValue){
        return PlayerPrefs.GetFloat(key,defaultValue);
    }

    public static void SetObject<T>(string key, T value)
    {
        PrintTool.Log("SetObject  key: ", key);
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, value);
        byte[] serializedData = stream.ToArray();
        stream.Dispose();
        // Save serialized data to PlayerPrefs
        PlayerPrefs.SetString(key, Convert.ToBase64String(serializedData));
    }
    
    public static bool HaveKey(string key){
        return PlayerPrefs.HasKey(key);
    }
    
    public static T GetObject<T>(string key, T defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            return defaultValue;
        }
        
        string serializedData = PlayerPrefs.GetString(key);
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(Convert.FromBase64String(serializedData));
        T t = (T)formatter.Deserialize(stream);
        stream.Dispose();
        return t;    
    }

    public static void Delete(string key)
    {
        // 调用 PlayerPrefs 来删除数据
        PlayerPrefs.DeleteKey(key);
    }
}
