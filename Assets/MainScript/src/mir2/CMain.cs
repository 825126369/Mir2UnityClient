using System;
using System.Diagnostics;
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

        public readonly static Stopwatch Timer = Stopwatch.StartNew();
        public readonly static DateTime StartTime = DateTime.UtcNow;
        public static long Time;
        private static long _cleanTime;
        private static long _drawTime;

        private static long _fpsTime;
        public static int FPS;
        private static int _fps;
        public static int DPS;
        public static int DPSCounter;
        public static bool Shift, Alt, Ctrl, Tilde, SpellTargetLock;
        public static readonly System.Random Random = new System.Random();
        public static long PingTime;
        public static long NextPing = 10000;
        public static double BytesSent, BytesReceived;
        public static KeyBindSettings InputKeys = new KeyBindSettings();

        private void Update()
        {
            UpdateTime();
            UpdateEnviroment();
            UpdateFrameTime();
        }
        
        private static void UpdateEnviroment()
        {
            if (Time >= _cleanTime)
            {
                _cleanTime = Time + 1000;
            }
            
            if (Mir2Settings.DebugMode)
            {
                //CreateDebugLabel();
            }
        }

        private static void UpdateTime()
        {
            Time = Timer.ElapsedMilliseconds;
        }

        private static void UpdateFrameTime()
        {
            if (Time >= _fpsTime)
            {
                _fpsTime = Time + 1000;
                FPS = _fps;
                _fps = 0;

                DPS = DPSCounter;
                DPSCounter = 0;
            }
            else
            {
                _fps++;
            }
        }
    }
}