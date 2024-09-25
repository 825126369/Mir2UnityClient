using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class BitEncrypt
{

// 实现sigleTon
    private static BitEncrypt _instance;
    public static BitEncrypt Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BitEncrypt();
            }
            return _instance;
        }
    }

    private string logTag = "[BitEncrypt]:";
    private string _encryptKey = "ABCJHKJMvzZBILhcTv";

    public string EncryptKey
    {
        get { return _encryptKey; }
        set { _encryptKey = value; }
    }

    public string Decode(string content, string key = null)
    {
        return Code(content, key);
    }

    public string Encode(string content, string key = null)
    {
        return Code(content, key);
    }

    private string Code(string content, string key = null)
    {
        var result = Check(content, key);
        if (result.isOK)
        {
             PrintTool.Log("contentCharCode isOK  :");

            var contentCharCode = new int[content.Length];
            PrintTool.Log("Code content  :", content.Length);
            
            for (int i = 0; i < content.Length; i++)
            {
                contentCharCode[i] = content[i];
            }

            int index = 0;
            string ch = "";
            var regex = new Regex(@"[\w\d_-`~#!$%^&*(){}=+;:'""<,>,/?|\\\u4e00-\u9fa5]");

            PrintTool.Log("contentCharCode content  :", contentCharCode.Length);

            for (int i = 0; i < contentCharCode.Length; i++)
            {
                if (regex.IsMatch(content[i].ToString()))
                {
                    contentCharCode[i] ^= result.key[index];
                    ch = ((char)contentCharCode[i]).ToString();
                    if (regex.IsMatch(ch))
                    {
                        // converted character is still displayable
                    }
                    else
                    {
                        // converted character is not displayable, restore it
                        contentCharCode[i] ^= result.key[index];
                    }

                    index++;
                    if (index >= result.key.Length)
                    {
                        index = 0;
                    }
                }
            }

            string newContent = "";
            for (int i = 0; i < contentCharCode.Length; i++)
            {
                newContent += (char)contentCharCode[i];
            }
            return newContent;
        }
        else
        {
            if (false) Console.WriteLine($"{logTag} encode/decode error content : {content} key : {key}");
            return content;
        }
    }

    private (bool isOK, char[] key) Check(string content, string key = null)
    {
        if (!string.IsNullOrEmpty(content))
        {
            if (!string.IsNullOrEmpty(key))
            {
                // use the provided key for encryption/decryption
                return (true, key.ToCharArray());
            }
            else
            {
                if (!string.IsNullOrEmpty(EncryptKey))
                {
                    return (true, EncryptKey.ToCharArray());
                }
                else
                {
                    return (false, "".ToCharArray());
                }
            }
        }
        else
        {
            return (false, "".ToCharArray());
        }
    }

    public void TestCase(){

        string content = "1234567890";

        string encode = Encode(content);
        string decode = Decode(encode);
        Console.WriteLine($"{logTag} encode : {encode} decode : {decode}");

        // read filecontent
        string  filePathFull = string.Format(Application.dataPath + "/Configs/jianhuan_newbie.json");
        string configString = File.ReadAllText(filePathFull);
        // string encodeConfigString = Encode(configString,key);
        string decodeConfigString = Decode(configString);
        PrintTool.Log(this.logTag, " TestCase :", decodeConfigString );


    }
}