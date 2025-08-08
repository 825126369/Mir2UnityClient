using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Mir2
{
    public class CMain : SingleTonMonoBehaviour<CMain>
    {
        public static Text DebugBaseLabel, HintBaseLabel;
        public static Text DebugTextLabel, HintTextLabel, ScreenshotTextLabel;
        public static Graphics Graphics;
        public static Vector3Int MPoint;
        public readonly static DateTime StartTime = DateTime.UtcNow;
        public static long Time;
        public static bool Shift, Alt, Ctrl, Tilde, SpellTargetLock;
        public static readonly System.Random Random = new System.Random();
        public static long PingTime;
        public static long NextPing = 10000;
        public static double BytesSent, BytesReceived;
        public static KeyBindSettings InputKeys = new KeyBindSettings();
        public static Texture2D[] Cursors;
        public static MouseCursor CurrentCursor = MouseCursor.None;
        public void Init()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            try
            {
                LoadMouseCursors();
                SetMouseCursor(MouseCursor.Default);
                AudioMgr.Instance.Init();
            }
            catch (Exception ex)
            {
                PrintTool.LogError(ex.ToString());
            }
        }

        private void Update()
        {
            UpdateTime();
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.KeypadEnter))
            {
                ToggleFullScreen();
            }
        }

        private void UpdateTime()
        {
            Time = (long)Math.Ceiling(UnityEngine.Time.realtimeSinceStartupAsDouble * 1000);
        }
        
        public static void SetMouseCursor(MouseCursor cursor)
        {
            if (!Mir2Settings.UseMouseCursors) return;

            if (CurrentCursor != cursor)
            {
                CurrentCursor = cursor;
                Cursor.SetCursor(Cursors[(int)cursor], Vector2.zero, CursorMode.Auto);
            }
        }

        private static void LoadMouseCursors()
        {
            Cursors = new Texture2D[8];
            Cursors[(int)MouseCursor.None] = null;
            string path = $"{Mir2Settings.MouseCursorPath}Cursor_Default.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.Default] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_Normal_Atk.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.Attack] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_Compulsion_Atk.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.AttackRed] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_Npc.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.NPCTalk] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_TextPrompt.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.TextPrompt] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_Trash.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.Trash] = LoadCustomCursor(path);
            }

            path = $"{Mir2Settings.MouseCursorPath}Cursor_Upgrade.CUR";
            if (File.Exists(path))
            {
                Cursors[(int)MouseCursor.Upgrade] = LoadCustomCursor(path);
            }
        }

        private static Texture2D LoadCustomCursor(string path)
        {
            byte[] Data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(32, 32); // 初始尺寸无关紧要
            bool success = texture.LoadImage(Data); // 自动识别PNG/JPG等格式
            if(!success)
            {
                PrintTool.LogError("加载失败: " + path);
            }
            return texture;
        }

        public void CreateScreenShot()
        {
            string path = "./Screenshots/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, DateTime.Now.ToString()+ ".png");
            UnityEngine.ScreenCapture.CaptureScreenshot(filePath);
        }

        private static void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

    }
}