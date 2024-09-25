using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class AESUtils:SingleTonMonoBehaviour<AESUtils>
{
    private string logTag = "[AESUtils]:";
    private string _encryptKey = "ABCJHKJMvzZBILhc";
    
    public string EncryptKey
    {
        get { return _encryptKey; }
        set { _encryptKey = value; }
    }

    public void InitEncryptKey(string keystr){
        EncryptKey = keystr;
    }

     public string Encrypt(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return null;
        }
        byte[] contentBytes = Encoding.UTF8.GetBytes(content);
        byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptKey);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = new byte[16]; 
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;
            byte[] encryptedBytes = aes.CreateEncryptor().TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            return Convert.ToBase64String(encryptedBytes, 0, encryptedBytes.Length);
        }

        // ICryptoTransform ict = aes.CreateEncryptor();
        // byte[] resultBytes = ict.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
        // return Convert.ToBase64String(resultBytes, 0, resultBytes.Length);
    }
 
    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="str">密文</param>
    public  string Decrypt(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return "";
        }
        
        byte[] contentBytes = Convert.FromBase64String(content);
        byte[] keyBytes = Encoding.UTF8.GetBytes(EncryptKey);
        using (Aes aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.Mode = CipherMode.ECB;
            aes.IV = new byte[16]; 
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform ict = aes.CreateDecryptor();
            byte[] resultBytes = ict.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            return Encoding.UTF8.GetString(resultBytes);
        }
    }

    // public string DecryptStringAES(string cipherText, string key)
    // {
    //     byte[] keyBytes = Encoding.UTF8.GetBytes(key);
    //     byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

    //     using (Aes aes = Aes.Create())
    //     {
    //         aes.Key = keyBytes;
    //         aes.IV = new byte[16]; // AES needs a 16-byte IV
    //         aes.Padding = PaddingMode.PKCS7;
    //         ICryptoTransform decryptor = aes.CreateDecryptor();

    //         byte[] plainTextBytes = decryptor.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
    //         return Encoding.UTF8.GetString(plainTextBytes);
    //     }
    // }







    public void EncrpytFile(string filePathFull,string outfilePathFull){

        // 读取文件内容字符串
        string configString = File.ReadAllText(filePathFull);
        // 加密
        string encodeConfigString = Encrypt(configString);
        // 写入文件
        File.WriteAllText(outfilePathFull, encodeConfigString);
    }

    public string DecryptFile(string filePathFull){

        // 读取文件内容字符串
        string configString = File.ReadAllText(filePathFull);        
        string str = Decrypt(configString);
        return str;
    }

    // public void EncrpytDirs(string dirPathFull,string outDirPathFull){


    //     string[] files = Directory.GetFiles(dirPathFull);  
    //     //  遍历files
    //     foreach (var file in files)
    //     {

    //         // 读取文件内容字符串
    //         // string configString = File.ReadAllText(file);
    //         // 加密
    //         // string encodeConfigString = Encrypt(configString);
    //         // 写入文件
    //         string fileName = Path.GetFileName(file);
    //         string outfilePathFull = outDirPathFull + "/" + fileName;
    //         Console.WriteLine($"{logTag} EncrpytDirs file : {file} outfilePathFull : {outfilePathFull}");   
    //         EncrpytFile(file,outfilePathFull);
           
    //     }
       
    // }
}