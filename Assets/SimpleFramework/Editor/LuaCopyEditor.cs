using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class LuaCopyEditor
{
	[MenuItem("Tools/Copy Lua")]
	public static void Do()
	{
		// string root = "Assets/Lua";
		// string dest = "Assets/GameAssets/Lua";
		// if (Directory.Exists(dest))
		// {
		// 	Directory.Delete(dest, true);
		// }
		// Directory.CreateDirectory(dest);
		// CloneLuaDirectory(root, dest);

		// AssetDatabase.SaveAssets();
		// AssetDatabase.Refresh();
		// Debug.Log("Copy Lua Success !");
	}

	private static void CloneLuaDirectory(string root, string dest)
	{
		foreach (var directory in Directory.GetDirectories(root))
		{
			string dirName = Path.GetFileName(directory);
			if (!Directory.Exists(Path.Combine(dest, dirName)))
			{
				Directory.CreateDirectory(Path.Combine(dest, dirName));
			}
			CloneLuaDirectory(directory, Path.Combine(dest, dirName));
		}

		foreach (var file in Directory.GetFiles(root))
		{
			if (file.EndsWith(".lua") || file.EndsWith(".pb") || file.EndsWith(".proto"))
			{
				EncodeAndWriteFile(dest, file);
			}
		}
	}

	private static void EncodeAndWriteFile(string destPath, string orifilePath)
	{
		string content = File.ReadAllText(orifilePath, Encoding.UTF8);
		string filePath = Path.Combine(destPath, Path.GetFileName(orifilePath) + ".txt");
		File.WriteAllText(filePath, content, Encoding.UTF8);
	}
}